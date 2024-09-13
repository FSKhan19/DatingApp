import { AccountService } from './../_services/account.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  users: any;
  apiBaseUrl = 'https://localhost:7274/api';

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {}

  ngOnInit() {
    this.getUsers();
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (userString) {
      const user: User = JSON.parse(userString);
      this.accountService.setCurrentUser(user);
    }
  }

  getUsers() {
    this.http.get(`${this.apiBaseUrl}/Users`).subscribe({
      next: (response) => {
        this.users = response;
      },
      error: (error) => {
        console.log('error => ', error);
      },
    });
  }
}
