import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-delivery',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './delivery.component.html'
})
export class DeliveryComponent implements OnInit {
  deliveries: any[] = [];
  deliveriesHistory: any[] = [];
  livreurId!: number;

  private apiUrl = 'http://localhost:5009/api/delivery-partners';

  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit(): void {
    const id = this.authService.getUserId();
    if (!id) {
      console.error('Impossible de récupérer l’ID du livreur');
      return;
    }
    this.livreurId = id;

    this.loadDeliveries();
    this.loadHistory();
  }

  // ⚡ Helper pour injecter le token dans les headers
  private getAuthHeaders() {
    const token = this.authService.getToken(); // à implémenter si pas déjà
    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
  }

  // 📦 Livraisons en cours
  loadDeliveries() {
    this.http.get<any[]>(`${this.apiUrl}/${this.livreurId}/orders`, this.getAuthHeaders())
      .subscribe({
        next: (data) => this.deliveries = data,
        error: (err) => console.error('Erreur lors du chargement des livraisons', err)
      });
  }

  // 🕒 Historique
  loadHistory() {
    this.http.get<any[]>(`${this.apiUrl}/${this.livreurId}/history`, this.getAuthHeaders())
      .subscribe({
        next: (data) => this.deliveriesHistory = data,
        error: (err) => console.error('Erreur lors du chargement de l’historique', err)
      });
  }

  // 🔄 Mettre à jour le statut
  updateStatus(orderId: number, newStatus: string) {
    this.http.put(`${this.apiUrl}/orders/${orderId}/status`, { status: newStatus }, this.getAuthHeaders())
      .subscribe({
        next: () => {
          console.log(`Statut mis à jour : ${newStatus}`);
          this.loadDeliveries();
          this.loadHistory();
        },
        error: (err) => console.error('Erreur lors de la mise à jour du statut', err)
      });
  }
}
