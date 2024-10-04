import { Product } from '../all-products/product';
import { Warehouse } from '../warehouses/warehouse';

export interface NewTransaction {
  creationDate: Date;
  productId: string;
  productName: string;
  amount: number;
  targetWarehouseId?: string;
  targetWarehouseName?: string;
  sourceWarehouseId?: string;
  sourceWarehouseName?: string;
}

export interface Transaction extends NewTransaction {
  transactionId: string;
  type: 'Import' | 'Export';
  sourceWarehouse?: Warehouse;
  targetWarehouse?: Warehouse;
  product?: Product;
}
