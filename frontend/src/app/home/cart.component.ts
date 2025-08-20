import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService } from '../services/cart.service';
import { AuthService } from '../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cart',
  standalone: true,
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss'],
  imports: [CommonModule,FormsModule]
})
export class CartComponent implements OnInit {
  cart: any[] = [];
  total = 0;

  customerId!: number; 

  shippingAddress = '';
  paymentMethod = 'Carte';
  confirmationMessage = '';

  constructor(
    private cartService: CartService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    const id = this.authService.getUserId();
    if (id) {
      this.customerId = Number(id);
      this.loadCart();
    } else {
      console.warn("⚠️ Aucun client connecté, pas de panier disponible.");
    }
  }
isOpen = false;

openCart() {
  this.isOpen = true;
}

closeCart() {
  this.isOpen = false;
}

  loadCart() {
    this.cartService.getCart(this.customerId).subscribe({
      next: (data) => {
        this.cart = data;
        this.total = this.cart.reduce(
          (sum, item) => sum + item.produit.prix * item.quantite,
          0
        );
      },
      error: (err) => {
        console.error("❌ Erreur lors du chargement du panier :", err);
      }
    });
  }

  removeItem(id: number) {
    this.cartService.removeItem(this.customerId, id).subscribe({
      next: () => {
        this.cart = this.cart.filter(item => item.id !== id);
        this.total = this.cart.reduce(
          (sum, item) => sum + item.produit.prix * item.quantite,
          0
        );
      },
      error: (err) => {
        console.error("❌ Erreur suppression :", err);
      }
    });
  }

  checkout() {
    if (!this.shippingAddress.trim()) {
      alert("⚠️ Merci de renseigner une adresse de livraison.");
      return;
    }

    this.cartService.checkout(this.customerId, this.shippingAddress, this.paymentMethod).subscribe({
      next: (res) => {
        this.confirmationMessage = `✅ Commande #${res.commandeId} confirmée. Total : ${res.total} €`;
        this.cart = [];
        this.total = 0;
      },
      error: (err) => {
        console.error(err);
        this.confirmationMessage = "❌ Erreur lors du paiement.";
      }
    });
  }
}
