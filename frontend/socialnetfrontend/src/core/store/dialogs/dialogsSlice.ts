import type {PayloadAction} from '@reduxjs/toolkit'
import {createEntityAdapter, createSlice} from '@reduxjs/toolkit'
import { Dialog, CurrentUser, Post, User, DialogMessage, CurrentDialog } from '../../../core/types'

const initialState : CurrentDialog = {}
export const dialogsSlice = createSlice({
    name: 'dialogs',
    initialState,
    reducers:{
      setDialog(state, {payload}: PayloadAction<Dialog>){
        state.dialog =  payload
      },

      addDialogMessage(state, {payload}: PayloadAction<DialogMessage>){
        state.dialog.messages.push(payload)
      },
    }
})

export default dialogsSlice.reducer