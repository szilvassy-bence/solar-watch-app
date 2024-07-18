import React from 'react';
import TextInput from '../../components/textInput';
import { Container, Grid, Alert, Typography } from '@mui/material';
import SubmitButton from '../../components/submitButton';
import { useNavigate } from 'react-router-dom';

function Register() {

	const navigate = useNavigate();
	const [registerForm, setRegisterForm] = React.useState({
		name: '',
    email: '',
    password: '',
  });
	const [errorMessage, setErrorMessage] = React.useState('');
	const [successMessage, setSuccessMessage] = React.useState('');
	const [fieldErrors, setFieldErrors] = React.useState({})

	const handleSubmit = async (e) => {
		e.preventDefault();

		try {
			const response = await fetch("/api/Auth/Register", {
				method: "POST",
				headers: {
						"Content-Type": "application/json",
				},
				body: JSON.stringify(registerForm),
			});
			if (response.ok) {
				const success = await response.json();
				setSuccessMessage('Registration successful!');
				setErrorMessage('');
				setFieldErrors([]);
				setTimeout(() => {
					navigate('/login');
				}, 2000);
			} else {
				const error = await response.json();
				const errors = error.errors;
				console.log(error);
				console.log(errors)
        setFieldErrors(errors);
				setErrorMessage('Registration failed. Please check the inputs.');
				setSuccessMessage('');
				console.log(fieldErrors)
			}
		} catch (error) {
			console.log(error);
		}
	}
	
	return (
		<Container>
      <Typography variant="h3" component="h1" gutterBottom>
        Register
      </Typography>
      <form noValidate autoComplete="off"  onSubmit={handleSubmit}>
				<Grid container spacing={2}>
					{errorMessage && (
            <Grid item xs={12}>
              <Alert severity="error">{errorMessage}</Alert>
            </Grid>
          )}
					{successMessage && (
            <Grid item xs={12}>
              <Alert severity="success">{successMessage}</Alert>
            </Grid>
          )}
					<Grid item xs={12}>
						<TextInput
							label='userName'
							type='userName'
							defaultValue={registerForm.userName}
							value={registerForm.userName}
							onChange={(e) =>
								setRegisterForm({ ...registerForm, userName: e.target.value })
							}
							required={true}
							helperText={fieldErrors.userName ? fieldErrors.userName : ''}
              error={!!fieldErrors.userName}
						/>
					</Grid>
					<Grid item xs={12}>
						<TextInput
							label='email'
							type='email'
							defaultValue={registerForm.email}
							value={registerForm.email}
							onChange={(e) =>
								setRegisterForm({ ...registerForm, email: e.target.value })
							}
							required={true}
						/>
					</Grid>
					<Grid item xs={12}>
						<TextInput
							required='true'
							label="password"
							type="password"
							defaultValue={registerForm.password}
							value={registerForm.password}
							onChange={(e) =>
								setRegisterForm({ ...registerForm, password: e.target.value })
							}
						/>
					</Grid>
					<Grid item xs={4}>
						<SubmitButton text="Register" />
					</Grid>
				</Grid>
			</form>
    </Container>
	)
}

export default Register