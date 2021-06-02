import { Component, OnInit } from '@angular/core';
import { Flight } from 'src/Models/Flight';
import { FlightService } from 'src/Services/flight.service';

@Component({
  selector: 'app-flights-list',
  templateUrl: './flights-list.component.html',
  styleUrls: ['./flights-list.component.scss']
})

export class FlightsListComponent implements OnInit {
  flights: Flight[];

  constructor(private flightService : FlightService) {
    this.flights = this.flightService.Read();
   }

  ngOnInit(): void {
  }

}