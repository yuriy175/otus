
export const webSocketStartActionType = "websocket/start"
export const webSocketWithUserActionType = "websocket/withUser"
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

export const createGoDialogStart = (token: string): WebSocketStart => ({
    type: webSocketStartActionType,
    payload:{
        token,
        //golang
        //endpoint: 'ws://localhost:55230/dialogs?token='
        //endpoint: 'ws://localhost:8005/dialogs?token='
        //endpoint: 'ws://localhost:3004/wsapp/go/dialogs?token='
        endpoint: 'ws://localhost:8005/dialogs?token='
        //endpoint: 'ws://localhost:3104/wsapp/go/dialogs?token='
    }
})

export const createCsDialogStart = (token: string): WebSocketStart => ({
    type: webSocketStartActionType,
    payload:{
        token,
        //cs
        endpoint: 'ws://localhost:5230/dialogs?token='
        //endpoint: 'ws://localhost:8006/dialogs?token='
        //endpoint: 'ws://localhost:3104/wsapp/cs/dialogs?token='
        //endpoint: 'ws://front:3104/wsapp/cs/dialogs?token='
        //endpoint: 'ws://localhost:8006/dialogs?token='
        //endpoint: 'ws://localhost:3104/wsapp/cs/dialogs?token='
    }
})

export const startDialogWithUser = (buddyId: number) => ({
    type: webSocketWithUserActionType,
    payload:{
        buddyId
    }
})