import {useState, createContext, useEffect, useMemo} from "react";
import "./Root.css";
import {useNavigate, Outlet} from "react-router-dom";
import Header from '../../components/header';
import Container from '@mui/material/Container';
/* import {useLocalStorage} from "../../hooks/useLocalStorage.jsx"; */

export const AuthContext = createContext(null);
export const FavoriteContext = createContext(null);

export default function Root() {

   
    return (
        <>
            <Header/>
            <Container id="content" maxWidth="sm">
                <Outlet/>
            </ Container >
        </>
    )
}
