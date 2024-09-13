import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../../_services/account.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  constructor(
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}
  ngOnInit(): void {}
  register() {
    this.accountService.register(this.model).subscribe({
      next: (res) => {
        this.toastr.success('Successfully Logged In!');
        this.cancel();
      },
      error: (error) => {
        console.log(error);
        this.toastr.error(error.error);
      },
    });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }
}
