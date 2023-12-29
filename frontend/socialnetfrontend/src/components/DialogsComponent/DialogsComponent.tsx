import React, {
  ChangeEventHandler,
  useEffect,
  useState,
} from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import ListItemAvatar from '@mui/material/ListItemAvatar';
import Avatar from '@mui/material/Avatar';
import { default as FaceIcon } from '@mui/icons-material/Face';
import {
  addUserMessage,
  addUserPost,
  selectDialogMessages,
  selectPosts,
  useAppDispatch,
  useRootSelector,
} from '../../core/store';
import css from './DialogsComponent.css';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';

export function DialogsComponent() {
  const dispatch = useAppDispatch();
  const dialog = useRootSelector(selectDialogMessages);
  const [newMessage, setNewMessage] = useState<string | undefined>();

  const partner = dialog?.partner;
  const onAddMessage = () => {
    if (partner && newMessage) {
      dispatch(addUserMessage(partner.id, newMessage));
    }
  };

  const onNewMessageChange: ChangeEventHandler<
    HTMLInputElement | HTMLTextAreaElement
  > = (e) => {
    setNewMessage(e.target.value);
  };

  return (
    <div className={css.panel}>
      <div className={css.addPanel}>
        <TextField
          required
          id="outlined-required"
          label="Сообщение"
          value={newMessage}
          onChange={onNewMessageChange}
        />
        <Button variant="contained" onClick={() => onAddMessage()}>
          Добавить
        </Button>
      </div>
      <List
        sx={{
          width: '100%',
          maxWidth: 360,
          bgcolor: 'background.paper',
        }}
      >
        <Typography variant="h5" component="div">
          Диалог с {partner?.name} {partner?.surname} ({partner?.id})
        </Typography>
        {!dialog?.messages?.length ? (
          <Typography variant="h5" component="div">
            Нет сообщений от друзей
          </Typography>
        ) : (
          dialog?.messages?.map((p) => (
            <ListItem>
              <ListItemText
                primary={`${
                  p.authorId
                } ${p.datetime?.toLocaleString()} - ${p.message}`}
              />
            </ListItem>
          ))
        )}
      </List>
    </div>
  );
}
