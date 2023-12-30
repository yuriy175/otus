import { getDialogBuddies} from "../../../api";
import { AppThunk } from "../store";
import {buddiesSlice} from "./buddiesSlice";

const {setBuddies} = buddiesSlice.actions
export const getUserBuddies = ():AppThunk => 
async(dispatch, getState) => {
    const buddies = await getDialogBuddies()
    dispatch(setBuddies(buddies))
}