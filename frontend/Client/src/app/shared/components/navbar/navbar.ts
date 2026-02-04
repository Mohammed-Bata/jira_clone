import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/authservice';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { NotificationsService } from '../../../core/services/notificationsservice';

@Component({
  selector: 'app-navbar',
  imports: [AsyncPipe],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  
  isAuthenticated$!: Observable<boolean | null>;
  isClicked = false;

  constructor(private authservice:AuthService,public notificationservice:NotificationsService){
    this.isAuthenticated$ = this.authservice.isAuthenticated$;
  }

  toggle(){
    this.isClicked = !this.isClicked;

    if(this.isClicked){
      this.notificationservice.loadNotifications();
    }
  }
  
}
