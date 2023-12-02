import { drawerWidth } from './types';
import React from 'react';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import AccountCircle from '@mui/icons-material/AccountCircle';
import css from './MainPage.css';

import { logoffCurrentUser, useAppDispatch } from '../../core/store';
import { User } from '../../core/types';

export function ToolbarComponent(props: { user: User }) {
  const { user } = props;
  const dispatch = useAppDispatch();

  const handleExit = () => {
    dispatch(logoffCurrentUser());
  };

  return (
    <AppBar
      position="fixed"
      sx={{
        width: { sm: `calc(100% - ${drawerWidth}px)` },
        ml: { sm: `${drawerWidth}px` },
      }}
    >
      <Toolbar>
        <IconButton
          edge="start"
          color="inherit"
          aria-label="menu"
          sx={{ mr: 2 }}
        ></IconButton>
        <Typography
          variant="h6"
          color="inherit"
          component="div"
          className={css.toolbarTitle}
        >
          {user?.surname} {user?.name} ({user?.id})
        </Typography>
        <IconButton size="large" onClick={handleExit} color="inherit">
          <AccountCircle />
        </IconButton>
      </Toolbar>
    </AppBar>
  );
}
