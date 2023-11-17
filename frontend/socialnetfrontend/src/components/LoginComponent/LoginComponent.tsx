import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import axios from 'axios';

import TextField from '@mui/material/TextField';
import css from './LoginComponent.css';
import Button from '@mui/material/Button';

export function LoginComponent() {
  const [loggedIn, setLoggedIn] = useState(false);
  const onLogin = () => {
    setLoggedIn(true);
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
