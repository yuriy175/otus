import React, { lazy, Suspense } from 'react';

import { BrowserRouter, Routes, Route } from 'react-router-dom';

import css from './App.css';
import {
  LoginComponent,
  UsersComponent,
  FriendsComponent,
  PostsComponent,
  DialogsComponent,
} from './components';
import { MainPage } from './pages';

const App: React.FC = () => {
  return (
    <div className={css.mainDiv}>
      <BrowserRouter>
        <Routes>
          <Route path="/" Component={LoginComponent} />
          <Route path="/main" Component={MainPage} />
        </Routes>{' '}
      </BrowserRouter>
    </div>
  );
};

export default App;
