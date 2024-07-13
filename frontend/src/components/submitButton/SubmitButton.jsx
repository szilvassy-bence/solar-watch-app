import { Button } from "@mui/material";
import SendIcon from '@mui/icons-material/Send';

const SubmitButton = ({ text }) => {
	return (
		<Button variant="contained" type="submit" endIcon={<SendIcon />}>
			{text}
		</ Button >
	)
}

export default SubmitButton