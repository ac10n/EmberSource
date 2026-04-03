import { Component, contentChild, input, output } from '@angular/core';
import { MatIcon } from "@angular/material/icon";
import { ModelPresenterComponent } from '../../../models/model-state';

@Component({
  selector: 'ember-model-presenter',
  imports: [MatIcon],
  templateUrl: './model-presenter.html',
  styleUrl: './model-presenter.scss',
})
export class ModelPresenter {
  child = contentChild(ModelPresenterComponent<any>);

  showEdit = input(false);
  onEdit = output();

  onDelete = output();
  showDelete = input(false);
  
  showRefresh = input(true);

  edit() {
  }

  delete() {
  }

  refresh() {
    this.child()?.manager?.load('refresh');
  }
}
