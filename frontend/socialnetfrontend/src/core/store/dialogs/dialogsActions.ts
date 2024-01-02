import { Dialog, DialogMessage } from "../../../core/types";
import { getDialog, createDialogMessage} from "../../../api";
import { AppThunk } from "../store";
import {dialogsSlice} from "./dialogsSlice";

const {setDialog, addDialogMessage} = dialogsSlice.actions

export const getUserDialog = (userId: number):AppThunk => 
async(dispatch, getState) => {
    const dialog = await getDialog(userId)
    dispatch(setDialog({
        buddy: dialog.user,
        messages: dialog.messages.map(m => ({
            ...m,
            message: m.message ?? '<Пустое сообщение>',
            datetime: new Date(m.created),   
        }))
    }))
}

export const addUserMessage = (userId: number, text: string):AppThunk => 
async(dispatch, getState) => {
    const message = await createDialogMessage(userId, text)
    dispatch(addDialogMessage(
        {
            own:true, 
            dialog: {
                ...message,
                message: message.message ?? '<Пустое сообщение>',
                datetime: new Date(message.created),  
            },
    }))
}
