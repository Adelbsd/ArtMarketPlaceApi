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
  dateAjout: string; // en ISO string depuis l'API
  artisanId: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'http://localhost:5009/api/produits'; // ⚠️ ton controller utilise "api/produits"

  constructor(private http: HttpClient) {}

  getProducts(params?: any): Observable<Product[]> {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key]) {
          httpParams = httpParams.set(key, params[key]);
        }
      });
    }

    return this.http.get<Product[]>(this.apiUrl, { params: httpParams });
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
