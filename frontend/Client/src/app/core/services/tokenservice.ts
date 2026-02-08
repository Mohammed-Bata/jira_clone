import { Injectable, signal } from "@angular/core";
import { jwtDecode } from "jwt-decode";


interface JwtPayload {
  sub: string; // userId
  unique_name: string; 
  email: string;
  exp: number;
}


@Injectable({
  providedIn: 'root',
})

export class TokenService{
    private accessToken:string | null = null;
    private _user = signal<{name:string, email:string} | null>(null);
    user = this._user.asReadonly();
    
    setToken(accesstoken:string){
        this.accessToken = accesstoken;
        this._user.set({
            name: this.getUserName(),
            email: this.getUserEmail()!
        });
    }

    getToken(){
        return this.accessToken;
    }

    clearToken() {
    this.accessToken = null;
    this._user.set(null);
    }

    getUserName(): string | any {
    const decoded = this.decodeToken();
    return decoded?.unique_name || null;
  }

  getUserId(): string | any {
    const decoded = this.decodeToken();
    return decoded?.sub || null;
  }

  getUserEmail(): string | null {
    const decoded = this.decodeToken();
    return decoded?.email || null;
  }

  willExpireSoon(){
    const decoded = this.decodeToken();
    if(!decoded?.exp){
        return true;
    }

    const expiry = decoded.exp * 1000;
    return Date.now() > expiry - 90 * 1000;
  }

    decodeToken() {
    const token = this.accessToken

    if (!token) {
      return;
    }

    try {
      return jwtDecode<JwtPayload>(token);
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }


}


