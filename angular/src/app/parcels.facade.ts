import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, forkJoin, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { CourierService } from './proxy/couriers/application/courier.service';
import { ParcelService } from './proxy/shipping-management/parcels';
import type { CourierLookupDto } from './proxy/couriers/application/models';
import type { ParcelDto, ParcelListFilterDto } from './proxy/shipping-management/parcels/dtos';
import { PagedResultDto } from '@abp/ng.core';
import { ParcelStatus } from './proxy/modules/parcels/parcels';

export interface CourierVM {
  id?: string;
  fullName?: string;
  zone?: string;
  isAvailable?: boolean;
  // local-only fields
  deliveredCount?: number;
  workload?: number;
  badgeColor?: string;
  disabled?: boolean;
}

@Injectable({ providedIn: 'root' })
export class ParcelsFacade {
  // State subjects
  private couriersSubject = new BehaviorSubject<CourierVM[]>([]);
  public couriers$ = this.couriersSubject.asObservable();

  private parcelsSubject = new BehaviorSubject<PagedResultDto<ParcelDto> | null>(null);
  public parcels$ = this.parcelsSubject.asObservable();

  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  public isLoading$ = this.isLoadingSubject.asObservable();

  private isAssigningSubject = new BehaviorSubject<boolean>(false);
  public isAssigning$ = this.isAssigningSubject.asObservable();

  // Dashboard stats subjects
  private totalCountSubject = new BehaviorSubject<number>(0);
  public totalCount$ = this.totalCountSubject.asObservable();

  private assignedCountSubject = new BehaviorSubject<number>(0);
  public assignedCount$ = this.assignedCountSubject.asObservable();

  private deliveredCountSubject = new BehaviorSubject<number>(0);
  public deliveredCount$ = this.deliveredCountSubject.asObservable();

  // Caches
  private couriersMap = new Map<string, CourierVM>();
  private parcelsMap = new Map<string, ParcelDto>();

  constructor(
    private courierService: CourierService,
    private parcelService: ParcelService
  ) {}

  // Load couriers + an initial parcels page to compute workload/delivered counts
  loadCouriers(): void {
    this.isLoadingSubject.next(true);

    // Load couriers and parcels in parallel (single calls)
    forkJoin({
      couriers: this.courierService.getAvailableLookup(),
      parcels: this.parcelService.getList({ skipCount: 0, maxResultCount: 1000 } as ParcelListFilterDto),
    })
      .pipe(
        tap(() => this.isLoadingSubject.next(false)),
        catchError(err => {
          this.isLoadingSubject.next(false);
          console.error('Failed to load couriers or parcels', err);
          return of({ couriers: [], parcels: { items: [], totalCount: 0 } as any });
        })
      )
      .subscribe((res: any) => {
        const couriers: CourierLookupDto[] = res.couriers || [];
        const parcelsPaged: PagedResultDto<ParcelDto> = res.parcels || { items: [], totalCount: 0 };

        // build parcel cache
        this.parcelsMap.clear();
        parcelsPaged.items?.forEach(p => {
          if (p && p.id) this.parcelsMap.set(p.id, p);
        });
        this.parcelsSubject.next(parcelsPaged);

        // compute workload & delivered counts per courier
        const workloadMap = new Map<string, number>();
        const deliveredMap = new Map<string, number>();

        parcelsPaged.items?.forEach(p => {
          const cId = p.assignedCourierId?.toString();
          if (!cId) return;
          const w = workloadMap.get(cId) ?? 0;
          workloadMap.set(cId, w + 1);
        });

        // build CourierVM list and caches
        this.couriersMap.clear();
        const vms: CourierVM[] = (couriers || []).map(c => {
          const id = c.id?.toString();
          const workload = id ? (workloadMap.get(id) ?? 0) : 0;
          const delivered = id ? (deliveredMap.get(id) ?? 0) : 0;
          const vm: CourierVM = {
            id: c.id,
            fullName: c.fullName,
            zone: c.zone,
            isAvailable: c.isAvailable,
            workload,
            deliveredCount: delivered,
            badgeColor: c.isAvailable ? 'success' : 'secondary',
            disabled: !c.isAvailable,
          };
          if (id) this.couriersMap.set(id, vm);
          return vm;
        });

        // sort: available first, then by fullName
        vms.sort((a, b) => {
          const av = (b.isAvailable ? 1 : 0) - (a.isAvailable ? 1 : 0);
          if (av !== 0) return av;
          return (a.fullName || '').localeCompare(b.fullName || '');
        });

        this.couriersSubject.next(vms);

        // Emit dashboard stats based on loaded parcels
        const total = parcelsPaged.totalCount ?? (parcelsPaged.items?.length ?? 0);
        const assigned = (parcelsPaged.items || []).filter(p => !!p.assignedCourierId).length;
        const delivered = (parcelsPaged.items || []).filter(p => p.status === ParcelStatus.Delivered).length;
        this.totalCountSubject.next(total);
        this.assignedCountSubject.next(assigned);
        this.deliveredCountSubject.next(delivered);
      });
  }

  // Load parcels using given filter (delegates to parcel service)
  loadParcels(filter: ParcelListFilterDto): void {
    this.isLoadingSubject.next(true);
    this.parcelService.getList(filter).pipe(
      tap(() => this.isLoadingSubject.next(false)),
      catchError(err => {
        this.isLoadingSubject.next(false);
        console.error('Failed to load parcels', err);
        return of({ items: [], totalCount: 0 } as PagedResultDto<ParcelDto>);
      })
    ).subscribe(paged => {
      // Update parcel cache
      this.parcelsMap.clear();
      paged.items?.forEach(p => { if (p && p.id) this.parcelsMap.set(p.id, p); });
      this.parcelsSubject.next(paged);

      // Update dashboard stats from paged result (server is source of truth for total)
      const total = paged.totalCount ?? (paged.items?.length ?? 0);
      const assigned = (paged.items || []).filter(p => !!p.assignedCourierId).length;
      const delivered = (paged.items || []).filter(p => p.status === ParcelStatus.Delivered).length;
      this.totalCountSubject.next(total);
      this.assignedCountSubject.next(assigned);
      this.deliveredCountSubject.next(delivered);

      // Optionally update workload map from this page
      // (Not adjusting couriers list here to keep UI flow simple)
    });
  }

  // Assign courier orchestration
  assignCourier(parcelId: string, courierId: string): Observable<ParcelDto> {
    const courierVm = this.couriersMap.get(courierId);
    if (!courierVm) {
      return of(null as any);
    }
    if (!courierVm.isAvailable) {
      // Return an observable null result to indicate unavailable
      return of(null as any);
    }

    this.isAssigningSubject.next(true);
    return this.parcelService.assignCourier(parcelId, { courierId } as any).pipe(
      tap((updatedParcel: ParcelDto) => {
        this.isAssigningSubject.next(false);
        if (!updatedParcel) return;

        // Update parcel cache
        if (updatedParcel.id) this.parcelsMap.set(updatedParcel.id, updatedParcel);

        // Update workloads: increment new courier (best-effort)
        const newId = updatedParcel.assignedCourierId?.toString();
        if (newId) {
          const vm = this.couriersMap.get(newId);
          if (vm) {
            vm.workload = (vm.workload ?? 0) + 1;
            this.couriersMap.set(newId, vm);
          }
        }

        // Emit updated couriers list
        const updatedList = Array.from(this.couriersMap.values()).sort((a, b) => {
          const av = (b.isAvailable ? 1 : 0) - (a.isAvailable ? 1 : 0);
          if (av !== 0) return av;
          return (a.fullName || '').localeCompare(b.fullName || '');
        });
        this.couriersSubject.next(updatedList);

        // Recompute dashboard stats based on cached parcels
        const assignedSum = Array.from(this.parcelsMap.values()).filter(p => !!p.assignedCourierId).length;
        const deliveredSum = Array.from(this.parcelsMap.values()).filter(p => p.status === ParcelStatus.Delivered).length;
        const totalSum = this.totalCountSubject.getValue ? this.totalCountSubject.getValue() : this.parcelsMap.size;
        this.assignedCountSubject.next(assignedSum);
        this.deliveredCountSubject.next(deliveredSum);
        this.totalCountSubject.next(totalSum);
      }),
      catchError(err => {
        this.isAssigningSubject.next(false);
        console.error('Assign courier failed', err);
        throw err;
      })
    );
  }

  // Expose helper to get courier VM by id
  getCourierById(id?: string): CourierVM | undefined {
    if (!id) return undefined;
    return this.couriersMap.get(id.toString());
  }
}
