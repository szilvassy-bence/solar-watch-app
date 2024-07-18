import TextField from '@mui/material/TextField';

const TextInput = ({ label, type, defaultValue, helperText, onChange, required, error }) => {
	return (
		<TextField
          id="outlined-error-helper-text"
          label={label}
          type={type ?? "text"}
          defaultValue={defaultValue}
          onChange={onChange}
          required={required}
          helperText={helperText}
          error={error}
        />
	)
}

export default TextInput