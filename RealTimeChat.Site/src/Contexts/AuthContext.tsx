import React, { createContext, useContext, useEffect, useState } from "react";
import IUser from "../Interfaces/IUser";
import Cookies from "js-cookie";
import { useLocation, useNavigate } from "react-router-dom";
import api from "../api/axiosConfig";
import IChatRoom from "../Interfaces/IChatRoom";
import { ToastContext } from "./ToastContext";

interface IAuthContextData
{
  jwtToken: string | null,
  setJwtToken: React.Dispatch<React.SetStateAction<string>>,
  user: IUser | null,
  myGroups: IChatRoom[],
  setMyGroups: React.Dispatch<React.SetStateAction<IChatRoom[]>>
}

const AuthContext = createContext<IAuthContextData>({ jwtToken: null, setJwtToken: null! , user: null!, myGroups: [], setMyGroups: null! });

const AuthContextProvider = ({children}: any) =>
{
  const [jwtToken, setJwtToken] = useState<string>("");
  const [user, setUser] = useState<IUser | null>(null);
  const [myGroups, setMyGroups] = useState<IChatRoom[]>([]);
  const { setToastColor, setToastIsOpen, setToastMessage } = useContext(ToastContext);
  const navigate = useNavigate();
  const location = useLocation();

  //Get token from cookie
  useEffect(() =>
  {
    //check if response has status 401 and redirect to login page
    api.interceptors.response.use(
      response => {
        return response;
      },
      error => {
        if (error.response)
        {
          console.log("passou aqui na 66")
          setToastIsOpen(true);
          setToastColor("var(--accent-color)")
          if (error.response.status === 401)
          {
            setToastMessage("You are not authorized to access this service, redirecting...")
            setTimeout(() => navigate("/login"), 4000)
          }
          else
          {
            console.log("pass")
            setToastMessage(error.response.data.message)
          }
        }
        return Promise.reject(error);
      }
    )

    const isLoginPage = location.pathname === "/login" || location.pathname === "/register";

    const getUser = async () =>{
        const response = await api.get(`/users`, { headers: { Authorization: "Bearer "+ token } });
        if (response.status === 200)
          setUser(response.data.data);
    }

    const getMyGroups = async () => {
      const response = await api.get("/chatrooms/users")
      if (response.status === 200)
        setMyGroups(response.data.data);
    }

    const token = Cookies.get("JwtToken");
    if (token)
    {
      setJwtToken(token);
      //Add token to headers
      api.interceptors.request.use(
        config => {
          config.headers['Authorization'] = "Bearer " + token;        
          return config;
        },
      );

      if (!isLoginPage)
      {
        getUser();
        getMyGroups();
      }
    }
    else if (!isLoginPage)
      navigate("/login")
    
  }, [])

  return (
    <AuthContext.Provider value={{jwtToken,setJwtToken, user, myGroups, setMyGroups}}>
      {children}
    </AuthContext.Provider>
  )
}

export { AuthContextProvider, AuthContext }