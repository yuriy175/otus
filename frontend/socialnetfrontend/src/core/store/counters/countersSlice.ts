import type {PayloadAction} from '@reduxjs/toolkit'
import {createSlice} from '@reduxjs/toolkit'
import { CounterState } from '../../../core/types'

const initialState: CounterState = {unreadCount: 0}

export const counterSlice = createSlice({
    name: 'counter',
    initialState,
    reducers:{
        setUnreadCount(state, {payload}: PayloadAction<number>){
          state.unreadCount =  payload
        },
    }
})

export default counterSlice.reducer