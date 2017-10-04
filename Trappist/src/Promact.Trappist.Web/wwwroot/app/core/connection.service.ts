import { Injectable, EventEmitter, NgZone } from '@angular/core';

declare let signalR: any;
//This service is used as a middleware of the communication between clinet ans server hub in real time 
@Injectable()
export class ConnectionService {
    hubConnection: any;

    public recievedAttendee: EventEmitter<any>;
    public recievedAttendeeId: EventEmitter<any>;
   
   
    constructor(private _zone: NgZone) {
        this.recievedAttendee = new EventEmitter<any>();
        this.recievedAttendeeId = new EventEmitter<any>();
        
        //makes a connection with hub
        this.hubConnection = new signalR.HubConnection('/TrappistHub');
        this.registerProxy();
        this.startConnection();
    }
    //This method defines that what action should be taken when getReport and getRequest methods are invoked from the TrappistHub
    registerProxy() {
        this.hubConnection.on('getReport', (testAttendee) => { this._zone.run(() => this.recievedAttendee.emit(testAttendee));});
        this.hubConnection.on('getRequest', (id) => { this._zone.run(() => this.recievedAttendeeId.emit(id));});
    }
    //starts the connection between hub and client
    startConnection() {
        this.hubConnection.start().then(() => {
            console.log(new Date());
        });
    }
    //This method sends the testAttendee object to the hub method SendReport
    sendReport(testAttendee) {
        this.hubConnection.invoke('sendReport', testAttendee);
    }
    //Sends the id of candidate to the hub method sendRequest
    sendRequest(attendeeId: number) {
        this.hubConnection.invoke('sendRequest', attendeeId);
    }

    getReport(testAttendee: any) {
        return testAttendee;
    }

    getRequest(id: number) {
        return id;
    }
}