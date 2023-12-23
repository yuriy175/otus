import type { RootState } from "../store"
import { usersSlice } from './usersSlice'

export const selectCurrentUser = (state: RootState) => 
{
    return state.users?.user
}

export const selectFoundUsers = (state: RootState) => state.users?.foundUsers
