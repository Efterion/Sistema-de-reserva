import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { CoreModule } from './core/core-module';
import { SharedModule } from './shared/shared-module';
import { AuthModule } from './auth/auth-module';
import { ReservationModule } from './Reservation/reservation.module';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    App
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    SharedModule,
    AuthModule,
    ReservationModule,
    HttpClientModule

  ],
  providers: [
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [App]
})
export class AppModule { }
