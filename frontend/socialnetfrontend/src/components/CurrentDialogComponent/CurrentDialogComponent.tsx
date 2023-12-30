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
import css from './CurrentDialogComponent.css';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';

export function CurrentDialogComponent() {
  const dispatch = useAppDispatch();
  const dialog = useRootSelector(selectDialogMessages);
  const [newMessage, setNewMessage] = useState<string | undefined>();

  const buddy = dialog?.buddy;
  const onAddMessage = () => {
    if (buddy && newMessage) {
      dispatch(addUserMessage(buddy.id, newMessage));
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
          Диалог с {buddy?.name} {buddy?.surname} ({buddy?.id})
        </Typography>
        {!dialog?.messages?.length ? (
          <Typography variant="h5" component="div">
            Нет сообщений от друзей
          </Typography>
        ) : (
          dialog?.messages?.map((p) => (
            <ListItem
              sx={{
                padding: '0rem',
              }}
            >
              <ListItemText
                primary={`${
                  p.authorId
                } ${p.datetime?.toLocaleString()} - ${p.message}`}
                sx={{
                  color:
                    p.authorId === buddy?.id ? '#3f50b5' : '#f44336',
                }}
              />
            </ListItem>
          ))
        )}
      </List>
    </div>
  );
}
