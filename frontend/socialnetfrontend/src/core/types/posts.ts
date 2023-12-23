import { User } from "."

export type Post = 
{
    id: number
    author: User
    message: string
    datetime?: Date
}