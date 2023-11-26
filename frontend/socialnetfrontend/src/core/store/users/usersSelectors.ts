import type { RootState } from "../store"
import { usersSlice } from './usersSlice'

export const selectCurrentUser = (state: RootState) => 
{
    return state.users?.user
}

