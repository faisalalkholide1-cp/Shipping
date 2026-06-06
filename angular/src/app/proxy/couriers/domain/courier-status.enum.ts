import { mapEnumToOptions } from '@abp/ng.core';

export enum CourierStatus {
  Active = 0,
  Inactive = 1,
  Busy = 2,
}

export const courierStatusOptions = mapEnumToOptions(CourierStatus);
