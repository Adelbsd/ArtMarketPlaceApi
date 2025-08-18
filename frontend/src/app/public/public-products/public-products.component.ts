import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-public-products',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './public-products.component.html'
})
export class PublicProductsComponent implements OnInit {
  products: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.http.get<any[]>('http://localhost:5009/api/Produits')
      .subscribe({
        next: (data) => this.products = data,
        error: (err) => console.error('Erreur chargement produits', err)
      });
  }
}
