import React, { lazy, Suspense } from 'react';
import {
  FriendsComponent,
  LoginComponent,
  PostsComponent,
  UsersComponent,
} from './components';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

const App: React.FC = () => {
  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          <Route path="/" Component={LoginComponent} />
          <Route path="/users" Component={UsersComponent} />
          <Route path="/friends" Component={FriendsComponent} />
          <Route path="/posts" Component={PostsComponent} />
        </Routes>{' '}
      </BrowserRouter>
    </div>
  );
};

export default App;
