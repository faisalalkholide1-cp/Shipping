import type { TrackingResultDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TrackingService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  getByTrackingNumber = (trackingNumber: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TrackingResultDto>({
      method: 'GET',
      url: '/api/app/tracking/by-tracking-number',
      params: { trackingNumber },
    },
    { apiName: this.apiName,...config });
}