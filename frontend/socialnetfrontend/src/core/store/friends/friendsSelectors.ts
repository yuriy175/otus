import type { RootState } from "../store"
import { friendsAdapter, friendsSlice } from './friendsSlice'

export const {selectAll: selectFriends} = friendsAdapter.getSelectors<RootState>(state => state.friends)

