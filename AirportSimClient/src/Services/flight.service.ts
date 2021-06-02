import { Injectable } from '@angular/core';
import { Flight } from 'src/Models/Flight';

@Injectable({
  providedIn: 'root'
})
export class FlightService {
  flights : Flight[];

  constructor() {
    this.flights = [];
  }

  Create(flight: Flight) : boolean{
    let index: number = this.FindIndex(flight);
    if(index == -1){
      this.flights.push(flight);
      return true;
    }
    else
      return false;
  }

  Read() : Flight[]{
    return this.flights;
  }

  Update(flight: Flight): boolean{
    let index: number = this.FindIndex(flight);
    if(index != -1)  {
      let flightToUpdate : Flight = this.flights[index];
      flightToUpdate.legId = flight.legId;
      flightToUpdate.legType = flight.legType;
      flightToUpdate.process = flight.process;
      flightToUpdate.name = flight.name;
      return true;
    }
    else
      return false;
  }

  Delete(flight: Flight) : boolean{
    let index: number = this.FindIndex(flight);
    if(index != -1){
      this.flights.splice(index, 1);
      return true;
    }
    else
      return false;
  }

  private FindIndex(flight: Flight) : number{
    let ans: number = -1;
    for(let i = 0; i < this.flights.length; i++){
      if(this.flights[i].name == flight.name){
        ans = i;
        i = this.flights.length;
      }
    }
    return ans;
  }
}
