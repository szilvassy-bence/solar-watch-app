import React from 'react';
import ReactDOM from 'react-dom/client';
import {createBrowserRouter, RouterProvider} from 'react-router-dom';
import './index.css';
import Root from './layout/root';
import Error from './pages/error'
import Home from './pages/home';
import Cities from './pages/cities';


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
        }

    ]
  }
]);

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
);
