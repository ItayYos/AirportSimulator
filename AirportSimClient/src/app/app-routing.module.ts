import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FlightsListComponent } from 'src/app/Flights/Displays/flights-list/flights-list.component';
import { FlightBoardComponent } from 'src/app/Flights/Displays/flight-board/flight-board.component';
import { FlightComponent } from 'src/app/Flights/flight/flight.component';

const routes: Routes = [
  {path: '' , component: FlightsListComponent},
  {path: 'flightBoard', component: FlightBoardComponent},
  {path: 'list', component: FlightComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
