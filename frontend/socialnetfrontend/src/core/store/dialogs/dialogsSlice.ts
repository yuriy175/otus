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

      addDialogMessage(state, {payload}: PayloadAction<{own:boolean, dialog:DialogMessage}>){
        const {own, dialog} = payload
        if(state.dialog && (own || dialog.authorId === state.dialog.buddy.id)){
          state.dialog.messages.unshift(dialog)
        }
      },
    }
})

export default dialogsSlice.reducer