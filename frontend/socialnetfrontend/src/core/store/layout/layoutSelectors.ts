import type { RootState } from "../store"

export const selectCurrentPage = (state: RootState) => state.layout.page
