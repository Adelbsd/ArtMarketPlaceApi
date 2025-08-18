import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Product, ProductService } from '../services/product.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
   imports: [CommonModule,FormsModule],
  
})
export class HomeComponent implements OnInit {
  products: Product[] = [];
search = '';
sortBy = '';
errorMessage: any;

constructor(private productService: ProductService) {}

ngOnInit() {
  this.loadProducts();
}

loadProducts() {
  const params: any = {};
  if (this.search) params.search = this.search;
  if (this.sortBy) params.sortBy = this.sortBy;

  this.productService.getProducts(params).subscribe({
    next: (data) => {
      this.products = data;
    },
    error: (err) => {
      console.error(err);
      this.errorMessage = "Impossible de charger les produits.";
    }
  });
}}

