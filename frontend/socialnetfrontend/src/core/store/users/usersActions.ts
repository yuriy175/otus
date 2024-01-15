import { getFriends, getUsers, loginUser } from "../../../api";
import { createCsDialogStart, createGoDialogStart } from "../middlewares";
import { AppThunk } from "../store";
import {usersSlice} from "./usersSlice";

const {setCurrentUser, setFoundUser} = usersSlice.actions
export const loginCurrentUser = (id: number, password: string):AppThunk => 
async(dispatch, getState) => {
    const user = await loginUser(id, password)
    dispatch(setCurrentUser(user.user))
    dispatch(createCsDialogStart(user.token))
    //dispatch(createGoDialogStart(user.token))
}

export const logoffCurrentUser = ():AppThunk => 
async(dispatch, getState) => {
    dispatch(setCurrentUser(undefined))
}

export const searchUsers = (name: string, surname: string):AppThunk => 
async(dispatch, getState) => {
    const users = await getUsers(name, surname)
    dispatch(setFoundUser(users))
}
