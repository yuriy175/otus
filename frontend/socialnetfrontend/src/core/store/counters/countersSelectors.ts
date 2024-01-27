import type { RootState } from "../store"

export const selectUnreadCount = (state: RootState) => state.counters.unreadCount
