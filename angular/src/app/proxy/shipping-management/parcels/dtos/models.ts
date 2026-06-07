import type { EntityDto, FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ParcelStatus } from '../../../modules/parcels/parcels/parcel-status.enum';

export interface AssignCourierDto {
  courierId: string;
}

export interface CourierDashboardDto {
  activeParcels?: number;
  deliveredToday?: number;
  totalDelivered?: number;
  returnedParcels?: number;
}

export interface CreateParcelDto {
  senderName: string;
  senderPhone: string;
  receiverName: string;
  receiverPhone: string;
  pickupAddress: string;
  deliveryAddress: string;
  weight?: number;
  price?: number;
}

export interface MyParcelListFilterDto extends PagedAndSortedResultRequestDto {
  status?: ParcelStatus | null;
}

export interface ParcelDto extends FullAuditedEntityDto<string> {
  trackingNumber?: string;
  senderName?: string;
  senderPhone?: string;
  receiverName?: string;
  receiverPhone?: string;
  pickupAddress?: string;
  deliveryAddress?: string;
  weight?: number;
  price?: number;
  status?: ParcelStatus;
  assignedCourierId?: string | null;
}

export interface ParcelListFilterDto extends PagedAndSortedResultRequestDto {
  filter?: string | null;
  status?: ParcelStatus | null;
  assignedCourierId?: string | null;
}

export interface ParcelStatusHistoryDto extends EntityDto<string> {
  status?: ParcelStatus;
  note?: string | null;
  creationTime?: string;
}

export interface TrackingResultDto {
  trackingNumber?: string;
  status?: ParcelStatus;
  receiverName?: string;
  deliveryAddress?: string;
  creationTime?: string;
  lastModificationTime?: string | null;
  history?: ParcelStatusHistoryDto[];
}

export interface UpdateParcelDto {
  senderName: string;
  senderPhone: string;
  receiverName: string;
  receiverPhone: string;
  pickupAddress: string;
  deliveryAddress: string;
  weight?: number;
  price?: number;
}
