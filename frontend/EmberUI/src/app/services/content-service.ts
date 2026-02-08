import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { KnowledgeRequestModel, KnowledgeResponseModel } from '../models/contract-models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ContentService {
  constructor(private http: HttpClient) {}

  getKnowledge(request: KnowledgeRequestModel): Observable<KnowledgeResponseModel> {
    return this.http.post<KnowledgeResponseModel>(`${environment.apiBaseUrl}/knowledge/GetKnowledgeItems`, request);
  }
}
