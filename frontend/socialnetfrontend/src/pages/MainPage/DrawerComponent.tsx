import React, { useEffect, useState } from 'react';
import Divider from '@mui/material/Divider';
import InboxIcon from '@mui/icons-material/MoveToInbox';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import MailIcon from '@mui/icons-material/Mail';
import Toolbar from '@mui/material/Toolbar';
import Switch from '@mui/material/Switch';

import FormControlLabel from '@mui/material/FormControlLabel';
import FormGroup from '@mui/material/FormGroup';
import { pages } from './types';

export function DrawerComponent(props: {
  onPageSelect: (PageType) => void;
}) {
  const { onPageSelect } = props;
  const [lang, setLang] = useState<'CSharp' | 'Golang'>('CSharp');
  const handleLangChange = () => {
    setLang((p) => (p === 'CSharp' ? 'Golang' : 'CSharp'));
  };
  return (
    <div>
      <Toolbar>
        <FormGroup>
          <FormControlLabel
            control={
              <Switch
                checked={lang === 'CSharp'}
                onChange={handleLangChange}
              />
            }
            label={lang}
          />
        </FormGroup>
      </Toolbar>
      <Divider />
      <List>
        {[...pages.keys()].map((text, index) => (
          <ListItem key={text} disablePadding>
            <ListItemButton
              onClick={() => onPageSelect(pages.get(text))}
            >
              <ListItemIcon>
                {index % 2 === 0 ? <InboxIcon /> : <MailIcon />}
              </ListItemIcon>
              <ListItemText primary={text} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </div>
  );
}
