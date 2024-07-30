import React from 'react';
import ReactDOM from 'react-dom/client';
import {createBrowserRouter, RouterProvider} from 'react-router-dom';
import './index.css';
import Root from './layout/root';
import Error from './pages/error'
import Home from './pages/home';
import Cities from './pages/cities';
import Login from './pages/login';
import Register from './pages/Register';
import Profile from './pages/profile';
import City, { loader as cityLoader } from './pages/city';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Root/>,
    errorElement: <Error/>,
    
    children: [
        {index: true, element: <Home/>},
        {
          path: '/cities',
          element: <Cities />
        },
        {
          path: '/login',
          element: <Login />
        },
        {
          path: '/register',
          element: <Register />
        },
        {
          path: '/profile',
          element: <Profile />
        },
        {
          path: "/cities/:name",
          element: <City/>,
          loader: ({params}) => {
              return cityLoader(params.name)
          }
      },

    ]
  }
]);

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
);
