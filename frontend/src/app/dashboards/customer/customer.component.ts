import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 

@Component({
  selector: 'app-customer-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  styleUrls: ['./customer.component.scss'],
  templateUrl: './customer.component.html'
  
})
export class CustomerComponent implements OnInit, OnDestroy {
  dashboardData: any;
  errorMessage = '';

  avisNote: number = 5;         
  avisCommentaire: string = ''; 
  produitSelectionne: number | null = null;

  private apiUrl = 'http://localhost:5009/api/customers';
  private avisApi = 'http://localhost:5009/api/avis'; 
  private refreshInterval: any;

  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit(): void {
    const customerId = this.authService.getUserId();
    if (!customerId) {
      this.errorMessage = "⚠️ Aucun client connecté.";
      return;
    }

    this.loadDashboard(customerId);

    this.refreshInterval = setInterval(() => {
      this.loadDashboard(customerId);
    }, 5000);
  }

  ngOnDestroy(): void {
    if (this.refreshInterval) {
      clearInterval(this.refreshInterval);
    }
  }

  private loadDashboard(customerId: number) {
    this.http.get(`${this.apiUrl}/${customerId}/dashboard`).subscribe({
      next: (data) => {
        console.log(" Dashboard Data:", data);
        this.dashboardData = data;
      },
      error: () => {
        this.errorMessage = '❌ Impossible de charger le tableau de bord';
      }
    });
  }


  envoyerAvis(produitId: number) {
    const clientId = this.authService.getUserId();

    const avis = {
      note: this.avisNote,
      commentaire: this.avisCommentaire,
      produitId: produitId,
      clientId: clientId
    };

    this.http.post(this.avisApi, avis).subscribe({
      next: () => {
        alert("✅ Avis ajouté avec succès !");
        this.avisNote = 5;
        this.avisCommentaire = '';
      },
      error: (err) => {
        console.error(err);
        alert("❌ Erreur lors de l'ajout de l'avis");
      }
    });
  }
}
