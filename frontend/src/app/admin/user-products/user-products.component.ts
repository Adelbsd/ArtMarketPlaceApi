import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Location } from '@angular/common'; 

@Component({
  selector: 'app-user-products',
  templateUrl: './user-products.component.html',
  imports: [CommonModule]
})
export class UserProductsComponent implements OnInit {
  products: any[] = [];
  userId!: number;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private location: Location 
  ) {}

  ngOnInit() {
    this.userId = Number(this.route.snapshot.paramMap.get('id'));
   this.http.get<any[]>(`http://localhost:5009/api/artisans/${this.userId}/produits`)

      .subscribe(data => {
        this.products = data;
      });
  }

  deleteProduct(productId: number) {
    if (confirm('Supprimer ce produit ?')) {
      this.http.delete(`http://localhost:5009/api/Produits/${productId}`)

        .subscribe(() => {
          this.products = this.products.filter(p => p.id !== productId);
        });
    }
  }

  approveProduct(productId: number) {
    this.http.put(`http://localhost:5009/api/admin/products/${productId}/approve`, {})
      .subscribe(() => {
        this.products = this.products.map(p =>
          p.id === productId ? { ...p, statut: 'Approuvé' } : p
        );
      });
  }

  goBack() {
    this.location.back(); 
  }
}
