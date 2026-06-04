import { Component, inject } from '@angular/core';
import { CouriersService } from '../services/couriers.service';

@Component({
  selector: 'lib-couriers',
  template: ` <p>couriers works!</p> `,
})
export class CouriersComponent {
  protected readonly service = inject(CouriersService);

  constructor() {
    this.service.sample().subscribe(console.log);
  }
}
