// import { Middleware } from 'redux'
// import { io, Socket } from 'socket.io-client';
// import { chatActions } from './chatSlice';
// import ChatEvent from './chatEvent';
// import ChatMessage from "./chatMessage";

import { Middleware } from "@reduxjs/toolkit";
import { delay } from "..";

 
const websocketMiddleware: Middleware = store => {
  //let socket: Socket;
  const endpoint = 'ws://localhost:5230/post/feed?token=' // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNjQ1ODAxIn0.YtBssxfyHC3yw5QxPmKqjaoXFqXAtWn161hDB3I_1cQ'
  let webSocket: WebSocket | undefined = undefined//new WebSocket(endpoint);
 
  return next => action => {
    const reconnect = async (token: string) =>{
        console.log("connecting...");
        webSocket = new WebSocket(endpoint + token);
        webSocket.onmessage = function(event) {
            var leng;
            if (event.data.size === undefined) {
                leng = event.data.length
            } else {
                leng = event.data.size
            }
            console.log("onmessage. size: " + leng + ", content: " + event.data);
        }

        webSocket.onopen = function(evt) {
            console.log("onopen.");
        };

        webSocket.onclose = function(evt) {
            console.log("onclose.");
            reconnect(token)
        };

        webSocket.onerror = function(evt) {
            console.log("Error!");
        };
        
        await delay(2000)
    }
    if(action.type !== 'websocket/start'){
        next(action);
        return
    }
    const token = action.payload.token
    //reconnect(token)
    // webSocket = new WebSocket(endpoint + token);
    // webSocket.onmessage = function(event) {
    //     var leng;
    //     if (event.data.size === undefined) {
    //         leng = event.data.length
    //     } else {
    //         leng = event.data.size
    //     }
    //     console.log("onmessage. size: " + leng + ", content: " + event.data);
    // //     var element = document.getElementById("receivedMsg");
    // // element.setAttribute("value", event.data)
    // }

    // webSocket.onopen = function(evt) {
    //     console.log("onopen.");
    // };

    // webSocket.onclose = function(evt) {
    //     console.log("onclose.");
    // };

    // webSocket.onerror = function(evt) {
    //     console.log("Error!");
    // };

    // const isConnectionEstablished = socket && store.getState().chat.isConnected;
 
    // if (chatActions.startConnecting.match(action)) {
    //   socket = io(process.env.REACT_APP_API_URL, {
    //     withCredentials: true,
    //   });
 
    //   socket.on('connect', () => {
    //     store.dispatch(chatActions.connectionEstablished());
    //     socket.emit(ChatEvent.RequestAllMessages);
    //   })
 
    //   socket.on(ChatEvent.SendAllMessages, (messages: ChatMessage[]) => {
    //     store.dispatch(chatActions.receiveAllMessages({ messages }));
    //   })
 
    //   socket.on(ChatEvent.ReceiveMessage, (message: ChatMessage) => {
    //     store.dispatch(chatActions.receiveMessage({ message }));
    //   })
    // }
 
    // if (chatActions.submitMessage.match(action) && isConnectionEstablished) {
    //   socket.emit(ChatEvent.SendMessage, action.payload.content);
    // }
 
    next(action);
  }
}
 
export default websocketMiddleware;