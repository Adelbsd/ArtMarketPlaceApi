import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = '/api/auth'; 

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

  return decoded['role'] || decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
}
  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');

  }
}
