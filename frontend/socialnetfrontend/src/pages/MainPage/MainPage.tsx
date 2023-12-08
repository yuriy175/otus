import React, { useEffect, useState } from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Drawer from '@mui/material/Drawer';
import List from '@mui/material/List';
import Divider from '@mui/material/Divider';
import css from './MainPage.css';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import Typography from '@mui/material/Typography';
import ListItemText from '@mui/material/ListItemText';
import InboxIcon from '@mui/icons-material/MoveToInbox';
import MailIcon from '@mui/icons-material/Mail';
import FormControlLabel from '@mui/material/FormControlLabel';
import FormGroup from '@mui/material/FormGroup';
import {
  useRootSelector,
  selectCurrentUser,
  logoffCurrentUser,
  useAppDispatch,
  getUserFriends,
} from '../../core/store';
import Switch from '@mui/material/Switch';
import { Navigate } from 'react-router-dom';
import AccountCircle from '@mui/icons-material/AccountCircle';
import { DrawerComponent } from './DrawerComponent';
import { ToolbarComponent } from './ToolbarComponent';
import { PageType, drawerWidth } from './types';
import { FriendsComponent, UsersComponent } from '../../components';

export function MainPage() {
  const user = useRootSelector(selectCurrentUser);
  const dispatch = useAppDispatch();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [pageType, setPageType] = useState<PageType>('Profile');

  if (!user) {
    return <Navigate to="/" />;
  }

  const onPageSelect = (type: PageType) => {
    switch (type) {
      case 'Friends': {
        dispatch(getUserFriends());
        break;
      }
    }
    setPageType(type);
  };

  const renderSwitch = (param: PageType) => {
    switch (param) {
      case 'Friends':
        return <FriendsComponent />;
      default:
        return <UsersComponent />;
    }
  };

  return (
    <Box sx={{ display: 'flex', flexGrow: '1' }}>
      <ToolbarComponent user={user}></ToolbarComponent>
      <Box
        component="nav"
        sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}
        aria-label="mailbox folders"
      >
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: 'none', sm: 'block' },
            '& .MuiDrawer-paper': {
              boxSizing: 'border-box',
              width: drawerWidth,
            },
          }}
          open
        >
          <DrawerComponent onPageSelect={onPageSelect} />
        </Drawer>
      </Box>
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          p: 3,
          width: { sm: `calc(100% - ${drawerWidth}px)` },
        }}
      >
        <Toolbar />
        {renderSwitch(pageType)}
      </Box>
    </Box>
  );
}
