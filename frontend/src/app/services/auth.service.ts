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

  private isBrowser(): boolean {
    return typeof window !== 'undefined' && typeof localStorage !== 'undefined';
  }

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

    try {
      const decoded: any = jwtDecode(token);
      console.log("📜 JWT décodé :", decoded); 

      const rawRole =
        decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
        decoded['role'] ||
        decoded['Role'] ||
        decoded['roles'] ||
        null;

      console.log("🎯 Rôle brut :", rawRole);

      const roleMap: { [key: string]: string } = {
        'Admin': 'Admin',
        'Artisan': 'Artisan',
        'Client': 'Customer',
        'Customer': 'Customer',
        'Livreur': 'DeliveryPartner',
        'DeliveryPartner': 'DeliveryPartner'
      };

      return rawRole ? roleMap[rawRole] || rawRole : null;

    } catch (error) {
      console.error('Erreur lors du décodage du token :', error);
      return null;
    }
  }

  getUserId(): number | null {
  const token = this.getToken();
  if (!token) return null;

  try {
    const decoded: any = jwtDecode(token);
    console.log("🔑 Token décodé :", decoded);

    return (
      decoded["nameid"] ||
      decoded["sub"] ||
      decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] ||
      null
    );
  } catch (error) {
    console.error("Erreur lors du décodage du token :", error);
    return null;
  }
}

  saveToken(token: string): void {
    if (this.isBrowser()) {
      localStorage.setItem('token', token);
    }
  }

  getToken(): string | null {
    if (!this.isBrowser()) return null;
    return localStorage.getItem('token');
  }

  logout(): void {
    if (this.isBrowser()) {
      localStorage.removeItem('token');
      localStorage.removeItem('role');
    }
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
