import React from 'react';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import { List } from '@mui/material';
import { useContext } from "react";
import { AuthContext, FavoriteContext } from '../../layout/root/Root';
import { useNavigate } from 'react-router-dom';

const CityCard = ({ city }) => {
  const {user} = useContext(AuthContext);
  const [favorites, setFavorites] = useContext(FavoriteContext);
  const navigate = useNavigate();

  const handleDislike = async () => {
    try {
      const response = await fetch(`/api/User/${city.id}/favorites/remove`, {
        method: "PATCH",
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${user.token}`
        }
      });

      if (!response.ok) {
        throw new Error('Failed to remove city from favorites');
      }

			setFavorites(favorites.filter(fav => fav.id !== city.id))
    } catch (error) {
      console.error(error);
    }
  }

  const handleLike = async () => {
    try {
      const response = await fetch(`/api/User/${city.id}/favorites/add`, {
        method: "PATCH",
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${user.token}`
        }
      });

      if (!response.ok) {
        throw new Error('Failed to remove city from favorites');
      }

			setFavorites([...favorites, city]);
    } catch (error) {
      console.error(error);
    }
  }

  function cityClick(e) {
    let city = e.target.id;
    navigate(`/cities/${city}`);
  }


	return (
		<React.Fragment>
      <Card sx={{ border: '1px solid #1976d2', borderRadius: '4px' }}>
        <CardContent>
          <Typography variant="h5" component="div">
          {city.name}
          </Typography>
          <Typography sx={{ mb: 1.5 }} color="text.secondary">
            {city.country}
          </Typography>
          { city.latitude && city.longitude && (
            <List
            sx={{
              width: '100%',
              maxWidth: 360,
              bgcolor: 'background.paper',
              position: 'relative',
              overflow: 'auto',
              maxHeight: 300,
              '& ul': { padding: 0 },
            }}
            subheader={<li />}
            >
              <li>lat: {city.latitude}</li>
              <li>lon: {city.longitude}</li>
            </List>
          )}
        </CardContent>
        <CardActions>
          <Button onClick={cityClick} id={city.name} size="small">Learn More</Button>
          { favorites && favorites.some(c => c.id === city.id) ? 
            <Button size="small" sx={{ backgroundColor: 'red', color: 'white' }} onClick={handleDislike}>Dislike</Button> :
            <Button size="small" sx={{ backgroundColor: 'green', color: 'white' }} onClick={handleLike}>Like</Button> 
          }
        </CardActions>
      </Card>
    </React.Fragment>
	)
}

export default CityCard