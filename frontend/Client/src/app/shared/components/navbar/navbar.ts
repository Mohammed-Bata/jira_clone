import { Component, Signal, signal } from '@angular/core';
import { AuthService } from '../../../core/services/authservice';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { NotificationsService } from '../../../core/services/notificationsservice';
import { Token } from '@angular/compiler';
import { TokenService } from '../../../core/services/tokenservice';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [AsyncPipe],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  
  isAuthenticated$!: Observable<boolean | null>;
  isClicked = false;
  isAvatarClicked = false;
  user: Signal<{ name: string; email: string } | null>;

  constructor(private router:Router,private authservice:AuthService,public notificationservice:NotificationsService,private tokenservice:TokenService){
    this.isAuthenticated$ = this.authservice.isAuthenticated$;
    this.user = this.tokenservice.user;
    console.log(this.user());
  }

  toggle(){
    this.isClicked = !this.isClicked;

    if(this.isClicked){
      this.notificationservice.loadNotifications();
    }
  }

  toggleAvatar(){
    this.isAvatarClicked = !this.isAvatarClicked;
  }

  logout(){
    this.authservice.logout();
    this.isClicked = false;
    this.isAvatarClicked = false;
  }

  getUserInitials(name: string): string {
    return name.split(' ').map(n => n[0]).join('').toUpperCase();
  }

  gotoLogin(){
    this.router.navigate(['/login']);
  }
  
}
