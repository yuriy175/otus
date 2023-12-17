import {combineReducers} from '@reduxjs/toolkit'

import {usersReducer} from '../users'
import {friendsReducer} from '../friends'
import {postsReducer} from '../posts'
import {dialogsReducer} from '../dialogs'

const rootReducer = combineReducers({
    users: usersReducer,
    friends: friendsReducer,
    posts: postsReducer,
    dialogs: dialogsReducer,
})

export default rootReducer