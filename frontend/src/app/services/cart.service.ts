import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = 'http://localhost:5009/api/customers';

  constructor(private http: HttpClient) {}

  // ➕ Ajouter au panier
  addToCart(customerId: number, produitId: number, quantite: number = 1): Observable<any> {
    return this.http.post(`${this.apiUrl}/${customerId}/cart/add`, { produitId, quantite });
  }

  // 📦 Voir le panier
  getCart(customerId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${customerId}/cart`);
  }

  // 🗑️ Supprimer un article du panier
  removeItem(customerId: number, cartItemId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${customerId}/cart/${cartItemId}`);
  }

  // 🧹 Vider le panier
  clearCart(customerId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${customerId}/cart/clear`);
  }

  // 💳 Checkout
  checkout(customerId: number, shippingAddress: string, paymentMethod: string = "Carte"): Observable<any> {
    return this.http.post(`${this.apiUrl}/${customerId}/cart/checkout`, { shippingAddress, paymentMethod });
  }
}
