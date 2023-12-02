import type { RootState } from "../store"
import { friendsSlice } from './friendsSlice'

export const selectFriends = (state: RootState) => 
{
    return state.users?.user
}

