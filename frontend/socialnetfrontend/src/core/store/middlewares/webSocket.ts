import { delay } from "..";

export const createWebSocket = async (
    token: string,
    endpoint: string, 
    reconnect: (token: string, endpoint: string) => Promise<void>) =>{
    const webSocket = new WebSocket(endpoint + token);
    // webSocket.onmessage = function(event) {
    //     var leng;
    //         if (event.data.size === undefined) {
    //             leng = event.data.length
    //         } else {
    //             leng = event.data.size
    //         }
    //         console.log("onmessage. size: " + leng + ", content: " + event.data);
    //     }

        webSocket.onopen = function(evt) {
            console.log(`onopen ${endpoint}.`);
        };

        webSocket.onclose = async function(evt) {
            console.log(`onclose ${endpoint}.`);
            await delay(2000)
            await reconnect(token, endpoint)
        };

        webSocket.onerror = function(evt) {
            console.log(`Error ${endpoint}.`);
        };
        
        return webSocket
    }