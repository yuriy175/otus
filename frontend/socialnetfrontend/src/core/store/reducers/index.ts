import {combineReducers} from '@reduxjs/toolkit'

import {usersReducer} from '../users'
import {friendsReducer} from '../friends'
import {postsReducer} from '../posts'
import {dialogsReducer} from '../dialogs'
import {layoutReducer} from '../layout'
import {buddiesReducer} from '../buddies'
import {countersReducer} from '../counters'

const rootReducer = combineReducers({
    users: usersReducer,
    friends: friendsReducer,
    posts: postsReducer,
    dialogs: dialogsReducer,
    layout: layoutReducer,
    buddies: buddiesReducer,
    counters: countersReducer,
})

export default rootReducer