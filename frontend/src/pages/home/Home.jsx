import React from 'react';
import {Link} from 'react-router-dom';
import './Home.css';
import TextInput from '../../components/textInput';
import SubmitButton from '../../components/submitButton'
import Container from '@mui/material/Container';
import { Grid } from '@mui/material';
import CityCard from '../../components/cityCard';

export default function Home() {
    
    /* const value = useContext(AuthContext); */

    const [cityName, setCityName] = React.useState("Budapest");
    const [city, setCity] = React.useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const res = await fetch(`/api/City/${cityName}`);
            if (res.ok) {
                const data = await res.json();
                setCity(data);
                console.log(data);
            } else {
                const data = await res.json();
                alert('There was a problem: ' + data);
            }
        } catch (error) {
            console.log(error);
        }
    }
        
    return (
        <Container>
            <form noValidate autoComplete="off" onSubmit={handleSubmit}>
                <Grid container spacing={2} direction='row' alignItems='center'>
                    <Grid item xs={8}>
                        <TextInput 
                            label="city" 
                            defaultValue={cityName}
                            helperText="Search for a city!"
                            value={cityName}
                            onChange={(e) => setCityName(e.target.value)} />
                    </Grid>
                    <Grid item xs={4}>
                        <SubmitButton text="Search" />
                    </Grid>
                </Grid>
            </form>
            { city && (
                <CityCard city={city} />
            )}

            {/* { !value || value.user == null  ? (
                <>
                    <h1>One more step..</h1>
                    <p>
                        Currently you are not logged in, <br/>
                        <Link to="/login">Login</Link> or <Link to="/register">Register</Link> to use our app!
                    </p>
                </>
            ) : (
                <>
                    <h1>Welcome, {value.user.userName}</h1>
                    <p>
                    Browse our site,<br/>
                    <Link to="/cities">Cities</Link> or <Link to="/profile">Profile</Link> are in your service!
                    </p>
                </>
            )} */}
        </Container>
    )
}