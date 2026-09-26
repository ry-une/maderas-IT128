import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { TokenStorageService } from 'src/app/services/token-storage.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {

  form: any = {
    username: null,
    password: null
  }

  errorMessage: string = '';

  constructor(
    private authService: AuthService,
    private tokenStorage: TokenStorageService,
    private router: Router
  ) { }

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.authService.isLoggedIn = true;
      this.router.navigate([this.authService.redirectUrl || '/']);
    }
  }

  onSubmit(): void {
    const { username, password } = this.form;

    this.authService.login(username, password).subscribe({
      next: (token: string) => {
        // Real backend returns the raw JWT string only - no user id -
        // so we save the token and do not fabricate/save a user id.
        this.tokenStorage.saveToken(token);
        this.authService.isLoggedIn = true;
        this.errorMessage = '';
        this.router.navigate([this.authService.redirectUrl || '/']);
      },
      error: (err) => {
        console.log(err);
        this.errorMessage = 'Login failed. Please check your username and password.';
      }
    });
  }

}
