import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {
  useAppDispatch,
  useRootSelector,
  selectFriends,
} from '../../core/store';

export function FriendsComponent() {
  const dispatch = useAppDispatch();
  const user = useRootSelector(selectFriends);
  return <div className="app">FriendsComponent!!!!</div>;
}
