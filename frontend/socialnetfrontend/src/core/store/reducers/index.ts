import {combineReducers} from '@reduxjs/toolkit'

import {usersReducer} from '../users'
//import {friendsReducer} from '../friends'

const rootReducer = combineReducers({
    users: usersReducer,
    //friends: friendsReducer,
})

export default rootReducer