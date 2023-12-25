import type {PayloadAction} from '@reduxjs/toolkit'
import {createEntityAdapter, createSlice} from '@reduxjs/toolkit'
import { Dialog, CurrentUser, Post, User, DialogMessage } from '../../../core/types'

export const dialogsAdapter = createEntityAdapter<Dialog>({
  selectId: (dialog) => dialog.id,
})

export const dialogsSlice = createSlice({
    name: 'dialogs',
    initialState: dialogsAdapter.getInitialState(),
    reducers:{
      setDialog(state, {payload}: PayloadAction<Dialog>){
        dialogsAdapter.upsertOne(state, payload)
      },

      addDialogMessage(state, {payload}: PayloadAction<{id: number, message: DialogMessage}>){
        state.entities[payload.id].messages.push(payload.message)
      },

      closeDialog(state, {payload}: PayloadAction<Dialog['id']>){
        dialogsAdapter.removeOne(state, payload)
      },
    }
})

export default dialogsSlice.reducer