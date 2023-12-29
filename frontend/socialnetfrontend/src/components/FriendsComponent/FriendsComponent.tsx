import React, {
  useEffect,
  useState,
  ChangeEventHandler,
} from 'react';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import css from './FriendsComponent.css';
import axios from 'axios';
import {
  useAppDispatch,
  useRootSelector,
  selectFriends,
  deleteUserFriends,
  addUserFriends,
  getUserDialog,
  setActivePage,
} from '../../core/store';
import TextField from '@mui/material/TextField';

export function FriendsComponent() {
  const dispatch = useAppDispatch();
  const friends = useRootSelector(selectFriends);
  const [newUserId, setNewUserId] = useState<number | undefined>();

  const onDeleteFriend = (id: number) => {
    dispatch(deleteUserFriends(id));
  };
  const onAddFriend = () => {
    newUserId && dispatch(addUserFriends(newUserId));
  };

  const onOpenDialog = (id: number) => {
    dispatch(getUserDialog(id));
    dispatch(setActivePage('Dialogs'));
  };

  const onIdChange: ChangeEventHandler<
    HTMLInputElement | HTMLTextAreaElement
  > = (e) => {
    setNewUserId(Number(e.target.value));
  };

  return (
    <div className={css.panel}>
      <div className={css.addPanel}>
        <TextField
          required
          id="outlined-required"
          label="Ид"
          type="number"
          value={newUserId}
          onChange={onIdChange}
        />
        <Button variant="contained" onClick={() => onAddFriend()}>
          Добавить
        </Button>
      </div>
      {friends.map((e) => (
        <Card key={e.id.toString()} variant="outlined">
          <CardContent>
            <Typography variant="body2">{e.id}</Typography>
            <Typography variant="h5" component="div">
              {e.surname}
            </Typography>
            <Typography sx={{ mb: 1.5 }} color="text.secondary">
              {e.name}
            </Typography>
            <Typography
              sx={{ fontSize: 14 }}
              color="text.secondary"
              gutterBottom
            >
              Город: {e.city}
            </Typography>
            <Typography variant="body2">
              Пол: {e.sex}
              <br />
              Возраст: {e.age}
            </Typography>
          </CardContent>
          <CardActions>
            <Button size="small" onClick={() => onOpenDialog(e.id)}>
              Написать
            </Button>
            <Button size="small" onClick={() => onDeleteFriend(e.id)}>
              Удалить
            </Button>
          </CardActions>
        </Card>
      ))}
    </div>
  );
}
