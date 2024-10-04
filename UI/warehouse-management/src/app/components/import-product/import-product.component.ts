import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { Product } from '../all-products/product';
import { Subscription } from 'rxjs';
import { ClrWizard } from '@clr/angular';
import { ProductService } from '../../services/products.service';
import { InventoryService } from '../../services/inventory.service';
import { Warehouse, WarehouseProduct } from '../warehouses/warehouse';
import { NewTransaction } from '../transaction-history/transaction';
import { TransactionService } from '../../services/transactions.service';
import { InventoryRecord } from '../warehouses/inventory';
import { WarehouseService } from '../../services/warehouses.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-import-product',
  templateUrl: './import-product.component.html',
  styleUrl: './import-product.component.scss',
})
export class ImportProductComponent implements OnInit, OnChanges, OnDestroy {
  @Input() currentWarehouse!: Warehouse;
  allProducts: Product[] = [];
  warehouses: WarehouseProduct[] = [];
  selected: any;
  selectedWarehouse: any;
  selectedTotalAmount: number = 1;
  currentlyAvalailableSpace: number = 0;
  disableNext = true;
  hasAnySpace = false;

  @ViewChild('amountInput') selectedAmount = 1;
  @ViewChild('creationDate') creationDate: any;
  @ViewChild('wizardImport', { static: true }) wizard: ClrWizard | undefined;

  loadingFlag = false;
  success = false;
  allProductsLoading = true;
  warehouseLoading = true;
  errorFlag = false;
  answer: number | null = null;
  open = false;
  compatible = true; // true by default as empty WH won't match any existing product

  prSubscription: Subscription = new Subscription();
  trSubscription: Subscription = new Subscription();
  whSubscription: Subscription = new Subscription();
  invSubscription: Subscription = new Subscription();

  constructor(
    private _productService: ProductService,
    private _inventoryService: InventoryService,
    private _warehouseService: WarehouseService,
    private _transactionService: TransactionService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes['currentWarehouse'].isFirstChange()) {
      this.allProductsLoading = true;
      this.warehouseLoading = true;
      this.currentlyAvalailableSpace =
        this.currentWarehouse.maxCapacity -
        this.currentWarehouse.currentCapacity;
      if (this.currentlyAvalailableSpace > 0) {
        this.hasAnySpace = true;
        this.getAllProducts();
      }
    }
  }

  ngOnInit(): void {
    this.allProductsLoading = true;
    this.warehouseLoading = true;
    this.currentlyAvalailableSpace =
      this.currentWarehouse.maxCapacity - this.currentWarehouse.currentCapacity;
    if (this.currentlyAvalailableSpace > 0) {
      this.hasAnySpace = true;
      this.getAllProducts();
    }
  }

  getAllProducts() {
    this.prSubscription = this._productService
      .getProducts()
      .subscribe((result) => {
        if (result != undefined) {
          this.handleProducts(result as Product[]);
        } else {
          this.allProductsLoading = false;
        }
      });
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
      this.allProducts = productsArray;
      this.allProductsLoading = false;
    } else {
      this.allProductsLoading = false;
    }
  }

  doCancel(): void {
    this.wizard?.close();
    this.ngOnDestroy();
  }

  onCommitPage1(): void {
    // First page validation
    if (this.selected != undefined && this.selectedWarehouse == undefined) {
      // check if product is compatible with warehouse
      this.getProductsForCheck(this.selected.hazardous, this.currentWarehouse);

      if (this.compatible) {
        this.wizard?.forceNext();
        this.loadDataForPageTwo(this.selected);
      } else {
        //show error
      }
    }
  }

  getProductsForCheck(hazardous: 0 | 1, currentWarehouse: Warehouse) {
    this.invSubscription = this._inventoryService
      .getWarehouseInventory(currentWarehouse.warehouseId)
      .subscribe({
        next: (result) =>
          this.checkCompatibility(result as Product[], hazardous),
        error: (error: HttpErrorResponse) => {
          if (error.status == 404) {
            // no products to match with so compatible!
            this.compatible = true;
          } else {
            this.compatible = false;
          }
        },
      });
  }

  checkCompatibility(data: Product[], hazardous: 0 | 1) {
    if (data.findIndex((e) => e.hazardous != hazardous) != -1) {
      this.compatible = false;
    }
  }

  onCommitPage2(): void {
    // Second page validation
    if (this.selectedWarehouse != undefined) {
      this.wizard?.forceNext();
    }
  }

  onCommitPage3(): void {
    // Think of a rollback mechanism

    var s = this.sendAllRequests();
    console.log(s);


    if (!this.success) {
      // show error
      this.loadingFlag = false;
    }
  }

  async sendAllRequests(): Promise<void> {
    this.createInventoryRecord().then(() => {
      this.createTransaction().then(() => {
        this.updateCapacity(
          this.currentWarehouse.warehouseId,
          this.currentWarehouse.currentCapacity + this.selectedTotalAmount
        ).then(() => {
          this.updateCapacity(
            this.selectedWarehouse.warehouse.warehouseId,
            this.selectedWarehouse.warehouse.currentCapacity +
              this.selectedTotalAmount
          ).then(() => (this.success = true));
        });
      });
    });
  }

  createInventoryRecord() {
    let record: InventoryRecord = {
      warehouseId: this.currentWarehouse.warehouseId,
      productId: this.selected.productId,
      amount: this.selectedTotalAmount,
    };

    return new Promise((resolve) => {
      this._inventoryService
        .createInventoryRecord(record)
        .subscribe((result) => {
          resolve(result);
        });
    });
  }

  createTransaction() {
    let transaction: NewTransaction = {
      creationDate: this.creationDate,
      productId: this.selected.productId,
      productName: this.selected.name,
      amount: this.selectedAmount,
      targetWarehouseId: this.currentWarehouse.warehouseId,
      targetWarehouseName: this.currentWarehouse.name,
      sourceWarehouseId: this.selectedWarehouse.warehouse.warehouseId,
      sourceWarehouseName: this.selectedWarehouse.warehouse.name,
    };

    return new Promise((resolve) => {
      this._transactionService
        .createTransaction(transaction)
        .subscribe((data) => {
          resolve(data);
        });
    });
  }

  updateCapacity(warehouseId: string, newCapacity: number) {
    return new Promise((resolve) => {
      this._warehouseService
        .updateCapacity(warehouseId, newCapacity)
        .subscribe((data) => {
          resolve(data);
        });
    });
  }

  loadDataForPageTwo(selected: Product) {
    this.invSubscription = this._inventoryService
      .getWarehousesWithProduct(selected.productId,this.currentWarehouse.warehouseId)
      .subscribe({
        next: (result) => this.handleWarehouses(result as WarehouseProduct[]),
        error: () => {
          this.warehouseLoading = false;
          this.warehouses = [];
        },
      });
  }

  handleWarehouses(data: WarehouseProduct[]) {
    let whArray: WarehouseProduct[] = [];

    if (data.length > 0) {
      data.forEach((element) => {
        let w: Warehouse = {
          warehouseId: element.warehouse.warehouseId,
          name: element.warehouse.name,
          maxCapacity: element.warehouse.maxCapacity,
          currentCapacity: element.warehouse.currentCapacity,
          country: element.warehouse.country,
          zipcode: element.warehouse.zipcode,
          city: element.warehouse.city,
          address: element.warehouse.address,
        };
        let wh: WarehouseProduct = {
          warehouse: w,
          amount: element.amount | 0,
        };
        whArray.push(wh);
      });
      this.warehouses = whArray;
      this.warehouseLoading = false;
    } else {
      this.warehouses = [];
      this.warehouseLoading = false;
    }
  }

  public calculateSpace(n: number): void {
    if (n > 0) {
      this.selectedAmount = n;
      this.selectedTotalAmount = this.selected.unitSize * n;
      if (this.currentlyAvalailableSpace < this.selectedTotalAmount) {
        this.disableNext = false;
      } else {
        this.disableNext = true;
      }
    }
  }

  goBack(): void {
    this.wizard?.previous();
  }

  doReset(): void {
    this.allProducts = [];
    this.warehouses = [];
    this.selected = undefined;
    this.selectedWarehouse = undefined;
    this.wizard?.reset();
  }

  trackProductItemById(product: Product) {
    return product.productId;
  }

  trackWarehouseItemById(warehouse: WarehouseProduct) {
    return warehouse.warehouse.warehouseId;
  }

  ngOnDestroy(): void {
    this.allProducts = [];
    this.warehouses = [];
    this.selected = undefined;
    this.selectedWarehouse = undefined;
    this.selectedTotalAmount = 1;
    this.currentlyAvalailableSpace = 0;
    this.disableNext = true;
    this.hasAnySpace = false;
    this.prSubscription.unsubscribe();
    this.trSubscription.unsubscribe();
    this.invSubscription.unsubscribe();
    this.whSubscription.unsubscribe();
    this.doReset();
  }
}
