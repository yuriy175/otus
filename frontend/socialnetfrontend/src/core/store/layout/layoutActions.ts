import { PageType } from "../../../core/types";
import { AppThunk } from "../store";
import {layoutSlice} from "./layoutSlice";

const {setLayoutPage} = layoutSlice.actions
export const setActivePage = (page: PageType):AppThunk => 
async(dispatch, getState) => {
    dispatch(setLayoutPage(page))
}
