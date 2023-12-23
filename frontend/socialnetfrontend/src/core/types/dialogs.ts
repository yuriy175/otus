import { User } from "."

export type Dialog = 
{
    id: number
    partner: User
    messages: DialogMessage[]
}

export type DialogMessage = 
{
    id: number
    authorId: number
    message: string
    datetime?: Date
}