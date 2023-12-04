import { getFriends, loginUser } from "../../../api";
import { AppThunk } from "../store";
import {friendsSlice} from "./friendsSlice";

const {setFriends} = friendsSlice.actions
export const getUserFriends = ():AppThunk => 
async(dispatch, getState) => {
    const friendIds = await getFriends()
    dispatch(setFriends())
}
