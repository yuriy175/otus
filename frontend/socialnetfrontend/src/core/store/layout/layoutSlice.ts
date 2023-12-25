import type {PayloadAction} from '@reduxjs/toolkit'
import {createSlice} from '@reduxjs/toolkit'
import { STATUS_CODES } from 'http'
import { CurrentLayout, PageType } from '../../../core/types'

const initialState: CurrentLayout = {page: 'Profile'}

export const layoutSlice = createSlice({
    name: 'layout',
    initialState,
    reducers:{
        setLayoutPage(state, {payload}: PayloadAction<PageType>){
          state.page =  payload
        },
    }
})

export default layoutSlice.reducer