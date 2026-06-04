import { Component, inject, OnInit } from '@angular/core';
import { AuthService, CoreModule } from '@abp/ng.core';
import { ParcelService } from '../proxy/shipping-management/parcels/';
import { ParcelStatus } from '../proxy/modules/parcels/parcels';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  standalone: true,
  // استخدام CoreModule بدلاً من LocalizationPipe بمفرده يضمن عمل الـ Pipe والـ Directives الأخرى بأمان
  imports: [CoreModule], 
})
export class HomeComponent implements OnInit {
  private authService = inject(AuthService);
  private parcelService = inject(ParcelService);

  totalParcels = 0;
  inTransitCount = 0;
  deliveredTodayCount = 0;

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  ngOnInit(): void {
    if (!this.hasLoggedIn) return;

    // 1. جلب إجمالي الشحنات (طلب خفيف جداً يرجع العدد فقط)
    this.parcelService.getList({ maxResultCount: 1, skipCount: 0 }).subscribe({
      next: (r) => this.totalParcels = r.totalCount ?? 0,
      error: (err) => console.error('Error fetching total parcels:', err)
    });

    // 2. جلب الشحنات التي قيد النقل
    this.parcelService.getList({ status: ParcelStatus.InTransit, maxResultCount: 1, skipCount: 0 }).subscribe({
      next: (r) => this.inTransitCount = r.totalCount ?? 0,
      error: (err) => console.error('Error fetching in-transit parcels:', err)
    });

    // 3. جلب الشحنات المستلمة لحساب إنتاجية اليوم بأمان
    this.parcelService.getList({ status: ParcelStatus.Delivered, maxResultCount: 1000, skipCount: 0 }).subscribe({
      next: (r) => {
        if (!r?.items) return;
        
        const today = new Date().toDateString();
        this.deliveredTodayCount = r.items.filter(p => 
          p?.creationTime && new Date(p.creationTime).toDateString() === today
        ).length;
      },
      error: (err) => console.error('Error fetching delivered parcels:', err)
    });
  }

  login(): void {
    this.authService.navigateToLogin();
  }
}