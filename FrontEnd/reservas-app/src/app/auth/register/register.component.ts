import { Component, OnInit } from '@angular/core';
import { EmailValidator, Form, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth.services';
import { errorContext } from 'rxjs/internal/util/errorContext';

import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ]
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    //Formulario reactivo
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  onSubmit(): void {
    this.errorMessage = null;
    this.successMessage = null;

    if (this.registerForm.invalid) {
      this.errorMessage = 'Ha habido un error, vuelve a intentarlo.';
      return; 
    }

    this.authService.register(this.registerForm.value).subscribe({
      next: (Response) => {
        this.successMessage = '¡Usuario creado correctamente, ya puedes iniciar sesion';
        this.registerForm.reset();
      },

      error: (err) => {
        console.error('Error en el registro:', err);
        this.errorMessage = err.error || 'Ocurrió un error al registrar al usuario';
      }
    });
  }
}
