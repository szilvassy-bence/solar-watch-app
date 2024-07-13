import TextField from '@mui/material/TextField';

const TextInput = ({ label, defaultValue, helperText }) => {
	return (
		<TextField
          id="outlined-error-helper-text"
          label={label}
          defaultValue={defaultValue}
          helperText={helperText}
        />
	)
}

export default TextInput