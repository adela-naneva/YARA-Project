import { Component, OnDestroy, OnInit } from '@angular/core';
import { Warehouse } from './warehouse';
import { max, Subscription } from 'rxjs';
import { WarehouseService } from '../../services/warehouses.service';
import { formatPercent } from '@angular/common';

@Component({
  selector: 'app-warehouses',
  templateUrl: './warehouses.component.html',
  styleUrl: './warehouses.component.scss',
})
export class WarehousesComponent implements OnInit, OnDestroy {
  warehouses: Warehouse[] = [];
  currentWH: Warehouse | undefined;

  subscription: Subscription = new Subscription();

  constructor(private _whService: WarehouseService) {}

  ngOnInit(): void {
    this.getWarehouses();
  }
  getWarehouses() {
    this.subscription = this._whService.getWarehouses().subscribe((result) => {
      if (result != undefined) {
        this.handleWarehouses(result as Warehouse[]);
      }
    });
  }
  handleWarehouses(data: Warehouse[]) {
    let whArray: Warehouse[] = [];

    if (data.length > 0) {
      data.forEach((element) => {
        let wh: Warehouse = {
          warehouseId: element.warehouseId,
          name: element.name,
          maxCapacity: element.maxCapacity,
          currentCapacity: element.currentCapacity,
          fullness: this.calcFullness(
            element.currentCapacity,
            element.maxCapacity
          ),
          country: element.country,
          zipcode: element.zipcode,
          city: element.city,
          address: element.address,
        };
        whArray.push(wh);
      });
      this.warehouses = whArray;
    } else {
      this.warehouses = [];
    }
  }

  selectedWH(wh: Warehouse) {
    this.currentWH = wh;
  }

  calculatePercentage(currentCapacity: number, maxCapacity: number) {
    return formatPercent(currentCapacity / maxCapacity, 'en-US', '1.0-0');
  }

  calcFullness(currentCapacity: number, maxCapacity: number) {
    if (maxCapacity > 0) {
      return (currentCapacity / maxCapacity) * 100;
    }
    return 100; // 100% full
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
