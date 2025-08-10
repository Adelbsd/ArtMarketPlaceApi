import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

import { Router } from '@angular/router';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  imports: [CommonModule]
})
export class AdminComponent implements OnInit {
  users: any[] = [];

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.http.get<any[]>('http://localhost:5009/api/admin/users')
      .subscribe(data => {
        this.users = data;
      });
  }

  deleteUser(id: number) {
    if (confirm('Supprimer cet utilisateur ?')) {
      this.http.delete(`http://localhost:5009/api/admin/users/${id}`)
        .subscribe(() => {
          this.loadUsers();
        });
    }
  }

  viewUserProducts(userId: number) {
    this.router.navigate(['/admin/user-products', userId]);
  }
}
