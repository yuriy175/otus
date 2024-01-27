import React from 'react';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import css from './DialogsComponent.css';
import {
  useAppDispatch,
  useRootSelector,
  getUserDialog,
  setActivePage,
  selectBuddies,
  startDialogWithUser,
} from '../../core/store';

export function DialogsComponent() {
  const dispatch = useAppDispatch();
  const buddies = useRootSelector(selectBuddies);

  const onOpenDialog = (id: number) => {
    dispatch(startDialogWithUser(id));
    dispatch(getUserDialog(id));
    dispatch(setActivePage('CurrentDialog'));
  };

  return (
    <div className={css.panel}>
      {buddies.map((e) => (
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
          </CardActions>
        </Card>
      ))}
    </div>
  );
}
