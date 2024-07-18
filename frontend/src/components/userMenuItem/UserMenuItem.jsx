import React from 'react'
import MenuItem from '@mui/material/MenuItem';
import Typography from '@mui/material/Typography';

const UserMenuItem = ({ name, handleCloseUserMenu }) => {
	return (
		<MenuItem 
			onClick={handleCloseUserMenu}>
			<Typography textAlign="center">{name}</Typography>
		</MenuItem>
	)
}

export default UserMenuItem