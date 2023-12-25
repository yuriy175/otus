import { Dialog, DialogMessage } from "../../../core/types";
import { getDialog, createDialogMessage} from "../../../api";
import { AppThunk } from "../store";
import {dialogsSlice} from "./dialogsSlice";

const {setDialog, addDialogMessage, closeDialog} = dialogsSlice.actions

export const getUserDialog = (userId: number):AppThunk => 
async(dispatch, getState) => {
    const friends = await getDialog(userId)
    dispatch(setDialog({}))
}

export const addUserMessage = (dialogId: number, userId: number, text: string):AppThunk => 
async(dispatch, getState) => {
    const message = await createDialogMessage(userId, text)
    if(!message.id){
        return
    }
    dispatch(addDialogMessage({id: dialogId, message}))
}

export const closeUseDialog = (dialogId: number):AppThunk => 
async(dispatch, getState) => {
    dispatch(closeDialog(dialogId))
}