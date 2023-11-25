import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import axios from 'axios';

import TextField from '@mui/material/TextField';
import css from './LoginComponent.css';
import Button from '@mui/material/Button';
import { loginCurrentUser, useAppDispatch } from '../../core/store';

export function LoginComponent() {
  const dispatch = useAppDispatch();
  const [loggedIn, setLoggedIn] = useState(false);
  const onLogin = () => {
    //setLoggedIn(true);
    dispatch(loginCurrentUser(1645801, 'Абрамов'));
  };
  if (loggedIn) {
    return <Navigate to="/main" />;
  }

  return (
    <div className={css.panel}>
      <TextField
        required
        id="outlined-required"
        label="Id пользователя"
      />
      <TextField
        required
        id="outlined-required"
        label="Пароль"
        type="password"
      />
      <Button variant="contained" onClick={onLogin}>
        Логин
      </Button>
    </div>
  );
}
