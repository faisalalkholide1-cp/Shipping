import { Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModal, NgbModalRef, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

import { ParcelService } from '../proxy/shipping-management/parcels';
import { ParcelsFacade } from '../parcels.facade';
import {
  ParcelDto,
  CreateParcelDto,
  UpdateParcelDto,
  ParcelListFilterDto,
} from '../proxy/shipping-management/parcels/dtos';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { ParcelStatus, parcelStatusOptions } from '../proxy/modules/parcels/parcels';
import { LocalizationPipe } from '@abp/ng.core';
import { ParcelFormComponent } from "./parcel-form-component/parcel-form.component";

@Component({
  selector: 'app-parcels',
  standalone: true,
  templateUrl: './parcels.component.html',
  styleUrls: ['./parcels.component.scss'],
  imports: [CommonModule, NgbModule, FormsModule, LocalizationPipe, ParcelFormComponent],
})
export class ParcelsComponent implements OnInit {
  private parcelService = inject(ParcelService);
  private facade = inject(ParcelsFacade);
  private fb = inject(FormBuilder);
  private modalService = inject(NgbModal);
  private confirmation = inject(ConfirmationService);

  @ViewChild('parcelModal') parcelModal!: TemplateRef<any>;
  @ViewChild('courierModal') courierModal!: TemplateRef<any>;
  @ViewChild('returnModal') returnModal!: TemplateRef<any>;

  // ── Observables from facade ───────────────────────────────────
  public parcels = this.facade.parcels$;
  public couriers = this.facade.couriers$;
  public isLoading = this.facade.isLoading$;
  public isAssigning = this.facade.isAssigning$;

  // Dashboard stats
  public totalCount = this.facade.totalCount$;
  public assignedCount = this.facade.assignedCount$;
  public deliveredCount = this.facade.deliveredCount$;

  // ── Local UI state ───────────────────────────────────────────
  selectedCourierId: string | null = null;
  courierModalRef?: NgbModalRef;
  currentParcelForCourier?: ParcelDto;
  editingParcel?: ParcelDto;


  // ── Pagination ────────────────────────────────────────────────
  currentPage = 1;
  pageSize = 10;

  // ── Filters ───────────────────────────────────────────────────
  filterText = '';
  selectedStatus: ParcelStatus | null = null;

  // ── Parcel Modal ──────────────────────────────────────────────
  modalRef?: NgbModalRef;
  isEditMode = false;
  editingId?: string;
  form!: FormGroup;
  isSaving = false;

  // ── Return Modal ──────────────────────────────────────────────
  returnModalRef?: NgbModalRef;
  returningParcel?: ParcelDto;
  returnReason = '';
  isReturning = false;

  // ── Misc ──────────────────────────────────────────────────────
  activeDropdown: string | null = null;
  ParcelStatus = ParcelStatus;
  statusOptions = parcelStatusOptions;

  // ─────────────────────────────────────────────────────────────

  ngOnInit(): void {
    this.buildForm();

    // use facade observables in template via async pipe

    this.loadParcels();
    this.loadCouriers();
  }

  buildForm(): void {
    this.form = this.fb.group({
      senderName:      ['', Validators.required],
      senderPhone:     ['', Validators.required],
      receiverName:    ['', Validators.required],
      receiverPhone:   ['', Validators.required],
      pickupAddress:   ['', Validators.required],
      deliveryAddress: ['', Validators.required],
      weight:          [null],
      price:           [null],
    });
  }

  // ── Couriers ─────────────────────────────────────────────────
  loadCouriers(): void {
    this.facade.loadCouriers();
  }

  // ── Parcels ───────────────────────────────────────────────────
  loadParcels(): void {
    const filter: ParcelListFilterDto = {
      filter:         this.filterText || undefined,
      status:         this.selectedStatus ?? undefined,
      skipCount:      (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
      sorting:        'creationTime desc',
    };

    this.facade.loadParcels(filter);
  }

  // ── Create / Edit Modal ───────────────────────────────────────
  openCreateModal(): void {
    this.isEditMode = false;
    this.editingId  = undefined;
    this.editingParcel = undefined;
    this.form.reset();
    this.modalRef = this.modalService.open(this.parcelModal, { size: 'lg', centered: true });
  }

  openEditModal(parcel: ParcelDto): void {
    this.isEditMode = true;
    this.editingId  = parcel.id;
    this.editingParcel = parcel;
    this.modalRef = this.modalService.open(this.parcelModal, { size: 'lg', centered: true });
  }

  save(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSaving = true;

    const obs = this.isEditMode
      ? this.parcelService.update(this.editingId!, this.form.value as UpdateParcelDto)
      : this.parcelService.create(this.form.value as CreateParcelDto);

    obs.subscribe({
      next:  () => { this.isSaving = false; this.modalRef?.close(); this.loadParcels(); },
      error: () => (this.isSaving = false),
    });
  }

  onParcelFormSave(payload: CreateParcelDto | UpdateParcelDto): void {
    this.isSaving = true;
    const obs = this.isEditMode
      ? this.parcelService.update(this.editingId!, payload as UpdateParcelDto)
      : this.parcelService.create(payload as CreateParcelDto);

    obs.subscribe({
      next: () => { this.isSaving = false; this.modalRef?.close(); this.loadParcels(); },
      error: () => (this.isSaving = false),
    });
  }

  // ── Courier Modal ─────────────────────────────────────────────
  openCourierModal(parcel: ParcelDto): void {
    this.currentParcelForCourier = parcel;
    this.selectedCourierId       = null;
    this.courierModalRef = this.modalService.open(this.courierModal, { size: 'md', centered: true });
  }

  submitCourierAssignment(): void {
    if (!this.selectedCourierId || !this.currentParcelForCourier?.id) return;

    const courierId = this.selectedCourierId;
    const parcelId = this.currentParcelForCourier.id;

    this.facade.assignCourier(parcelId, courierId).subscribe({
      next: res => { this.courierModalRef?.close(); this.loadParcels(); },
      error: err => console.error('Courier assignment failed:', err),
    });
  }

  // ── Return Modal ──────────────────────────────────────────────
  openReturnModal(parcel: ParcelDto): void {
    this.returningParcel = parcel;
    this.returnReason    = '';
    this.returnModalRef  = this.modalService.open(this.returnModal, { size: 'sm', centered: true });
  }

  confirmReturn(): void {
    if (!this.returnReason.trim() || !this.returningParcel?.id) return;
    this.isReturning = true;

    this.parcelService.markReturned(this.returningParcel.id, this.returnReason).subscribe({
      next:  () => { this.isReturning = false; this.returnModalRef?.close(); this.loadParcels(); },
      error: () => (this.isReturning = false),
    });
  }

  // ── Delete ────────────────────────────────────────────────────
  delete(parcel: ParcelDto): void {
    this.confirmation.warn('ShippingMvp::AreYouSureToDelete', 'ShippingMvp::AreYouSure').subscribe(status => {
      if (status === 'confirm') {
        this.parcelService.delete(parcel.id!).subscribe(() => this.loadParcels());
      }
    });
  }

  // ── Status Actions ────────────────────────────────────────────
  changeStatus(parcel: ParcelDto, action: string): void {
    this.activeDropdown = null;

    if (action === 'assignCourier') { this.openCourierModal(parcel); return; }
    if (action === 'markReturned')  { this.openReturnModal(parcel);  return; }

    let obs;
    switch (action) {
      case 'markPickedUp':       obs = this.parcelService.markPickedUp(parcel.id!);       break;
      case 'startTransit':       obs = this.parcelService.startTransit(parcel.id!);       break;
      case 'markOutForDelivery': obs = this.parcelService.markOutForDelivery(parcel.id!); break;
      case 'markDelivered':      obs = this.parcelService.markDelivered(parcel.id!);      break;
      case 'cancel':             obs = this.parcelService.cancel(parcel.id!);             break;
      default: return;
    }

    obs.subscribe({
      next:  () => this.loadParcels(),
      error: err => console.error('Status update failed:', err),
    });
  }

  // ── Filters & Pagination ──────────────────────────────────────
  toggleDropdown(id: string): void {
    this.activeDropdown = this.activeDropdown === id ? null : id;
  }

  onFilterChange(): void { this.currentPage = 1; this.loadParcels(); }
  onPageChange(page: number): void { this.currentPage = page; this.loadParcels(); }

  totalPages(totalCount: number): number { return Math.ceil(totalCount / this.pageSize); }

  // ── Helpers ───────────────────────────────────────────────────
  getStatusLabel(status?: ParcelStatus): string {
    return ParcelStatus[status ?? 0];
  }

  getStatusClass(status?: ParcelStatus): string {
    const map: Record<number, string> = {
      [ParcelStatus.Created]:        'badge-created',
      [ParcelStatus.Assigned]:       'badge-assigned',
      [ParcelStatus.PickedUp]:       'badge-pickedup',
      [ParcelStatus.InTransit]:      'badge-transit',
      [ParcelStatus.OutForDelivery]: 'badge-outdelivery',
      [ParcelStatus.Delivered]:      'badge-delivered',
      [ParcelStatus.Cancelled]:      'badge-cancelled',
      [ParcelStatus.Returned]:       'badge-returned',
    };
    return map[status ?? 0] ?? '';
  }

  getAvailableActions(status?: ParcelStatus): { key: string; label: string; icon: string; danger?: boolean }[] {
    const map: Partial<Record<ParcelStatus, { key: string; label: string; icon: string; danger?: boolean }[]>> = {
      [ParcelStatus.Created]: [
        { key: 'assignCourier', label: 'Assign Courier',    icon: 'bi-person-check'      },
        { key: 'cancel',        label: 'Cancel',             icon: 'bi-x-circle',  danger: true },
      ],
      [ParcelStatus.Assigned]: [
        { key: 'markPickedUp',  label: 'Mark Picked Up',    icon: 'bi-box-arrow-up'      },
      ],
      [ParcelStatus.PickedUp]: [
        { key: 'startTransit',  label: 'Start Transit',     icon: 'bi-truck'             },
      ],
      [ParcelStatus.InTransit]: [
        { key: 'markOutForDelivery', label: 'Out for Delivery', icon: 'bi-send'           },
        { key: 'markReturned',       label: 'Mark Returned',    icon: 'bi-arrow-return-left', danger: true },
      ],
      [ParcelStatus.OutForDelivery]: [
        { key: 'markDelivered', label: 'Mark Delivered',    icon: 'bi-check2-circle'     },
        { key: 'markReturned',  label: 'Mark Returned',     icon: 'bi-arrow-return-left', danger: true },
      ],
    };
    return map[status ?? 1] ?? [];
  }

  isInvalid(field: string): boolean {
    const ctrl = this.form.get(field);
    return !!(ctrl?.invalid && ctrl?.touched);
  }
}