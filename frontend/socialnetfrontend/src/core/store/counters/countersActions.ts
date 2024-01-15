import { getUnreadMessageCount } from "../../../api";
import { AppThunk } from "../store";
import {counterSlice} from "./countersSlice";

const {setUnreadCount} = counterSlice.actions
export const setUnreadMessagesCount = ():AppThunk => 
async(dispatch, getState) => {
    const count = await getUnreadMessageCount()
    dispatch(setUnreadCount(count))
}
