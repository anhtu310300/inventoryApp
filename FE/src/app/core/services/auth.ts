import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LoginRequestModel } from '../models/classes/login-request.model';
import { Observable } from 'rxjs';
import { LoginResponse } from '../models/interfaces/login-response.interface';
import { environment } from '../../../environments/environment';
import { GlobalConstant } from '../constant/GlobalConstant';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);

  login(request: LoginRequestModel): Observable<LoginResponse> {
    const url = environment.apiUrl + GlobalConstant.API_METHODS.LOGIN;
    return this.http.post<LoginResponse>(url, request);
  }
}
