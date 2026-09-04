import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptors } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { SharedModule } from '../_modules/shared.module';
import { errorInterceptor } from '../_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { authGuard } from '../_guards/auth.guard';

@NgModule({
  // Declare all components that belong to this module
  declarations: [AppComponent, NavComponent, HomeComponent, RegisterComponent, MessagesComponent, ListsComponent, MemberListComponent, MemberDetailComponent, NotFoundComponent, ServerErrorComponent],
  // Import required Angular and custom modules
  imports: [
    BrowserModule,             // Required for running the app in a browser
    AppRoutingModule,          // Handles application routing
    BrowserAnimationsModule,   // Enables animations (e.g. for Angular Material)
    FormsModule,               // Supports template-driven forms
    SharedModule               // Custom shared module (e.g. shared components, pipes, etc.)
  ],
  // Register global services and HTTP interceptors using Angular's DI system
  providers: [provideHttpClient(withInterceptors([errorInterceptor]))],
  // Define the root component to bootstrap the application
  bootstrap: [AppComponent],
})
export class AppModule {}
