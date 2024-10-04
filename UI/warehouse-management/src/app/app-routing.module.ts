import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllProductsComponent } from './components/all-products/all-products.component';
import { WarehousesComponent } from './components/warehouses/warehouses.component';

const routes: Routes = [
  { path: 'products', component: AllProductsComponent },
  { path: 'warehouses', component: WarehousesComponent },
  { path: '',   redirectTo: '/products', pathMatch: 'full' },
  { path: '**', redirectTo: '/products', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
