import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Product {
  id: number;
  titre: string;
  imageUrl?: string;
  description: string;
  prix: number;
  categorie: string;
  stock: number;
  dateAjout: string; 
  artisanId: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'http://localhost:5009/api'; 

  constructor(private http: HttpClient) {}

  // Récupère tous les produits (pour Admin ou Client)
  getProducts(params?: any): Observable<Product[]> {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key]) {
          httpParams = httpParams.set(key, params[key]);
        }
      });
    }

    return this.http.get<Product[]>(`${this.apiUrl}/produits`, { params: httpParams });
  }

  
  getProductsByArtisan(artisanId: number): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/artisans/${artisanId}/products`);
  }

  // Créer un produit
  createProduct(product: Product, artisanId?: number): Observable<Product> {
    if (artisanId) {
      
      return this.http.post<Product>(`${this.apiUrl}/artisans/${artisanId}/products`, product);
    }
    
    return this.http.post<Product>(`${this.apiUrl}/produits`, product);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/produits/${id}`);
  }
}
