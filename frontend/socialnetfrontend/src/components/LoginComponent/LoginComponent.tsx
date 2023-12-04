import React, {
  ChangeEventHandler,
  useEffect,
  useState,
} from 'react';
import { Navigate } from 'react-router-dom';
import axios from 'axios';

import TextField from '@mui/material/TextField';
import css from './LoginComponent.css';
import Button from '@mui/material/Button';
import {
  getUserFriends,
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

  useEffect(() => {
    if (user) {
      //dispatch(getUserFriends());
    }
  }, [user]);
  if (user) {
    return <Navigate to="/main" />;
  }

  const onIdChange: ChangeEventHandler<
    HTMLInputElement | HTMLTextAreaElement
  > = (e) => {
    setUserId(Number(e.target.value));
  };
  const onPasswordChange: ChangeEventHandler<
    HTMLInputElement | HTMLTextAreaElement
  > = (e) => {
    setPassword(e.target.value);
  };

  return (
    <div className={css.panel}>
      <TextField
        required
        id="outlined-required"
        label="Id пользователя"
        value={userId}
        onChange={onIdChange}
      />
      <TextField
        required
        id="outlined-required"
        label="Пароль"
        type="password"
        value={password}
        onChange={onPasswordChange}
      />
      <Button variant="contained" onClick={onLogin}>
        Логин
      </Button>
    </div>
  );
}
