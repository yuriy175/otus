import { User } from "."

export type Dialog = 
{
    buddy: User
    messages: DialogMessage[]
}

export type DialogMessage = 
{
    id: number
    authorId: number
    userId: number
    message: string
    datetime?: Date
}

export type CurrentDialog = {
    dialog?: Dialog
}