import { Pipe, PipeTransform } from '@angular/core';

@Pipe ({name: 'DescriptionPipe'})
export class DescriptionPipe implements PipeTransform{
    
    transform(process: string, legType: string, args?:any): string{
        let ans: string = "Waiting for Entrance.";
       
        if(process.includes("DepartureProcess")){
            if(legType == "Hanger")
                ans = "Parked at hanger.";
            if(legType == "Load")
                ans = "Loading passangers.";
            if(legType == "TransportToStrip")
                ans = "Transporting to strip.";
            if(legType == "Strip")
                ans = "Taking off.";
            if(legType == "LeftAirport")
                ans = "Left airport.";
        }
            
        if(process.includes("LandingProcess")){
            ans = "Approach landing.";
            if(legType == "WaitingForEntrance")
                ans = "Waiting for Entrance.";
            if(legType == "Strip")
                ans = "Landing.";
            if(legType == "TransportToLoad")
                ans = "Transporting to loading";
            if(legType == "Load")
                ans = "Unloading passangers.";
            if(legType == "Hanger")
                ans = "Parked at hanger.";
        }
        
        return ans;
    }
}