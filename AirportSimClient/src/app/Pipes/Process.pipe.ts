import { Pipe, PipeTransform } from '@angular/core';

@Pipe ({name: 'ProcessPipe'})
export class ProcessPipe implements PipeTransform{
    
    transform(value: string, args?:any): string{
        let ans: string = '';

        if(value.includes("DepartureProcess"))
            ans = "DepartureProcess";
        if(value.includes("LandingProcess"))
            ans = "LandingProcess";

        return ans;
    }
}