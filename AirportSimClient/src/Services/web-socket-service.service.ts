import { Injectable, EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class WebSocketServiceService {

  private socket : WebSocket;
  private listener : EventEmitter<any>;

  constructor() {
    this.listener = new EventEmitter();
    //this.socket = new WebSocket("ws://localhost:50677/ws");
    this.socket = new WebSocket("ws://localhost:50677/wsConnect");
    this.socket.onopen = event => {this.listener.emit({"type": "open", "data": event})};
    this.socket.onclose = event => {this.listener.emit({"type": "close", "data": event})};
    this.socket.onmessage = event => {this.listener.emit({"type": "message", "data": event})};
    }

  public send(data: string) {
    this.socket.send(data);
  }

  public close() {
    this.socket.close();
  }

  public getEventListener() {
    return this.listener;
  }
}
