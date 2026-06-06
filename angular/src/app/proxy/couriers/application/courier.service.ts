import type { CourierListFilterDto, CourierLookupDto, CourierProfileDto, CreateCourierDto, UpdateCourierDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { CourierStatus } from '../domain/courier-status.enum';

@Injectable({
  providedIn: 'root',
})
export class CourierService {
  private restService = inject(RestService);
  apiName = 'Couriers';
  

  create = (input: CreateCourierDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourierProfileDto>({
      method: 'POST',
      url: '/api/couriers/courier',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/couriers/courier/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourierProfileDto>({
      method: 'GET',
      url: `/api/couriers/courier/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAvailableLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourierLookupDto[]>({
      method: 'GET',
      url: '/api/couriers/courier/available-lookup',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: CourierListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CourierProfileDto>>({
      method: 'GET',
      url: '/api/couriers/courier',
      params: { filter: input.filter, zone: input.zone, status: input.status, isAvailable: input.isAvailable, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  setAvailability = (id: string, isAvailable: boolean, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourierProfileDto>({
      method: 'POST',
      url: `/api/couriers/courier/${id}/set-availability`,
      params: { isAvailable },
    },
    { apiName: this.apiName,...config });
  

  setStatus = (id: string, status: CourierStatus, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourierProfileDto>({
      method: 'POST',
      url: `/api/couriers/courier/${id}/set-status`,
      params: { status },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateCourierDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourierProfileDto>({
      method: 'PUT',
      url: `/api/couriers/courier/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}