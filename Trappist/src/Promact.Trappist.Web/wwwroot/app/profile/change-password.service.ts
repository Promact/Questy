import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
import { ChangePasswordModel } from "../profile/password.model";
@Injectable()

export class PasswordService {
  private passwordApiUrl = "api/changePassword";

  constructor(public httpService: HttpService) {
  }

  /**
  * update the  password of the user
  */
  updateUserPassword(userPassword: ChangePasswordModel) {
    return this.httpService.put(this.passwordApiUrl, userPassword);
  }

}
