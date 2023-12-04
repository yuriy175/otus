import {addBearerToken, axiosCsInstance, axiosGoInstance} from '../common'

import {UserClient} from '../Client'

enum UserClients {
    AuthClient, UserClient
}

//const getAuthClient = (client: UserClients): AuthClient => new AuthClient('auth', axiosInstance)
const getUserClient = (): UserClient => new UserClient('users', axiosCsInstance)

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