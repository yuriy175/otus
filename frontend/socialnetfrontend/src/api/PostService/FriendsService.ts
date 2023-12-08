import {axiosCsInstance, axiosGoInstance} from '../common'

import {FriendsClient} from '../Client'

const getFriendsClient = (): FriendsClient => new FriendsClient('friends', axiosCsInstance)

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