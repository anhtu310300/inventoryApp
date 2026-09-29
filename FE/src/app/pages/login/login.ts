import { Component, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth';
import { Router } from '@angular/router';
import { LoginRequestModel } from '../../core/models/classes/login-request.model';
import { LoginResponse } from '../../core/models/interfaces/login-response.interface';
import { GlobalConstant } from '../../core/constant/GlobalConstant';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  isSubmitting: boolean = false;
  loginError: string = '';

  readonly loginForm = this.formBuilder.nonNullable.group({
    email: [
      '',
      [
        Validators.required,
        Validators.email,
      ],
    ],

    password: [
      '',
      [
        Validators.required,
      ],
    ],

    rememberMe: [false],
  });

  login(): void {
    this.loginError = '';
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }
    const formValue = this.loginForm.getRawValue();

    const loginRequest = new LoginRequestModel();
    loginRequest.email = formValue.email;
    loginRequest.password = formValue.password;

    this.isSubmitting = true;

    this.authService.login(loginRequest).subscribe({
      next: (response: LoginResponse) => {
        this.isSubmitting = false;

        localStorage.removeItem(GlobalConstant.TOKEN_KEY);
        sessionStorage.removeItem(GlobalConstant.TOKEN_KEY);

        const storage: Storage = formValue.rememberMe
          ? localStorage
          : sessionStorage;

        storage.setItem(
          GlobalConstant.TOKEN_KEY,
          response.accessToken,
        );

        this.router.navigateByUrl('/app');
      },

      error: (error: HttpErrorResponse) => {
        this.isSubmitting = false;

        const apiError = error.error as {
          message?: string;
        };

        this.loginError =
          apiError?.message ??
          'Wrong email or password!';
      },
    });
  }
}
