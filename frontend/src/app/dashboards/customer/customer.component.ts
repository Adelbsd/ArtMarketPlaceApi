import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customer-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './customer.component.html'
})
export class CustomerComponent implements OnInit {
  dashboardData: any;
  errorMessage = '';

  private apiUrl = 'http://localhost:5009/api/customers';

  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit(): void {
    const customerId = this.authService.getUserId();
    this.http.get(`${this.apiUrl}/${customerId}/dashboard`).subscribe({
      next: data => this.dashboardData = data,
      error: () => this.errorMessage = 'Impossible de charger le tableau de bord'
    });
  }
}
