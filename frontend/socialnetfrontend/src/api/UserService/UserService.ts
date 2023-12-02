import {addBearerToken, axiosInstance} from '../common'

import {AuthClient, UserClient} from './Client'

enum UserClients {
    AuthClient, UserClient
}

const getAuthClient = (client: UserClients): AuthClient => new AuthClient('auth', axiosInstance)
const getUserClient = (client: UserClients): UserClient => new UserClient('user', axiosInstance)

export const loginUser = async (
    id: number,
    password: string ) => {
    const authClient = getAuthClient(UserClients.AuthClient)
    const tokenDto = await authClient.login({
        id, password
    })
    
    if(!tokenDto?.token){
        return undefined
    }
    const token = tokenDto.token
    addBearerToken(token)

    const userClient = getUserClient(UserClients.UserClient)
    const user = await userClient.get(id)

    return user
}