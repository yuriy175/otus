import {addBearerToken, axiosCsInstance, axiosGoInstance} from '../common'

import {UserClient} from '../Client'
import { User } from '../../core/types'

enum UserClients {
    AuthClient, UserClient
}

const getUserClient = (): UserClient => new UserClient('', axiosGoInstance)

export const loginUser = async (
    id: number,
    password: string ) => {

    const userClient = getUserClient()
    const userDto = await userClient.login({
        id, password
    })

    if(!userDto?.token){
        return undefined
    }
    const token = userDto.token
    addBearerToken(token)

    return userDto
}

export const getUsers = async (name: string, surname: string) =>{
    const userClient = getUserClient()
    return userClient.search(name, surname)
}

export const registerUser = async (user: User, password: string) =>{
    const userClient = getUserClient()
    return userClient.register({...user, password})
}