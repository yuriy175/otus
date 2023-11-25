import {axiosInstance} from '../common'

import {PostsClient, FriendsClient} from './Client'

enum PostsClients {
    PostsClient, FriendsClient
}

const getPostsClient = (): PostsClient => new PostsClient('posts', axiosInstance)
const getFriendsClient = (): FriendsClient => new FriendsClient('friends', axiosInstance)

export const getFriends = async () => {
    const client = getFriendsClient()
    return client.friends()
}