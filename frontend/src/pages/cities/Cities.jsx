import React from 'react';
import { Container } from '@mui/material';
import CityCard from '../../components/cityCard';

const Cities = () => {

	const [cities, setCities] = React.useState([]);
	
	React.useEffect(() => {
		const fetchCities = async() => {
			try {
				const res = await fetch('/api/city/cities');
				if (res.ok) {
					const data = await res.json();
					setCities(data);
				} else {
					const data = await res.json();
					console.log(data);
				}
			} catch (error) {
				console.log(error);
			}
		};
		fetchCities();
	}, []);

	return (
		<Container>
			<h1>Cities</h1>
			{cities.length !== 0 ? (
				cities.map(c => (
					<CityCard key={c.id} city={c} />
				))
			) : (<p>There are no cities in the database.</p>)}
		</Container>
	)
}

export default Cities