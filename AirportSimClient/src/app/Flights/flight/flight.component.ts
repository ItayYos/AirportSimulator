import { Component, OnInit } from '@angular/core';
import {WebSocketServiceService} from '../../../Services/web-socket-service.service';
import {Flight} from '../../../Models/Flight';
import { FlightService } from 'src/Services/flight.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-flight',
  templateUrl: './flight.component.html',
  styleUrls: ['./flight.component.scss']
})

export class FlightComponent implements OnInit {

  public flights : Flight[];
  public output : string;

  constructor(private socket : WebSocketServiceService, private flightService : FlightService, private router : Router) {
    this.flights = this.flightService.Read();
    this.output = "ctor";
   }

  ngOnInit(): void {
    this.output = "on init";
    this.socket.getEventListener().subscribe(event => {
      if(event.type == "message") {
          let flight : Flight = new Flight();
          var msg = JSON.parse(event.data.data);
          flight.name = msg.Airplane;
          flight.legId = msg.CurrentLegId;
          flight.legType = msg.CurrentLegType;
          flight.process = msg.Process;
          this.IncomingData(flight);
          //this.o2 += event.data.data + "\n";
          //var obj = JSON.parse(event.data.data);
          //this.o3 = "Airplane: " + obj.airplane.Name + ", Terminal: " + obj.leg.Id;
      }
      if(event.type == "close") {
          this.output = "connection closed";
      }
      if(event.type == "open") {
        this.output = "connection established.";
        //this.socket.send("ping");
      }
    });
  }

  ngOnDestroy() {
    this.socket.close();
  }

  private IncomingData(flight : Flight) : void{
      let res: boolean = this.flightService.Create(flight);
      if(!res)
        this.flightService.Update(flight);
  }
}