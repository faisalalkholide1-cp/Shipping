import { Component } from '@angular/core';

@Component({
  selector: 'abp-footer',
  template: `
    <div class="lpx-footbar-container end-0">
      <div class="lpx-footbar">
        <div class="lpx-footbar-copyright">
          <i class="bi bi-box-seam me-1"></i>
          <span>{{ currentYear }}© ShippingManagement</span>
        </div>
        <div class="lpx-footbar-solo-links">
          <span class="footer-version">v1.0.0</span>
        </div>
      </div>
    </div>
  `,
})
export class FooterComponent {
  currentYear = new Date().getFullYear();
}