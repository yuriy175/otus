import { getFriends, loginUser } from "../../../api";
import { AppThunk } from "../store";
import {usersSlice} from "./usersSlice";

const {setCurrentUser} = usersSlice.actions
export const loginCurrentUser = (id: number, password: string):AppThunk => 
async(dispatch, getState) => {
    const user = await loginUser(id, password)
    //const friendIds = await getFriends()
    dispatch(setCurrentUser(user))
}

export const logoffCurrentUser = ():AppThunk => 
async(dispatch, getState) => {
    dispatch(setCurrentUser(undefined))
}