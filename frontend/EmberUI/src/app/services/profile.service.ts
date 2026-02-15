import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ProfileRequest, ProfileResponse, UpdateProfileRequest } from "../models/contract-models";
import { environment } from "../../environments/environment";

@Injectable({ providedIn: 'root' })
export class ProfileService {
    constructor(
        private readonly http: HttpClient
    ) {
    }

    getProfile(request?: ProfileRequest) {
        return this.http.post<ProfileResponse>(`${environment.apiBaseUrl}/v01/profile/getProfile`, request ?? {});
    }

    updateProfile(request: UpdateProfileRequest) {
        return this.http.post(`${environment.apiBaseUrl}/v01/profile/updateMyProfile`, request);
    }
}
