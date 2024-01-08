// import { Middleware } from 'redux'
// import { io, Socket } from 'socket.io-client';
// import { chatActions } from './chatSlice';
// import ChatEvent from './chatEvent';
// import ChatMessage from "./chatMessage";

import { Middleware } from "@reduxjs/toolkit";
import { delay } from "..";
import { webSocketStartActionType } from "./webSockerActions";
import { createWebSocket } from "./webSocket";
import {dialogsSlice} from "../dialogs/dialogsSlice";

const { addDialogMessage} = dialogsSlice.actions
 
const websocketMiddleware: Middleware = store => {
  //let socket: Socket;
  //const endpoint = 'ws://localhost:5230/post/feed?token=' // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNjQ1ODAxIn0.YtBssxfyHC3yw5QxPmKqjaoXFqXAtWn161hDB3I_1cQ'
  //let webSocket: WebSocket | undefined = undefined//new WebSocket(endpoint);
  const webSockets = new Map<string, WebSocket | undefined>()
 
  return next => action => {
    const reconnect = async (token: string, endpoint: string) =>{
        console.log(`connecting...${endpoint}`);
        const webSocket = await createWebSocket(token, endpoint, reconnect)
        webSocket.onmessage = function(event) {
          const message = JSON.parse(event.data)
          console.log(`onmessage ${endpoint} - ${message}`);
          store. dispatch(addDialogMessage({
            own:true, 
            dialog: {
                ...message,
                message: message.message ?? '<Пустое сообщение>',
                datetime: new Date(message.created),  
            }
          }))
        }

        webSockets.set(endpoint, webSocket)
    }
    if(action.type !== webSocketStartActionType){
        next(action);
        return
    }
    const {token, endpoint} = action.payload
    reconnect(token, endpoint)
    // const reconnect = async (token: string) =>{
    //     console.log("connecting...");
    //     webSocket = new WebSocket(endpoint + token);
    //     webSocket.onmessage = function(event) {
    //         var leng;
    //         if (event.data.size === undefined) {
    //             leng = event.data.length
    //         } else {
    //             leng = event.data.size
    //         }
    //         console.log("onmessage. size: " + leng + ", content: " + event.data);
    //     }

    //     webSocket.onopen = function(evt) {
    //         console.log("onopen.");
    //     };

    //     webSocket.onclose = function(evt) {
    //         console.log("onclose.");
    //         reconnect(token)
    //     };

    //     webSocket.onerror = function(evt) {
    //         console.log("Error!");
    //     };
        
    //     await delay(2000)
    // }
    // if(action.type !== 'websocket/start'){
    //     next(action);
    //     return
    // }
    // const token = action.payload.token
    //reconnect(token)

    next(action);
  }
}
 
export default websocketMiddleware;