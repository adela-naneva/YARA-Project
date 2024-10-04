import { Product } from '../all-products/product';

export interface NewWarehouse {
  name: string;
  maxCapacity: number;
  currentCapacity: number; // Zero if empty, equal to maxCapacity if full
  fullness?: number;
  country: string;
  zipcode?: string;
  city?: string;
  address?: string;
}

export interface Warehouse extends NewWarehouse {
  warehouseId: string;
}

export interface WarehouseProduct {
  warehouse: Warehouse;
  amount: number;
}
