import { Component, OnInit } from '@angular/core';
import { ProductService, Product } from '../../services/product.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-list.component.html',
  
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  errorMessage = '';

  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.productService.getProducts().subscribe({
      next: (data) => this.products = data,
      error: (err) => this.errorMessage = 'Erreur lors du chargement des produits.'
    });
  }
}