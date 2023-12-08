import type {PayloadAction} from '@reduxjs/toolkit'
import {createEntityAdapter, createSlice} from '@reduxjs/toolkit'
import { CurrentUser, User } from '../../../core/types'

export const friendsAdapter = createEntityAdapter<User>({
  selectId: (user) => user.id,
})

export const friendsSlice = createSlice({
    name: 'friends',
    initialState: friendsAdapter.getInitialState(),
    reducers:{
        setFriends(state, {payload}: PayloadAction<User[]>){
          friendsAdapter.setAll(state, payload)
        },

        addFriends(state, {payload}: PayloadAction<User>){
          friendsAdapter.addOne(state, payload)
        },

        deleteFriends(state, {payload}: PayloadAction<User['id']>){
          friendsAdapter.removeOne(state, payload)
        },
    }
})

export default friendsSlice.reducer