import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.development";
import { HttpClient } from "@angular/common/http";








@Injectable({
  providedIn: 'root',
})

export class AuthService{

    private readonly apiUrl = environment.apiUrl;


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