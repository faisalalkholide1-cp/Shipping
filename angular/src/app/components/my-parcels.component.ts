import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ParcelService } from '../proxy/shipping-management/parcels';
import { ParcelDto, MyParcelListFilterDto, CourierDashboardDto } from '../proxy/shipping-management/parcels/dtos';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-my-parcels',
  standalone: true,
  templateUrl: './my-parcels.component.html',
  styleUrls: ['./my-parcels.component.scss'],
  imports: [CommonModule, FormsModule],
})
export class MyParcelsComponent implements OnInit {
  private parcelService = inject(ParcelService);
  private confirmation = inject(ConfirmationService);
  private toaster = inject(ToasterService);

  parcels: ParcelDto[] = [];
  isLoading = false;
  actionLoading: Record<string, boolean> = {};

  // dashboard
  stats: CourierDashboardDto | null = null;
  statsLoading = false;

  // filters
  search = '';
  selectedStatus: number | null = null;

  // pagination
  page = 1;
  pageSize = 20;
  totalCount = 0;

  constructor() {}

  ngOnInit(): void {
    this.loadDashboard();
    this.loadParcels();
  }

  loadDashboard(): void {
    this.statsLoading = true;
    this.parcelService.getMyDashboard().subscribe({
      next: res => { this.stats = res; this.statsLoading = false; },
      error: () => { this.statsLoading = false; }
    });
  }

  loadParcels(): void {
    this.isLoading = true;
    const input: MyParcelListFilterDto = {
      status: this.selectedStatus ?? undefined,
      sorting: 'creationTime desc',
      skipCount: (this.page - 1) * this.pageSize,
      maxResultCount: this.pageSize,
    } as any;

    this.parcelService.getMyParcels(input).subscribe({
      next: res => { this.parcels = res.items ?? []; this.totalCount = res.totalCount ?? 0; this.isLoading = false; },
      error: () => { this.isLoading = false; }
    });
  }

  onSearch(): void { this.page = 1; this.loadParcels(); }
  onFilterChange(): void { this.page = 1; this.loadParcels(); }

  // actions
  confirmAndRun(parcelId: string, label: string, action: () => void): void {
    this.confirmation.warn('ShippingMvp::AreYouSure', label).subscribe(status => {
      if (status === 'confirm') action();
    });
  }

  markPickedUp(p: ParcelDto): void {
    this.confirmAndRun(p.id!, 'Mark Picked Up', () => {
      this.actionLoading[p.id!] = true;
      this.parcelService.markPickedUp(p.id!).subscribe({
        next: res => { this.actionLoading[p.id!] = false; this.loadParcels(); this.loadDashboard(); this.toaster.success('Marked as picked up.'); },
        error: () => { this.actionLoading[p.id!] = false; this.toaster.error('Failed to mark picked up.'); }
      });
    });
  }

  startTransit(p: ParcelDto): void {
    this.confirmAndRun(p.id!, 'Start Transit', () => {
      this.actionLoading[p.id!] = true;
      this.parcelService.startTransit(p.id!).subscribe({
        next: res => { this.actionLoading[p.id!] = false; this.loadParcels(); this.loadDashboard(); this.toaster.success('Started transit.'); },
        error: () => { this.actionLoading[p.id!] = false; this.toaster.error('Failed to start transit.'); }
      });
    });
  }

  markDelivered(p: ParcelDto): void {
    this.confirmAndRun(p.id!, 'Mark Delivered', () => {
      this.actionLoading[p.id!] = true;
      this.parcelService.markDelivered(p.id!).subscribe({
        next: res => { this.actionLoading[p.id!] = false; this.loadParcels(); this.loadDashboard(); this.toaster.success('Marked as delivered.'); },
        error: () => { this.actionLoading[p.id!] = false; this.toaster.error('Failed to mark delivered.'); }
      });
    });
  }

  // helpers

  getStatusLabel(status?: number): string {
    const map: Record<number, string> = {
      0: 'Created', 1: 'Assigned', 2: 'PickedUp', 3: 'InTransit', 4: 'OutForDelivery', 5: 'Delivered', 6: 'Cancelled', 7: 'Returned'
    };
    return map[status ?? 0] ?? '';
  }
}
