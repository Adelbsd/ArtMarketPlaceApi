import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5009/api/auth';

  constructor(private http: HttpClient) {}

  login(credentials: { email: string; mdp: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials);
  }

  register(data: {
    nomComplet: string;
    email: string;
    mdp: string;
    role: number;
  }): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  getUserRole(): string | null {
  const token = this.getToken();
  if (!token) return null;

  const decoded: any = jwtDecode(token);
  
  const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  console.log(' Décodage du JWT - rôle détecté :', role);

  const roleMap: { [key: string]: string } = {
    'Admin': 'Admin',
    'Artisan': 'Artisan',
    'Client': 'Customer',
    'Customer': 'Customer',
    'Livreur': 'DeliveryPartner',
    'DeliveryPartner': 'DeliveryPartner'
  };


  return role || null;
}


  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
