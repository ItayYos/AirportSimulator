import { Component, OnInit } from '@angular/core';
import { Flight } from 'src/Models/Flight';
import { FlightService } from 'src/Services/flight.service';

@Component({
  selector: 'app-flight-board',
  templateUrl: './flight-board.component.html',
  styleUrls: ['./flight-board.component.scss']
})
export class FlightBoardComponent implements OnInit {

  public flights : Flight[];
  constructor(private flightService : FlightService) {
    this.flights = this.flightService.Read();
   }

  ngOnInit(): void {
  }

  Validate(flight : Flight) : boolean{
    let ans : boolean = true;
    if(flight.legType == "Hanger" && flight.process.includes("LandingProcess"))
      ans = false;
    if(flight.legType == "LeftAirport" && flight.process.includes("DepartureProcess"))
      ans = false;
    return ans;
  }
}
