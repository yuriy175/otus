import type { RootState } from "../store"
import { postsAdapter, postsSlice } from './postsSlice'

export const {selectAll: selectPosts} = postsAdapter.getSelectors<RootState>(state => state.posts)

