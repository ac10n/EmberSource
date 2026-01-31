import { Component } from '@angular/core';
import { MatFormField, MatLabel } from '@angular/material/form-field';

@Component({
  selector: 'app-create-account',
  imports: [
    MatFormField,
    MatLabel,
  ],
  templateUrl: './create-account.html',
  styleUrl: './create-account.scss',
})
export class CreateAccountComponent {

}
