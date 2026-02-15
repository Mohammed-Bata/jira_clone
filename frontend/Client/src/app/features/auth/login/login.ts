import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/authservice';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { LoginRequest, RegisterRequest } from '../../../core/models/Auth';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  Form: FormGroup;
  page = "Login";


  constructor(private fb :FormBuilder,private authservice:AuthService,private route:ActivatedRoute,private router:Router){
    this.Form = this.fb.group({
      name:['',[Validators.minLength(3)]],
      email:['',[Validators.required,Validators.email]],
      password:['',Validators.required]
    })
  }

  togglePage(){
    this.page = this.page === 'Login' ? 'Register':'Login';

    if (this.page === 'Login') {
      this.Form.removeControl('name');
    } else {
    this.Form.addControl('name', this.fb.control('', [Validators.required, Validators.minLength(3)]));
    }
  }


  handleForm():void{
    if (this.Form.invalid) {
      this.Form.markAllAsTouched();
      return;
    }

    console.log("bad");

    this.Form.markAsUntouched();

    if(this.page === 'Login'){

      console.log('startlogin');
      const loginRequest:LoginRequest = {
        email: this.Form.value.email,
        password: this.Form.value.password
      }

      this.authservice.login(loginRequest).subscribe({
      next:(response)=>{
        console.log('login success');
        this.router.navigate(['/']);
      },
      error:(err)=>console.log(err.error)
    })

    }else{
      const registerRequest:RegisterRequest = {
        name:this.Form.value.name,
        email: this.Form.value.email,
        password: this.Form.value.password
      }

      this.authservice.register(registerRequest).subscribe({
        next:(response)=>{
          console.log(response);
          this.page = 'Register';
        },
        error:(err)=>console.log(err)
      })
    }


  }

  loginWithGoogle(){
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || null;

    console.log('logincomponent',returnUrl);

    this.authservice.loginWithGoogle(returnUrl);
  }
  loginWithMicrosoft(){
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || null;

    console.log('logincomponent',returnUrl);

    this.authservice.loginWithMicrosoft(returnUrl);
  }
  loginWithGitHub(){
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || null;

    console.log('logincomponent',returnUrl);

    this.authservice.loginWithGitHub(returnUrl);
  }
}
