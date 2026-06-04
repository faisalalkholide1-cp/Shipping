import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44363/',
  redirectUri: baseUrl,
  clientId: 'ShippingMvp_App',
  responseType: 'code',
  scope: 'offline_access ShippingMvp',
  requireHttps: true,
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'ShippingMvp',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44363',
      rootNamespace: 'ShippingMvp',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
    Parcels: {
      url: 'https://localhost:44363',
      rootNamespace: 'Modules.Parcels',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge'
  }
} as Environment;
