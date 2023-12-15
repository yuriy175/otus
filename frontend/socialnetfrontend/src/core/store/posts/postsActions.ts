import { getFriends, loginUser, addFriends as apiAddFriends, deleteFriends  as apiDeleteFriends} from "../../../api";
import { AppThunk } from "../store";
import {friendsSlice} from "./friendsSlice";

const {setFriends, addFriends, deleteFriends} = friendsSlice.actions
export const getUserFriends = ():AppThunk => 
async(dispatch, getState) => {
    const friends = await getFriends()
    dispatch(setFriends(friends))
}

export const addUserFriends = (friendId: number):AppThunk => 
async(dispatch, getState) => {
    const friend = await apiAddFriends(friendId)
    dispatch(addFriends(friend))
}

export const deleteUserFriends = (friendId: number):AppThunk => 
async(dispatch, getState) => {
    await apiDeleteFriends(friendId)
    dispatch(deleteFriends(friendId))
}