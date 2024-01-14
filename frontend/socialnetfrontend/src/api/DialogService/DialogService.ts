import {addBearerToken, axiosInstance} from '../common'

import {DialogsClient} from '../Client'


const getDialogsClient = (): DialogsClient => new DialogsClient('', axiosInstance) 

export const createDialogMessage = async (userId: number, text: string) => {
    const client = getDialogsClient()
    return client.send(userId, text)
}

export const getDialog = async (userId: number) => {
    const client = getDialogsClient()
    return client.list(userId)
}

export const getDialogBuddies = async () => {
    const client = getDialogsClient()
    return client.buddies()
}