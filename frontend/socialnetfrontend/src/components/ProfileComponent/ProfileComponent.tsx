import React, { useEffect, useState } from 'react';
import css from './ProfileComponent.css';

import {
  useAppDispatch,
  useRootSelector,
  selectCurrentUser,
} from '../../core/store';
import TextField from '@mui/material/TextField';

export function ProfileComponent() {
  const dispatch = useAppDispatch();
  const user = useRootSelector(selectCurrentUser);
  console.log('user', user);
  return (
    <div className={css.panel}>
      <TextField
        id="standard-basic"
        label="Имя"
        variant="standard"
        value={user?.name}
      />
      <TextField
        id="standard-basic"
        label="Фамилия"
        variant="standard"
        value={user?.surname}
      />
      <TextField
        id="standard-basic"
        label="Возраст"
        variant="standard"
        value={user?.age}
      />
      <TextField
        id="standard-basic"
        label="Город"
        variant="standard"
        value={user?.city}
      />
      <TextField
        id="standard-basic"
        label="Инфо"
        variant="standard"
        multiline
        rows={4}
        value={user?.info ?? ''}
      />
    </div>
  );
}
