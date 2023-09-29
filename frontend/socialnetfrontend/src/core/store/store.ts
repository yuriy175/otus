import { configureStore } from '@reduxjs/toolkit'
import { TypedUseSelectorHook, useSelector } from 'react-redux'
import { useDispatch } from 'react-redux'

// import rootReducer from './reducers'

//export type RootState = ReturnType<typeof rootReducer>
export type AppDispatch = typeof store.dispatch
//export const useRootSelector: TypedUseSelectorHook<RootState> = useSelector
export const useAppDispatch = () => useDispatch<AppDispatch>()

const store = configureStore({
  reducer: undefined, //rootReducer,
})

export default store
