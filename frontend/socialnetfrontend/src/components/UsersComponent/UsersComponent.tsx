import React, { useEffect, useState } from 'react';
import css from './UsersComponent.css';

import {
  useAppDispatch,
  useRootSelector,
  selectCurrentUser,
} from '../../core/store';
import TextField from '@mui/material/TextField';

export function UsersComponent() {
  const dispatch = useAppDispatch();
  const user = useRootSelector(selectCurrentUser);
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
        value={user?.info}
      />
    </div>
  );
}
