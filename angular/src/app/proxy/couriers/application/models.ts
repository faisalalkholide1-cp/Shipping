import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { CourierStatus } from '../domain/courier-status.enum';

export interface CourierListFilterDto extends PagedAndSortedResultRequestDto {
  filter?: string | null;
  zone?: string | null;
  status?: CourierStatus | null;
  isAvailable?: boolean | null;
}

export interface CourierLookupDto {
  id?: string;
  fullName?: string;
  zone?: string;
  isAvailable?: boolean;
}

export interface CourierProfileDto extends FullAuditedEntityDto<string> {
  userId?: string;
  fullName?: string;
  phone?: string;
  email?: string;
  zone?: string;
  status?: CourierStatus;
  isAvailable?: boolean;
  deliveredCount?: number;
}

export interface CreateCourierDto {
  fullName?: string;
  phone?: string;
  email?: string;
  zone?: string;
  password?: string;
}

export interface UpdateCourierDto {
  fullName?: string;
  phone?: string;
  email?: string;
  zone?: string;
}
