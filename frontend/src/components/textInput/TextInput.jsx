import TextField from '@mui/material/TextField';

const TextInput = ({ label, defaultValue, helperText, onChange }) => {
	return (
		<TextField
          id="outlined-error-helper-text"
          label={label}
          defaultValue={defaultValue}
          helperText={helperText}
          onChange={onChange}
        />
	)
}

export default TextInput