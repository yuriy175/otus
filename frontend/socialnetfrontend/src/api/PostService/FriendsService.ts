import {axiosInstance} from '../common'

import {FriendsClient} from '../Client'

const getFriendsClient = (): FriendsClient => new FriendsClient('', axiosInstance)

export const getFriends = async () => {
    const client = getFriendsClient()
    return client.friends()
}

export const addFriends = async (friendId: number) => {
    const client = getFriendsClient()
    return client.set(friendId)
}

export const deleteFriends = async (friendId: number) => {
    const client = getFriendsClient()
    return client.delete(friendId)
}