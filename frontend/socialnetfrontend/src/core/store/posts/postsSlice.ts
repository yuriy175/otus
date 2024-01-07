import type {PayloadAction} from '@reduxjs/toolkit'
import {createEntityAdapter, createSlice} from '@reduxjs/toolkit'
import { CurrentUser, Post, User } from '../../../core/types'

export const postsAdapter = createEntityAdapter<Post>({
  selectId: (post) => post.id,
})

export const postsSlice = createSlice({
    name: 'posts',
    initialState: postsAdapter.getInitialState(),
    reducers:{
        setPosts(state, {payload}: PayloadAction<Post[]>){
          postsAdapter.setAll(state, payload)
        },
    }
})

export default postsSlice.reducer