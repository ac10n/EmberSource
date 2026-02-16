import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ProfileRequest, ProfileResponse, UpdateProfileRequest, UpdateResult } from "../models/contract-models";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class ProfileService {
    constructor(
        private readonly http: HttpClient
    ) {
    }

    getProfile(request?: ProfileRequest) {
        return this.http.post<ProfileResponse>(`${environment.apiBaseUrl}/v01/profile/getProfile`, request ?? {});
    }

    updateProfile(request: UpdateProfileRequest): Observable<UpdateResult<ProfileResponse>> {
        return this.http.post<UpdateResult<ProfileResponse>>(`${environment.apiBaseUrl}/v01/profile/updateMyProfile`, request);
    }
}
