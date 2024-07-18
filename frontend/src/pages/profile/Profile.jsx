import React from 'react';
import { Container, Typography } from '@mui/material';
import { FavoriteContext } from '../../layout/root/Root';
import { useContext } from 'react';
import CityCard from '../../components/cityCard';

function Profile() {
  const [favorites, setFavorites] = useContext(FavoriteContext);

  return (
    <Container>
      <Typography variant="h3" component="h1" gutterBottom>
        Profile
      </Typography>
      <Typography variant="h4" component="h1" gutterBottom>
        Favorites
      </Typography>
      {favorites && (
        <div id="favorites">
          {favorites.map((fav) => (
            <CityCard key={fav.id} city={fav} />
          ))}
        </div>
      )}
    </Container>
  );
}

export default Profile;
