import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { email } from '@angular/forms/signals';
import { LoginRequest } from '../interfaces/login-request';
import { AuthResponse } from '../interfaces/auth-response';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  apiUrl:string= environment.apiUrl;

  private tokenKey = 'token';
  constructor(private http:HttpClient) {}
  
  login(data:LoginRequest):Observable<AuthResponse>{
    return this.http.post<AuthResponse>(`${this.apiUrl}/Account/Login`,data).pipe(
      map((response)=>{
        if(response.isSuccessfull)
        {
          localStorage.setItem(this.tokenKey,response.token);
        }
        return response;
      })
    )
  }

  isLoggedIn = ():boolean =>{
    const token = this.getToken();
    if(!token) return false;
    return !this.isTokenExpired();
  }

  private isTokenExpired(){
    const token = this.getToken();
    if(!token)
    {
      return true;
    }
    else{
      const decode = jwtDecode(token);
      const isTokenExpire = Date.now() >= decode['exp']!* 1000;
       return isTokenExpire;
    }
  }

  logout =():void =>{
    localStorage.removeItem(this.tokenKey);
  }
  private getToken = ():string | null => localStorage.getItem(this.tokenKey) || '';
}
