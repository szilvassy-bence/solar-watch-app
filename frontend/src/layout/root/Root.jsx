import { useState, createContext, useEffect, useMemo } from 'react';
import './Root.css';
import { useNavigate, Outlet } from 'react-router-dom';
import Header from '../../components/header';
import Container from '@mui/material/Container';
import { useLocalStorage } from '../../hooks/useLocalStorage.jsx';

export const AuthContext = createContext(null);
export const FavoriteContext = createContext(null);

export default function Root() {
  const [user, setUser] = useLocalStorage('user', null);
  const [favorites, setFavorites] = useState([]);

  const navigate = useNavigate();

  // Get user's favorite cities
  useEffect(() => {
    async function fetchFavorites() {
      try {
        const response = await fetch('/api/User/favorites', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${user.token}`,
          },
        });
        if (response.status === 200) {
          const data = await response.json();
          setFavorites(data);
        }
      } catch (err) {
        console.error(err);
      }
    }
    if (user !== null) {
        console.log(user)
      fetchFavorites();
    } else {
      setFavorites([]);
    }
  }, [user]);

  const login = async (data) => {
    try {
      let res = await fetch('/api/Auth/Login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
      });
      if (res.status === 400) {
        let errors = await res.json();
        return [false, errors];
      }
      let user = await res.json();
      console.log(user);
      setUser(user);
      setTimeout(() => {
          navigate('/');
      }, 2000);
      return [true, user];
    } catch (e) {
      console.log(e);
    }
  };

  const logout = () => {
    setUser(null);
    navigate('/', { replace: true });
  };

  const value = useMemo(
    () => ({
      user,
      login,
      logout,
    }),
    [user]
  );

  return (
    <AuthContext.Provider value={value}>
      <FavoriteContext.Provider value={[favorites, setFavorites]}>
        <Header />
        <Container id="content" maxWidth="sm">
          <Outlet />
        </Container>
      </FavoriteContext.Provider>
    </AuthContext.Provider>
  );
}
