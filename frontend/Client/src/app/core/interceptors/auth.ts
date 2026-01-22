import { inject } from "@angular/core";
import { TokenService } from "../services/tokenservice";
import { HttpInterceptorFn } from "@angular/common/http";

export const authInterceptor: HttpInterceptorFn = (req, next) => {
 const tokenService = inject(TokenService);
  const accessToken = tokenService.getToken();

  


 
    const clonedRequest = addTokenToRequest(req, accessToken!);

    console.log(accessToken);

    return next(clonedRequest);

  
};

function addTokenToRequest(request: any, accessToken: string) {
  return request.clone({
    setHeaders: {
      Authorization: `Bearer ${accessToken}`,
    },
    withCredentials: true,
  });
}

