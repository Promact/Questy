import { Injectable, EventEmitter, NgZone } from '@angular/core';

import { HubConnection } from '@aspnet/signalr-client';
//This service is used as a middleware of the communication between clinet ans server hub in real time 
@Injectable()
export class ConnectionService {
    hubConnection: any;
    isConnected: boolean;

    public recievedAttendee: EventEmitter<any>;
    public recievedAttendeeId: EventEmitter<any>;
    public recievedEstimatedEndTime: EventEmitter<any>;    
   
    constructor(private _zone: NgZone) {
        this.recievedAttendee = new EventEmitter<any>();
        this.recievedAttendeeId = new EventEmitter<any>();
        this.recievedEstimatedEndTime = new EventEmitter<any>();
        //makes a connection with hub
        this.hubConnection = new HubConnection('/TrappistHub');
        this.registerProxy();
        this.isConnected = false;
    }
    //This method defines that what action should be taken when getReport and getRequest methods are invoked from the TrappistHub
    registerProxy() {
        this.hubConnection.on('getReport', (testAttendee) => { this._zone.run(() => this.recievedAttendee.emit(testAttendee));});
        this.hubConnection.on('getAttendeeIdWhoRequestedForResumeTest', (id) => { this._zone.run(() => this.recievedAttendeeId.emit(id)); });
        this.hubConnection.on('setEstimatedEndTime', (remainingTime) => { this._zone.run(() => this.recievedEstimatedEndTime.emit(remainingTime)); });
    }
    //starts the connection between hub and client
    startConnection(_callback?: any) {
        if (!this.isConnected) {
            this.hubConnection.start().then(() => {
                if (_callback)
                    _callback();

                this.isConnected = true;
            });
        }
    }

    isHubConnected() {
        return this.isConnected;
    }

    //This method sends the testAttendee object to the hub method SendReport
    sendReport(testAttendee) {
        this.hubConnection.invoke('sendReport', testAttendee);
    }
    //Sends the id of candidate to the hub method sendRequest
    sendCandidateIdWhoRequestedForResumeTest(attendeeId: number) {
        this.hubConnection.invoke('sendCandidateIdWhoRequestedForResumeTest', attendeeId);
    }

    getReport(testAttendee: any) {
        return testAttendee;
    }

    getAttendeeIdWhoRequestedForResumeTest(attendeeId: number) {
        return attendeeId;
    }

    registerAttendee(id: number) {
        this.hubConnection.invoke('registerAttendee', id);
    }

    addTestLogs(id: number) {
        this.hubConnection.invoke('addTestLogs', id);
    }

    updateExpectedEndTime(seconds: number, testId: number) {
        this.hubConnection.invoke('GetExpectedEndTime', seconds, testId);
    }

    setEstimatedEndTime(time: Date) {
        return time;
    }

    joinAdminGroup() {
        this.hubConnection.invoke('JoinAdminGroup');
    }
}