import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {

  form: any = {
    username: null,
    password: null,
    firstName: null,
    lastName: null
  }

  successMessage: string = '';
  errorMessage: string = '';

  constructor(
    private http: HttpClient,
    private route: Router
  ) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    const {
      username, password, firstName, lastName
    } = this.form

    console.log(this.form);

    this.http.post(
      "https://localhost:7161/api/Login/register",
      this.form,
      { responseType: 'text' }
    ).subscribe({
      next: (data) => {
        console.log(data);
        this.route.navigate(['/login']);
      },
      error: (err) => {
        console.log(err);
        this.errorMessage = 'Registration failed. Please try a different username.';
      }
    })
  }

}
