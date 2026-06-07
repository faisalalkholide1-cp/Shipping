import type { AssignCourierDto, CourierDashboardDto, CreateParcelDto, MyParcelListFilterDto, ParcelDto, ParcelListFilterDto, ParcelStatusHistoryDto, UpdateParcelDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { CourierLookupItemDto } from '../../modules/parcels/couriers/models';

@Injectable({
  providedIn: 'root',
})
export class ParcelService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  assignCourier = (id: string, input: AssignCourierDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelDto>({
      method: 'POST',
      url: `/api/app/parcel/${id}/assign-courier`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  cancel = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelDto>({
      method: 'POST',
      url: `/api/app/parcel/${id}/cancel`,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateParcelDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelDto>({
      method: 'POST',
      url: '/api/app/parcel',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/parcel/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelDto>({
      method: 'GET',
      url: `/api/app/parcel/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAvailableCouriers = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourierLookupItemDto[]>({
      method: 'GET',
      url: '/api/app/parcel/available-couriers',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: ParcelListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ParcelDto>>({
      method: 'GET',
      url: '/api/app/parcel',
      params: { filter: input.filter, status: input.status, assignedCourierId: input.assignedCourierId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getMyDashboard = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, CourierDashboardDto>({
      method: 'GET',
      url: '/api/app/parcel/my-dashboard',
    },
    { apiName: this.apiName,...config });
  

  getMyParcels = (input: MyParcelListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ParcelDto>>({
      method: 'GET',
      url: '/api/app/parcel/my-parcels',
      params: { status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getStatusHistory = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelStatusHistoryDto[]>({
      method: 'GET',
      url: `/api/app/parcel/${id}/status-history`,
    },
    { apiName: this.apiName,...config });
  

  markDelivered = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelDto>({
      method: 'POST',
      url: `/api/app/parcel/${id}/mark-delivered`,
    },
    { apiName: this.apiName,...config });
  

  markOutForDelivery = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelDto>({
      method: 'POST',
      url: `/api/app/parcel/${id}/mark-out-for-delivery`,
    },
    { apiName: this.apiName,...config });
  

  markPickedUp = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelDto>({
      method: 'POST',
      url: `/api/app/parcel/${id}/mark-picked-up`,
    },
    { apiName: this.apiName,...config });
  

  markReturned = (id: string, reason: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelDto>({
      method: 'POST',
      url: `/api/app/parcel/${id}/mark-returned`,
      params: { reason },
    },
    { apiName: this.apiName,...config });
  

  startTransit = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelDto>({
      method: 'POST',
      url: `/api/app/parcel/${id}/start-transit`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateParcelDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ParcelDto>({
      method: 'PUT',
      url: `/api/app/parcel/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}