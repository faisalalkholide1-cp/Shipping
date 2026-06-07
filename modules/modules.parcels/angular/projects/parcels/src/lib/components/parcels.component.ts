import { DatePipe, NgClass } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import {
  ListService,
  LocalizationPipe,
  PagedResultDto,
  PermissionDirective,
} from '@abp/ng.core';
import {
  Confirmation,
  ConfirmationService,
  ModalCloseDirective,
  ModalComponent,
  NgxDatatableDefaultDirective,
  NgxDatatableListDirective,
} from '@abp/ng.theme.shared';
import { ParcelService } from '../services/parcel.service';
import { ParcelDto, ParcelStatus } from '../models/parcel.models';

@Component({
  selector: 'lib-parcels',
  templateUrl: './parcels.component.html',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgxDatatableModule,
    NgbDropdownModule,
    ModalComponent,
    NgxDatatableListDirective,
    NgxDatatableDefaultDirective,
    PermissionDirective,
    ModalCloseDirective,
    LocalizationPipe,
    DatePipe,
    NgClass,
  ],
  providers: [ListService],
})
export class ParcelsComponent implements OnInit {
  readonly list = inject(ListService);
  private readonly parcelService = inject(ParcelService);
  private readonly fb = inject(FormBuilder);
  private readonly confirmation = inject(ConfirmationService);

  readonly parcelStatus = ParcelStatus;
  private readonly defaultCourierId = '00000000-0000-0000-0000-000000000001';

  parcels = { items: [], totalCount: 0 } as PagedResultDto<ParcelDto>;
  statusFilter: ParcelStatus | null = null;
  isModalOpen = false;
  selectedId: string | null = null;
  form!: FormGroup;

  statusOptions = [
    { value: ParcelStatus.Created, labelKey: 'Parcels::Enum:ParcelStatus.0' },
    { value: ParcelStatus.Assigned, labelKey: 'Parcels::Enum:ParcelStatus.1' },
    { value: ParcelStatus.PickedUp, labelKey: 'Parcels::Enum:ParcelStatus.2' },
    { value: ParcelStatus.InTransit, labelKey: 'Parcels::Enum:ParcelStatus.3' },
    { value: ParcelStatus.Delivered, labelKey: 'Parcels::Enum:ParcelStatus.5' },
    { value: ParcelStatus.Cancelled, labelKey: 'Parcels::Enum:ParcelStatus.6' },
  ];

  ngOnInit(): void {
    this.list.hookToQuery(query =>
      this.parcelService.getList({
        ...query,
        status: this.statusFilter ?? undefined,
      })
    ).subscribe(res => (this.parcels = res));
  }

  statusLabelKey(status: ParcelStatus): string {
    return `Parcels::Enum:ParcelStatus.${status}`;
  }

  statusBadgeClass(status: ParcelStatus): string {
    switch (status) {
      case ParcelStatus.Delivered:
        return 'bg-success';
      case ParcelStatus.Cancelled:
      case ParcelStatus.Returned:
        return 'bg-secondary';
      case ParcelStatus.InTransit:
      case ParcelStatus.PickedUp:
        return 'bg-info';
      case ParcelStatus.Assigned:
        return 'bg-warning text-dark';
      default:
        return 'bg-primary';
    }
  }

  openCreate(): void {
    this.selectedId = null;
    this.buildForm();
    this.isModalOpen = true;
  }

  openEdit(row: ParcelDto): void {
    this.selectedId = row.id;
    this.buildForm(row);
    this.isModalOpen = true;
  }

  buildForm(row?: ParcelDto): void {
    this.form = this.fb.group({
      senderName: [row?.senderName ?? '', Validators.required],
      senderPhone: [row?.senderPhone ?? '', Validators.required],
      receiverName: [row?.receiverName ?? '', Validators.required],
      receiverPhone: [row?.receiverPhone ?? '', Validators.required],
      pickupAddress: [row?.pickupAddress ?? '', Validators.required],
      deliveryAddress: [row?.deliveryAddress ?? '', Validators.required],
      weight: [row?.weight ?? 1, [Validators.required, Validators.min(0.1)]],
      price: [row?.price ?? 0, [Validators.required, Validators.min(0)]],
    });
  }

  save(): void {
    if (this.form.invalid) return;
    const req = this.selectedId
      ? this.parcelService.update(this.selectedId, this.form.value)
      : this.parcelService.create(this.form.value);
    req.subscribe(() => {
      this.isModalOpen = false;
      this.list.get();
    });
  }

  delete(id: string): void {
    this.confirmation.warn('ShippingMvp::AreYouSureToDelete', 'ShippingMvp::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.parcelService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  assignCourier(row: ParcelDto): void {
    this.parcelService.assignCourier(row.id, { courierId: this.defaultCourierId }).subscribe(() => this.list.get());
  }

  markPickedUp(id: string): void {
    this.parcelService.markPickedUp(id).subscribe(() => this.list.get());
  }

  startTransit(id: string): void {
    this.parcelService.startTransit(id).subscribe(() => this.list.get());
  }

  markDelivered(id: string): void {
    this.parcelService.markDelivered(id).subscribe(() => this.list.get());
  }

  cancelParcel(id: string): void {
    this.confirmation.warn('Parcels::CancelParcel', 'ShippingMvp::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.parcelService.cancel(id).subscribe(() => this.list.get());
      }
    });
  }
}
