import type { RootState } from "../store"
import { dialogsSlice } from './dialogsSlice'

export const selectDialogMessages = (state: RootState) => state.dialogs.dialog