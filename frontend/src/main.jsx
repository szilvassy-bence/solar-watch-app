import React from 'react';
import ReactDOM from 'react-dom/client';
import {createBrowserRouter, RouterProvider} from 'react-router-dom';
import './index.css';
import Root from './layout/root';

import Home from './pages/home';
import Error from './pages/error'


const router = createBrowserRouter([
  {
    path: "/",
    element: <Root/>,
    errorElement: <Error/>,
    
    children: [
        {index: true, element: <Home/>},

    ]
  }
]);

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
);
