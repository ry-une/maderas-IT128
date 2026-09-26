import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isLoggedIn: boolean = false;
  public redirectUrl: string = "";

  constructor(private http: HttpClient) { }

  // The real LE4 backend (LoginController) returns the JWT as a raw
  // text/plain string, not JSON — responseType 'text' is required or
  // Angular will try (and fail) to JSON-parse the token.
  login(username: string, password: string): Observable<string> {
    return this.http.post(
      "https://localhost:7161/api/Login/login",
      { username, password },
      { responseType: 'text' }
    );
  }
}
