import { RoutesService, eLayoutType } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
      {
        path: '/',
        name: 'ShippingMvp::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
      path: '/parcels',
      name: 'Parcels::Menu:Parcels',
      iconClass: 'fas fa-truck',
      layout: eLayoutType.application,
      requiredPolicy: 'ShippingManagement.Parcels',
      order: 2,
    },
    {
      path: '/track',
      name: 'Parcels::Menu:Tracking',
      iconClass: 'fas fa-search-location',
      layout: eLayoutType.application,
      order: 3,
    },
    {
      path: '/my-parcels',
      name: 'Parcels::Menu:MyParcels',
      iconClass: 'fas fa-box',
      layout: eLayoutType.application,
      requiredPolicy: 'ShippingManagement.Parcels.Assign',
      order: 4,
    },
      // Books sample hidden for shipping MVP
      // {
      //   path: '/books',
      //   name: '::Menu:Books',
      //   iconClass: 'fas fa-book',
      //   layout: eLayoutType.application,
      //   requiredPolicy: 'ShippingMvp.Books',
      // },
      
  ]);
}
