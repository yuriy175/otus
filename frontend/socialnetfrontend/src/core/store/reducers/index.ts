import {combineReducers} from '@reduxjs/toolkit'

import {usersReducer} from '../users'
import {friendsReducer} from '../friends'
import {postsReducer} from '../posts'

const rootReducer = combineReducers({
    users: usersReducer,
    friends: friendsReducer,
    posts: postsReducer,
})

export default rootReducer