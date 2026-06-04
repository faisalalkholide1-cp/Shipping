import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export enum ParcelStatus {
  Created = 0,
  Assigned = 1,
  PickedUp = 2,
  InTransit = 3,
  OutForDelivery = 4,
  Delivered = 5,
  Cancelled = 6,
  Returned = 7,
}

export interface ParcelDto {
  id: string;
  trackingNumber: string;
  senderName: string;
  senderPhone: string;
  receiverName: string;
  receiverPhone: string;
  pickupAddress: string;
  deliveryAddress: string;
  weight: number;
  price: number;
  status: ParcelStatus;
  assignedCourierId?: string;
  creationTime?: string;
}

export interface CreateParcelDto {
  senderName: string;
  senderPhone: string;
  receiverName: string;
  receiverPhone: string;
  pickupAddress: string;
  deliveryAddress: string;
  weight: number;
  price: number;
}

export type UpdateParcelDto = CreateParcelDto;

export interface ParcelListFilterDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  status?: ParcelStatus;
  assignedCourierId?: string;
}

export interface AssignCourierDto {
  courierId: string;
}

export interface ParcelStatusHistoryDto {
  id?: string;
  status: ParcelStatus;
  note?: string;
  creationTime: string;
}

export interface TrackingResultDto {
  trackingNumber: string;
  status: ParcelStatus;
  receiverName: string;
  deliveryAddress: string;
  creationTime: string;
  lastModificationTime?: string;
  history: ParcelStatusHistoryDto[];
}
