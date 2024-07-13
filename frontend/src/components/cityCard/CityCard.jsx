import React from 'react';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import { List } from '@mui/material';

const CityCard = ({ city }) => {
	return (
		<React.Fragment>
    <CardContent>
      <Typography variant="h5" component="div">
       {city.name}
      </Typography>
      <Typography sx={{ mb: 1.5 }} color="text.secondary">
        {city.country}
      </Typography>
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
    </CardContent>
    <CardActions>
      <Button size="small">Learn More</Button>
    </CardActions>
  </React.Fragment>
	)
}

export default CityCard