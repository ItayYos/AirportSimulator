import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { FlightsListComponent } from './Flights/Displays/flights-list/flights-list.component';
import { FlightComponent } from './Flights/flight/flight.component';
import { WebSocketServiceService } from '../Services/web-socket-service.service'
import { DescriptionPipe } from './Pipes/Description.pipe';
import { ProcessPipe } from './Pipes/Process.pipe';
import { FlightService } from '../Services/flight.service';
import { FlightBoardComponent } from './Flights/Displays/flight-board/flight-board.component';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    FlightsListComponent,
    FlightComponent,
    DescriptionPipe,
    ProcessPipe,
    FlightBoardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [WebSocketServiceService, FlightService],
  bootstrap: [AppComponent]
})
export class AppModule { }
