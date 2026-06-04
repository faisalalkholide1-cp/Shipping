import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ParcelService } from '../proxy/shipping-management/parcels';
import { CreateParcelDto } from '../proxy/shipping-management/parcels/dtos';

@Component({
  selector: 'app-parcel-create',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: '../components/parcel-create.component.html',
})
export class ParcelCreateComponent {

  model: CreateParcelDto = {
    senderName: '',
    senderPhone: '',
    receiverName: '',
    receiverPhone: '',
    pickupAddress: '',
    deliveryAddress: ''
  };

  constructor(private parcelService: ParcelService) {}

  save(): void {

    this.parcelService.create(this.model)
      .subscribe(result => {

        console.log('Created', result);

        this.model = {
          senderName: '',
          senderPhone: '',
          receiverName: '',
          receiverPhone: '',
          pickupAddress: '',
          deliveryAddress: ''
        };
      });
  }
}