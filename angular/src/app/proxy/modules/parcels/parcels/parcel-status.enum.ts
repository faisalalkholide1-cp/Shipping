import { mapEnumToOptions } from '@abp/ng.core';

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

export const parcelStatusOptions = mapEnumToOptions(ParcelStatus);
