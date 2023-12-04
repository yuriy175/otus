import type {PayloadAction} from '@reduxjs/toolkit'
import {createSlice} from '@reduxjs/toolkit'
import { STATUS_CODES } from 'http'
import { CurrentUser, User } from '../../../core/types'

const initialState : CurrentUser = {}

export const friendsSlice = createSlice({
    name: 'friends',
    initialState,
    reducers:{
        setFriends(state, {payload}: PayloadAction<User>){
          state. =  payload
        },
    }
})

export default friendsSlice.reducer