import {addBearerToken, axiosInstance} from '../common'

import {AuthClient, UserClient} from './Client'

enum UserClients {
    AuthClient, UserClient
}

//function getClient(client: UserClients.AuthClient): AuthClient
//function getClient(client: UserClients.UserClient): UserClient
// function getClient(client: UserClients): UserClient | AuthClient{
//     switch(client){
//         case UserClients.AuthClient:
//             return new AuthClient('', axiosInstance)
//         case UserClients.UserClient:
//             return new UserClient('', axiosInstance)
//     }
// }

const getAuthClient = (client: UserClients): AuthClient => new AuthClient('auth', axiosInstance)
const getUserClient = (client: UserClients): UserClient => new UserClient('user', axiosInstance)

export const loginUser = async (
    id: number,
    password: string ) => {
    // const client2 = getUserClient(UserClients.UserClient)
    // const y = await client2.get(id)

    const client = getAuthClient(UserClients.AuthClient)
    const {token} = await client.login({
        id, password
    })
    
    addBearerToken(token)

    return token
}