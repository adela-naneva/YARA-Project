export interface NewProduct {
  name: string;
  hazardous: 0 | 1;
  unitSize: number;
}

export interface Product extends NewProduct {
  productId: string;
}
