export const webSocketStartActionType = "websocket/start"
export type WebSocketStart = {
    payload: {
        token: string,
        endpoint: string,
    },
    type : typeof webSocketStartActionType
}

export const createFeedPostStart = (token: string): WebSocketStart => ({
    type: webSocketStartActionType,
    payload:{
        token,
        endpoint: 'ws://localhost:55230/post/feed?token='
    }
})

export const createDialogStart = (token: string): WebSocketStart => ({
    type: webSocketStartActionType,
    payload:{
        token,
        endpoint: 'ws://localhost:55230/dialogs?token='
    }
})
