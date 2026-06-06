import type { SampleDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SampleService {
  private restService = inject(RestService);
  apiName = 'Couriers';
  

  get = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, SampleDto>({
      method: 'GET',
      url: '/api/couriers/sample',
    },
    { apiName: this.apiName,...config });
  

  getAuthorized = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, SampleDto>({
      method: 'GET',
      url: '/api/couriers/sample/authorized',
    },
    { apiName: this.apiName,...config });
}