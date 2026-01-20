import { Injectable } from "@angular/core";
import { jwtDecode } from "jwt-decode";


interface JwtPayload {
  sub: string; // userId
  name: string; 
  email: string;
  exp: number;
}


@Injectable({
  providedIn: 'root',
})

export class TokenService{
    private accessToken:string | null = null;
    
    setToken(accesstoken:string){
        this.accessToken = accesstoken;
    }

    getToken(){
        return this.accessToken;
    }

    clearToken() {
    this.accessToken = null;
  }

    getUserName(): string | any {
    const decoded = this.decodeToken();
    return decoded?.name || null;
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


