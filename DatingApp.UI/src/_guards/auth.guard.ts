import { inject } from '@angular/core';
import { CanActivateFn, CanActivateChildFn, RouterStateSnapshot } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';

export const authGuard: CanActivateFn | CanActivateChildFn = async (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  const toastr = inject(ToastrService);

  try {
    debugger
    const user = await firstValueFrom(accountService.currentUser$);
    console.log('User in guard:', user);

    if (user) return true;

    toastr.error('You shall not pass!');
    router.navigate(['/']);
    return false;
  } catch (err) {
    console.error('Auth Guard Error:', err);
    toastr.error('Authentication check failed');
    router.navigate(['/']);
    return false;
  }
};

