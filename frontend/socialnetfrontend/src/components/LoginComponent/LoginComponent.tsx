import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import axios from 'axios';

import TextField from '@mui/material/TextField';
import css from './LoginComponent.css';
import Button from '@mui/material/Button';
import {
  loginCurrentUser,
  selectCurrentUser,
  useAppDispatch,
  useRootSelector,
} from '../../core/store';

export function LoginComponent() {
  const dispatch = useAppDispatch();
  const user = useRootSelector(selectCurrentUser);
  const [userId, setUserId] = useState(1645801);
  const [password, setPassword] = useState('Абрамов');
  const onLogin = () => {
    dispatch(loginCurrentUser(userId, password));
  };
  if (user) {
    return <Navigate to="/main" />;
  }

  return (
    <div className={css.panel}>
      <TextField
        required
        id="outlined-required"
        label="Id пользователя"
        value={userId}
      />
      <TextField
        required
        id="outlined-required"
        label="Пароль"
        type="password"
        value={password}
      />
      <Button variant="contained" onClick={onLogin}>
        Логин
      </Button>
    </div>
  );
}
