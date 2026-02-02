import { Component } from '@angular/core';
import { ContentExplorer } from "../components/content-explorer/content-explorer";

@Component({
  selector: 'app-home',
  imports: [ContentExplorer],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class HomeComponent {

}
