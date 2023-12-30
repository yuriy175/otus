import type {PayloadAction} from '@reduxjs/toolkit'
import {createEntityAdapter, createSlice} from '@reduxjs/toolkit'
import { CurrentUser, User } from '../../../core/types'

export const buddiesAdapter = createEntityAdapter<User>({
  selectId: (user) => user.id,
})

export const buddiesSlice = createSlice({
    name: 'buddies',
    initialState: buddiesAdapter.getInitialState(),
    reducers:{
        setBuddies(state, {payload}: PayloadAction<User[]>){
          buddiesAdapter.setAll(state, payload)
        },
    }
})

export default buddiesSlice.reducer