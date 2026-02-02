import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/authservice';
import { ActivatedRoute, Route, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  Form: FormGroup;


  constructor(private fb :FormBuilder,private authservice:AuthService,private route:ActivatedRoute){
    this.Form = this.fb.group({
      name:['',[Validators.required,Validators.minLength(3)]],
      email:['',[Validators.required,Validators.email]],
      password:['',Validators.required]
    })
  }


  handleForm():void{
    if (this.Form.invalid) {
      this.Form.markAllAsTouched();
      return;
    }

    console.log("bad");

    this.Form.markAsUntouched();

  }

  loginWithGoogle(){
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || null;

    console.log('logincomponent',returnUrl);

    this.authservice.loginWithGoogle(returnUrl);
  }
  loginWithMicrosoft(){
    this.authservice.loginWithMicrosoft();
  }
  loginWithGitHub(){
    this.authservice.loginWithGitHub();
  }
}
