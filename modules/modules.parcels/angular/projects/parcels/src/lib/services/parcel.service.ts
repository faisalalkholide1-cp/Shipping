import { Injectable } from '@angular/core';
import { RestService, PagedResultDto } from '@abp/ng.core';
import type {
  AssignCourierDto,
  CreateParcelDto,
  ParcelDto,
  ParcelListFilterDto,
  TrackingResultDto,
  UpdateParcelDto,
} from '../models/parcel.models';

@Injectable({ providedIn: 'root' })
export class ParcelService {
  apiName = 'Parcels';

  constructor(private restService: RestService) {}

  get = (id: string) =>
    this.restService.request<void, ParcelDto>(
      { method: 'GET', url: `/api/app/parcels/parcel/${id}` },
      { apiName: this.apiName }
    );

  getList = (input: ParcelListFilterDto) =>
    this.restService.request<void, PagedResultDto<ParcelDto>>(
      {
        method: 'GET',
        url: '/api/app/parcels/parcel',
        params: input,
      },
      { apiName: this.apiName }
    );

  create = (input: CreateParcelDto) =>
    this.restService.request<CreateParcelDto, ParcelDto>(
      { method: 'POST', url: '/api/app/parcels/parcel', body: input },
      { apiName: this.apiName }
    );

  update = (id: string, input: UpdateParcelDto) =>
    this.restService.request<UpdateParcelDto, ParcelDto>(
      { method: 'PUT', url: `/api/app/parcels/parcel/${id}`, body: input },
      { apiName: this.apiName }
    );

  delete = (id: string) =>
    this.restService.request<void, void>(
      { method: 'DELETE', url: `/api/app/parcels/parcel/${id}` },
      { apiName: this.apiName }
    );

  assignCourier = (id: string, input: AssignCourierDto) =>
    this.restService.request<AssignCourierDto, ParcelDto>(
      { method: 'POST', url: `/api/app/parcels/parcel/${id}/assign-courier`, body: input },
      { apiName: this.apiName }
    );

  markPickedUp = (id: string) =>
    this.restService.request<void, ParcelDto>(
      { method: 'POST', url: `/api/app/parcels/parcel/${id}/mark-picked-up` },
      { apiName: this.apiName }
    );

  startTransit = (id: string) =>
    this.restService.request<void, ParcelDto>(
      { method: 'POST', url: `/api/app/parcels/parcel/${id}/start-transit` },
      { apiName: this.apiName }
    );

  markDelivered = (id: string) =>
    this.restService.request<void, ParcelDto>(
      { method: 'POST', url: `/api/app/parcels/parcel/${id}/mark-delivered` },
      { apiName: this.apiName }
    );

  cancel = (id: string) =>
    this.restService.request<void, ParcelDto>(
      { method: 'POST', url: `/api/app/parcels/parcel/${id}/cancel` },
      { apiName: this.apiName }
    );

  getByTrackingNumber = (trackingNumber: string) =>
    this.restService.request<void, TrackingResultDto>(
      {
        method: 'GET',
        url: '/api/app/parcels/tracking/by-tracking-number',
        params: { trackingNumber },
      },
      { apiName: this.apiName }
    );
}
