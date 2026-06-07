import type { SampleDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ExampleService {
  private restService = inject(RestService);
  apiName = 'Parcels';
  

  get = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, SampleDto>({
      method: 'GET',
      url: '/api/parcels/example',
    },
    { apiName: this.apiName,...config });
  

  getAuthorized = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, SampleDto>({
      method: 'GET',
      url: '/api/parcels/example/authorized',
    },
    { apiName: this.apiName,...config });
}