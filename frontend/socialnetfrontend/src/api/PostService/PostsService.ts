import {axiosCsInstance, axiosGoInstance} from '../common'

import {PostsClient} from '../Client'

const getPostsClient = (): PostsClient => new PostsClient('posts', axiosGoInstance)

export const createPost = async (text: string) => {
    const client = getPostsClient()
    return client.create(text)
}

export const feedPosts = async (offset: number, limit: number) => {
    const client = getPostsClient()
    return client.feed(offset, limit)
}
