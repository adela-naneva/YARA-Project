import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { Product } from '../all-products/product';
import { Subscription } from 'rxjs';
import { InventoryService } from '../../services/inventory.service';

@Component({
  selector: 'app-current-stock',
  templateUrl: './current-stock.component.html',
  styleUrl: './current-stock.component.scss',
})
export class CurrentStockComponent implements OnInit, OnChanges, OnDestroy {
  products: Product[] = [];
  loading = true;
  @Input() currentWarehouseId: string = '';

  subscription: Subscription = new Subscription();

  constructor(private _inventoryService: InventoryService) {}

  ngOnInit() {
    this.loading = true;
    this.getInventoryProducts();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes['currentWarehouseId'] &&
      !changes['currentWarehouseId'].isFirstChange() &&
      changes['currentWarehouseId'].previousValue !=
        changes['currentWarehouseId'].currentValue
    ) {
      this.getInventoryProducts();
    }
  }

  getInventoryProducts() {
    if (this.currentWarehouseId && this.currentWarehouseId !== '') {
      this.subscription = this._inventoryService
        .getWarehouseInventory(this.currentWarehouseId)
        .subscribe({
          next: (result) => this.handleProducts(result as Product[]),
          error: () => {
            this.loading = false;
            this.products = [];
          },
        });
    }
  }

  handleProducts(data: Product[]) {
    let productsArray: Product[] = [];

    if (data.length > 0) {
      data.forEach((element) => {
        let p: Product = {
          name: element.name,
          productId: element.productId,
          unitSize: element.unitSize,
          hazardous: element.hazardous,
        };
        productsArray.push(p);
      });
      this.products = productsArray;
      this.loading = false;
    } else {
      this.loading = false;
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
