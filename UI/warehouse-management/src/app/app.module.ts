import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ClarityModule } from '@clr/angular';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AllProductsComponent } from './components/all-products/all-products.component';
import { CreateNewProductComponent } from './components/create-new-product/create-new-product.component';
import { WarehousesComponent } from './components/warehouses/warehouses.component';
import { TransactionHistoryComponent } from './components/transaction-history/transaction-history.component';
import { ProductService } from './services/products.service';
import { provideHttpClient } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { WarehouseService } from './services/warehouses.service';
import { TransactionService } from './services/transactions.service';
import { CurrentStockComponent } from './components/current-stock/current-stock.component';
import { ImportProductComponent } from './components/import-product/import-product.component';
import { ExportProductComponent } from './components/export-product/export-product.component';
import { InventoryService } from './services/inventory.service';

@NgModule({
  declarations: [
    AppComponent,
    AllProductsComponent,
    CreateNewProductComponent,
    WarehousesComponent,
    TransactionHistoryComponent,
    CurrentStockComponent,
    ImportProductComponent,
    ExportProductComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ClarityModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    provideHttpClient(),
    ProductService,
    WarehouseService,
    TransactionService,
    InventoryService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
