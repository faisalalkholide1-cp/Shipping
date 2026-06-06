import { eLayoutType, RoutesService } from '@abp/ng.core';
import {
  EnvironmentProviders,
  inject,
  makeEnvironmentProviders,
  provideAppInitializer,
} from '@angular/core';
import { eCouriersRouteNames } from '../enums/route-names';

export const COURIERS_ROUTE_PROVIDERS = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

export function configureRoutes() {
  const routesService = inject(RoutesService);
  routesService.add([
    {
      path: '/couriers',
      name: eCouriersRouteNames.Couriers,
      iconClass: 'fas fa-book',
      layout: eLayoutType.application,
      order: 3,
    },
  ]);
}

const COURIERS_PROVIDERS: EnvironmentProviders[] = [...COURIERS_ROUTE_PROVIDERS];

export function provideCouriers() {
  return makeEnvironmentProviders(COURIERS_PROVIDERS);
}
