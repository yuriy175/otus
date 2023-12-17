import type { RootState } from "../store"
import { dialogsAdapter, dialogsSlice } from './dialogsSlice'

export const {selectById: selectDialog} = dialogsAdapter.getSelectors<RootState>(state => state.dialogs)

