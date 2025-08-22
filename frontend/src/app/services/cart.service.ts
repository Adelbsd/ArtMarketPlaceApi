import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = 'http://localhost:5009/api/customers';
  private cartUpdated = new BehaviorSubject<void>(undefined);


  cartUpdated$ = this.cartUpdated.asObservable();

  constructor(private http: HttpClient) {}

  
  addToCart(customerId: number, produitId: number, quantite: number = 1): Observable<any> {
    return this.http.post(`${this.apiUrl}/${customerId}/cart/add`, { produitId, quantite }).pipe(
      tap(() => this.cartUpdated.next()) 
    );
  }

 
  getCart(customerId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${customerId}/cart`);
  }

 
  removeItem(customerId: number, cartItemId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${customerId}/cart/${cartItemId}`).pipe(
      tap(() => this.cartUpdated.next()) 
    );
  }

  
  clearCart(customerId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${customerId}/cart/clear`).pipe(
      tap(() => this.cartUpdated.next()) 
    );
  }

  
  checkout(customerId: number, shippingAddress: string, paymentMethod: string = "Carte"): Observable<any> {
    return this.http.post(`${this.apiUrl}/${customerId}/cart/checkout`, { shippingAddress, paymentMethod }).pipe(
      tap(() => this.cartUpdated.next()) 
    );
  }
}
