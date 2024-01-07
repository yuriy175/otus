import type { RootState } from "../store"
import { buddiesAdapter, buddiesSlice } from './buddiesSlice'

export const {selectAll: selectBuddies} = buddiesAdapter.getSelectors<RootState>(state => state.buddies)

