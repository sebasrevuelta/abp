import { HttpErrorResponse } from '@angular/common/http';
import { Injector, Type } from '@angular/core';
import { Validation } from '@ngx-validate/core';
import { Observable } from 'rxjs';

export interface RootParams {
  httpErrorConfig: HttpErrorConfig;
  validation?: Partial<Validation.Config>;
}

export type ErrorScreenErrorCodes = 401 | 403 | 404 | 500;

export interface HttpErrorConfig {
  skipHandledErrorCodes?: ErrorScreenErrorCodes[] | number[];
  errorScreen?: {
    component: Type<any>;
    forWhichErrors?: ErrorScreenErrorCodes[];
    hideCloseIcon?: boolean;
  };
}

export type HttpErrorHandler = (
  injector: Injector,
  httpError: HttpErrorResponse,
) => Observable<any>;

export type LocaleDirection = 'ltr' | 'rtl';
