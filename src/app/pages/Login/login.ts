import { Component, inject, OnInit } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { email } from '@angular/forms/signals';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../service/auth';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  imports: [MatInputModule, MatButtonModule, MatCardModule, MatFormFieldModule, ReactiveFormsModule,MatIconModule,RouterLink, MatSnackBarModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  matSnackBar = inject(MatSnackBar);
  authService = inject(Auth);
  router = inject(Router)
  form!: FormGroup;
  fb = inject(FormBuilder);

  Login(){
    this.authService.login(this.form.value).subscribe({
      next:(response)=>
    {
      //console.log(response);
        this.matSnackBar.open(response.message,'Close',{
           duration:3000,
          horizontalPosition:'center', 
        });
        this.router.navigate(['/']);
    }
    
    });
  }
   ngOnInit(): void {
    this.form = this.fb.group({
      email:['',[Validators.required, Validators.email]],
      password:['',Validators.required]
    })
  }
}
