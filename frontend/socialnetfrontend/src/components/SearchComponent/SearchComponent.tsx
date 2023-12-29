import React, { useState, ChangeEventHandler } from 'react';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import css from './SearchComponent.css';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import ListItemAvatar from '@mui/material/ListItemAvatar';
import Avatar from '@mui/material/Avatar';
import ListItemIcon from '@mui/material/ListItemIcon';
import SendIcon from '@mui/icons-material/Send';
import AddAPhotoIcon from '@mui/icons-material/AddAPhoto';
import {
  useAppDispatch,
  useRootSelector,
  selectFoundUsers,
  searchUsers,
  addUserFriends,
  getUserDialog,
  setActivePage,
} from '../../core/store';
import TextField from '@mui/material/TextField';
import { User } from '../../core/types';

export function SearchComponent() {
  const dispatch = useAppDispatch();
  const users = useRootSelector(selectFoundUsers);
  const [userId, setUserId] = useState(1645801);
  const [name, setName] = useState<string | undefined>(undefined);
  const [surname, setSurname] = useState<string | undefined>(
    undefined
  );
  const onSearch = () => {
    dispatch(searchUsers(name, surname));
  };
  const onNameChange: ChangeEventHandler<
    HTMLInputElement | HTMLTextAreaElement
  > = (e) => {
    setName(e.target.value);
  };
  const onSurnameChange: ChangeEventHandler<
    HTMLInputElement | HTMLTextAreaElement
  > = (e) => {
    setSurname(e.target.value);
  };

  const onAddFriend = (user: User) => {
    dispatch(addUserFriends(user.id));
  };

  const onStartDialog = (user: User) => {
    dispatch(getUserDialog(user.id));
    dispatch(setActivePage('Dialogs'));
  };

  return (
    <div className={css.panel}>
      <TextField
        required
        id="outlined-required"
        label="Имя"
        value={name}
        onChange={onNameChange}
      />
      <TextField
        required
        id="outlined-required"
        label="Фамилия"
        value={surname}
        onChange={onSurnameChange}
      />
      <Button variant="contained" onClick={onSearch}>
        Найти
      </Button>
      {!users?.length ? (
        <Typography variant="h5" component="div">
          Никого не найдено
        </Typography>
      ) : (
        <>
          <Typography variant="h5" component="div">
            Найдено: {users.length}
          </Typography>
          <List
            sx={{
              width: '100%',
              bgcolor: 'background.paper',
            }}
          >
            {users.map((u) => (
              <ListItem>
                <ListItemText
                  primary={`${u.surname} ${u.name} (${u.id})`}
                  secondary={u.city}
                />
                <Button
                  variant="outlined"
                  startIcon={<AddAPhotoIcon />}
                  onClick={() => onAddFriend(u)}
                >
                  В друзья
                </Button>
                <Button
                  variant="contained"
                  endIcon={<SendIcon />}
                  onClick={() => onStartDialog(u)}
                >
                  Диалог
                </Button>
              </ListItem>
            ))}
          </List>
        </>
      )}
    </div>
  );
}
