import type {PayloadAction} from '@reduxjs/toolkit'
import {createSlice} from '@reduxjs/toolkit'
import { STATUS_CODES } from 'http'
import { CurrentUser, User } from '../../../core/types'

const initialState : CurrentUser = {}

export const usersSlice = createSlice({
    name: 'users',
    initialState,
    reducers:{
        setCurrentUser(state, {payload}: PayloadAction<User>){
          state.user =  payload
        },
        setFoundUser(state, {payload}: PayloadAction<User[]>){
          state.foundUsers =  payload
        },
    }
})

export default usersSlice.reducer