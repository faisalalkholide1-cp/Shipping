import { eLayoutType, RoutesService } from '@abp/ng.core';
import {
  EnvironmentProviders,
  inject,
  makeEnvironmentProviders,
  provideAppInitializer,
} from '@angular/core';
import { eParcelsRouteNames } from '../enums/route-names';

export const PARCELS_ROUTE_PROVIDERS = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

export function configureRoutes() {
  const routesService = inject(RoutesService);
  routesService.add([
    // { 
    //   path: '/parcels',
    //   name: 'Parcels::Menu:Parcels',
    //   iconClass: 'fas fa-truck',
    //   layout: eLayoutType.application,
    //   requiredPolicy: 'ShippingManagement.Parcels',
    //   order: 2,
    // },
    // {
    //   path: '/track',
    //   name: 'Parcels::Menu:Tracking',
    //   iconClass: 'fas fa-search-location',
    //   layout: eLayoutType.application,
    //   order: 3,
    // },
  ]);
}

const PARCELS_PROVIDERS: EnvironmentProviders[] = [...PARCELS_ROUTE_PROVIDERS];

export function provideParcels() {
  return makeEnvironmentProviders(PARCELS_PROVIDERS);
}
