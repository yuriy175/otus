import type {PayloadAction} from '@reduxjs/toolkit'
import {createEntityAdapter, createSlice} from '@reduxjs/toolkit'
import { Dialog, CurrentUser, Post, User } from '../../../core/types'

export const dialogsAdapter = createEntityAdapter<Dialog>({
  selectId: (dialog) => dialog.id,
})

export const dialogsSlice = createSlice({
    name: 'dialogs',
    initialState: dialogsAdapter.getInitialState(),
    reducers:{
        
    }
})

export default dialogsSlice.reducer