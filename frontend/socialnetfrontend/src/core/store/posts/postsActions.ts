import { Post } from "../../../core/types";
import { createPost, feedPosts} from "../../../api";
import { AppThunk } from "../store";
import {postsSlice} from "./postsSlice";

const {setPosts} = postsSlice.actions
export const feedFriendPosts = ():AppThunk => 
async(dispatch, getState) => {
    const apiPosts = await feedPosts(0, 100)
    const posts: Post[] = apiPosts.posts?.map(p => ({
        id:  p.id,
        author:  apiPosts.authors?.find(a => a.id === p.authorId),
        message:  p.message,
        //time:  p.created,       
    }))
    dispatch(setPosts(posts))
}

export const addUserPost = (text: string):AppThunk => 
async(dispatch, getState) => {
    await createPost(text)
}
