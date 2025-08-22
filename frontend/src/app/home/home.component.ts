import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Product, ProductService } from '../services/product.service';
import { CartService } from '../services/cart.service';
import { CartComponent } from './cart.component';
import { AuthService } from '../services/auth.service'; 

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None,
  imports: [CommonModule, FormsModule, CartComponent]
})
export class HomeComponent implements OnInit {
  products: Product[] = [];
  search = '';
  sortBy = '';
  errorMessage: string | null = null;
  cartOpen: boolean = false;

  customerId!: number; 

  constructor(
    private productService: ProductService,
    private cartService: CartService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.loadProducts();

   
    const id = this.authService.getUserId();
    if (id) {
      this.customerId = Number(id);
    } else {
      console.warn("⚠️ Aucun client connecté, fallback sur ID = 1");
      this.customerId = 1;
    }
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
        this.errorMessage = "❌ Impossible de charger les produits.";
      }
    });
  }

  toggleCart() {
    this.cartOpen = !this.cartOpen;
  }

  addToCart(productId: number) {
    if (!this.customerId) {
      alert("⚠️ Vous devez être connecté pour ajouter un produit !");
      return;
    }

    this.cartService.addToCart(this.customerId, productId).subscribe({
      next: () => {
        alert("✅ Produit ajouté au panier !");
      },
      error: (err) => {
        console.error(err);
        alert("❌ Erreur lors de l'ajout au panier");
      }
    });
  }
}
