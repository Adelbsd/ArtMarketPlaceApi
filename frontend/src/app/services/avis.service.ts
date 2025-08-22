import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AvisService {
  private apiUrl = 'http://localhost:5009/api/avis';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders() {
    const token = this.authService.getToken();
    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
  }

  ajouterAvis(avis: any): Observable<any> {
    return this.http.post(this.apiUrl, avis, this.getAuthHeaders());
  }

  getAvisParProduit(produitId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/produit/${produitId}`);
  }
}
