import React, { useContext } from 'react';
import TextInput from '../../components/textInput';
import { Container, Grid, Alert, Typography } from '@mui/material';
import SubmitButton from '../../components/submitButton';
import { AuthContext } from '../../layout/root/Root';

const Login = () => {

  const [loginForm, setLoginForm] = React.useState({
    email: '',
    password: '',
  });
	const [errorMessage, setErrorMessage] = React.useState('');
	const [successMessage, setSuccessMessage] = React.useState('');

	const {login} = useContext(AuthContext);

  const handleSubmit = async (e) => {
    e.preventDefault();

		try {
			const [isSuccess, response] = await login(loginForm);
			console.log(response);
			if (isSuccess) {
				console.log(response);
				setSuccessMessage('Login is successful, redirecting..');
			} else {
				console.log(response);
				setErrorMessage('Email or password is incorrect.');
			}
		} catch (error){
			console.log(error)
		}
  };

  return (
    <Container>
      <Typography variant="h3" component="h1" gutterBottom>
        Login
      </Typography>
      <form noValidate autoComplete="off"  onSubmit={handleSubmit}>
				<Grid container spacing={2}>
					{errorMessage && (
            <Grid item xs={12}>
              <Alert severity="error">{errorMessage}</Alert>
            </Grid>
          )}
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
							label='email'
							type='email'
							defaultValue={loginForm.email}
							value={loginForm.email}
							onChange={(e) =>
								setLoginForm({ ...loginForm, email: e.target.value })
							}
							required='true'
						/>
					</Grid>
					<Grid item xs={12}>
						<TextInput
							required='true'
							label="password"
							type="password"
							defaultValue={loginForm.password}
							value={loginForm.password}
							onChange={(e) =>
								setLoginForm({ ...loginForm, password: e.target.value })
							}
						/>
					</Grid>
					<Grid item xs={4}>
						<SubmitButton text="Log in" />
					</Grid>
				</Grid>
			</form>
    </Container>
  );
};

export default Login;
