import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users: any;
  apiBaseUrl = 'https://localhost:7274/api';

  constructor(private http: HttpClient) {

  }

  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    this.http.get(`${this.apiBaseUrl}/Users`).subscribe({
      next: response => {
        this.users = response;
      },
      error: error => {
        console.log("error => ", error);
      }
    });
  }

}
