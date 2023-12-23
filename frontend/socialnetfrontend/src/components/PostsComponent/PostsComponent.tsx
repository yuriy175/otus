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
  addUserPost,
  selectPosts,
  useAppDispatch,
  useRootSelector,
} from '../../core/store';
import css from './PostsComponent.css';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';

export function PostsComponent() {
  const dispatch = useAppDispatch();
  const posts = useRootSelector(selectPosts);
  const [newPost, setNewPost] = useState<string | undefined>();

  const onAddPost = () => {
    newPost && dispatch(addUserPost(newPost));
  };

  const onNewPostChange: ChangeEventHandler<
    HTMLInputElement | HTMLTextAreaElement
  > = (e) => {
    setNewPost(e.target.value);
  };

  return (
    <div className={css.panel}>
      <div className={css.addPanel}>
        <TextField
          required
          id="outlined-required"
          label="Сообщение"
          value={newPost}
          onChange={onNewPostChange}
        />
        <Button variant="contained" onClick={() => onAddPost()}>
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
        {!posts?.length ? (
          <Typography variant="h5" component="div">
            Нет сообщений от друзей
          </Typography>
        ) : (
          posts.map((p) => (
            <ListItem>
              <ListItemAvatar>
                <Avatar>
                  <FaceIcon />
                </Avatar>
              </ListItemAvatar>
              <ListItemText
                primary={`${p.author.surname} ${p.author.name} (${p.author.id})`}
                secondary={`${p.datetime.toLocaleString()} - ${
                  p.message
                }`}
              />
            </ListItem>
          ))
        )}
      </List>
    </div>
  );
}
