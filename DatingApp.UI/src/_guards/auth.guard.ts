import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);
  console.log('authGuard Entered');
  return accountService.currentUser$.pipe(
    map((user) => {
      if (user) return true;
      toastr.error('You shall not pass!');
      return false;
    })
  );
};
