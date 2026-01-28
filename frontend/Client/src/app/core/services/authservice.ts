import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable, tap, throwError } from "rxjs";
import { API_ENDPOINTS } from "../constants/api-endpoints";
import { TokenService } from "./tokenservice";
import { TokenResponse } from "../models/Auth";
import { Router } from "@angular/router";








@Injectable({
  providedIn: 'root',
})

export class AuthService{

    private readonly apiUrl = environment.apiUrl;

    constructor(private http:HttpClient,private tokenservice:TokenService,private router:Router){}

    refresh(): Observable<TokenResponse> {
    return this.http
      .post<TokenResponse>(
        `${this.apiUrl}${API_ENDPOINTS.AUTH.REFRESH}`,
        {},
        {
          withCredentials: true,
        }
      )
      .pipe(
        tap((response) => this.tokenservice.setToken(response.accessToken),),
         catchError((error) => {
        // 🔥 refresh failed → auth is invalid
       this.logout();

        return throwError(() => error);

      }));

  }

 logout(): void {
    console.log('🚪 Logging out...');
    this.http
      .post(
        `${this.apiUrl}${API_ENDPOINTS.AUTH.LOGOUT}`,
        {},
        {
          withCredentials: true,
        }
      )
      .subscribe({
        next: () => {
          this.tokenservice.clearToken();
          console.log('✅ Logout successful');
        },
        error: (error) => {
          console.error('❌ Logout error:', error);
         
        },
      });
  }

    // Google OAuth login - redirect to backend
    loginWithGoogle() {
        window.location.href = `${this.apiUrl}/Users/google/login`;
    }


    // Microsoft OAuth Login
    loginWithMicrosoft(){
        window.location.href = `${this.apiUrl}/Users/microsoft/login`;
    }

    // GitHub OAuth Login
    loginWithGitHub(){
        window.location.href = `${this.apiUrl}/Users/github/login`
    }

    handleOAuth(){

    }
}