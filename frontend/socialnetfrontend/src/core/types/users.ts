export type User = 
{
    id: number
    name: string
    surname: string
    age?: number
    sex?: string
    city?: string
    info?: string
}

export type CurrentUser = {
    user?: User
    foundUsers?: User[]
}