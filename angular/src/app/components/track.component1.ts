import { DatePipe, NgClass } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { LocalizationPipe } from '@abp/ng.core';
import { TrackingService } from '../proxy/shipping-management/parcels';
import { TrackingResultDto } from '../proxy/shipping-management/parcels/dtos';
import { ParcelStatus } from '../proxy/modules/parcels/parcels';
// import { ParcelService } from '../services/parcel.service';
// import { ParcelStatus, TrackingResultDto } from '../models/parcel.models';

@Component({
  selector: 'lib-track',
  templateUrl: './track.component.html', 
  imports: [FormsModule, LocalizationPipe, DatePipe, NgClass],
})
export class TrackComponent implements OnInit {
  private readonly parcelService = inject(TrackingService);
  private readonly route = inject(ActivatedRoute);

  trackingNumber = '';
  result: TrackingResultDto | null = null;
  errorMessage = '';
  loading = false;

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const number = params['number'] as string | undefined;
      if (number) {
        this.trackingNumber = number;
        this.search();
      }
    });
  }

  search(): void {
    if (!this.trackingNumber?.trim()) return;
    this.loading = true;
    this.errorMessage = ''; 
    this.result = null;
    this.parcelService.getByTrackingNumber(this.trackingNumber.trim()).subscribe({
      next: res => {
        this.result = res;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Parcels::Parcels:ParcelNotFound';
        this.loading = false;
      },
    });
  }

  statusLabelKey(status: ParcelStatus): string {
    return `Parcels::Enum:ParcelStatus.${status}`;
  }

  statusBadgeClass(status: ParcelStatus): string {
    if (status === ParcelStatus.Delivered) return 'bg-success';
    if (status === ParcelStatus.Cancelled) return 'bg-secondary';
    return 'bg-info';
  }
}
