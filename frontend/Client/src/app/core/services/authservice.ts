import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, catchError, Observable, of, tap, throwError } from "rxjs";
import { API_ENDPOINTS } from "../constants/api-endpoints";
import { TokenService } from "./tokenservice";
import { TokenResponse } from "../models/Auth";
import { Router } from "@angular/router";
import { NotificationsService } from "./notificationsservice";








@Injectable({
  providedIn: 'root',
})

export class AuthService{

    private readonly apiUrl = environment.apiUrl;

    private isAuthenticatedSubject = new BehaviorSubject<boolean | null>(null);
    public isAuthenticated$: Observable<boolean | null> = this.isAuthenticatedSubject.asObservable();

    constructor(private http:HttpClient,private tokenservice:TokenService,private notificationservice:NotificationsService,private router:Router){}

    get isAuthenticated(): boolean | null {
    return this.isAuthenticatedSubject.value;
    }

    setAuthenticated(value: boolean): void {
      this.isAuthenticatedSubject.next(value);
    }

    refresh(): Observable<TokenResponse|any> {
    return this.http
      .post<TokenResponse|any>(
        `${this.apiUrl}${API_ENDPOINTS.AUTH.REFRESH}`,
        {},
        {
          withCredentials: true,
        }
      )
      .pipe(
        tap((response) => {
          this.tokenservice.setToken(response.accessToken);
          this.isAuthenticatedSubject.next(true);
          //start signalR 
          this.notificationservice.startConnection(response.accessToken);
          this.notificationservice.loadNotifications();
        }),
        catchError((error) => {

        // 🔥 refresh failed → auth is invalid
        this.isAuthenticatedSubject.next(false);
        this.tokenservice.clearToken();
        return of(null);
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
          this.isAuthenticatedSubject.next(false);
          console.log('✅ Logout successful');
        },
        error: (error) => {
          console.error('❌ Logout error:', error);
         
        },
      });
  }

    // Google OAuth login - redirect to backend
    loginWithGoogle(returnUrl:string|null) {

      if(returnUrl !== null){
        localStorage.setItem('returnUrl',returnUrl);
        console.log("authserviceloginbygoogle",localStorage.getItem('returnUrl'));
      }

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