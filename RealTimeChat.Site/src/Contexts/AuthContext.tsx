import React, { createContext, useEffect, useState } from "react";
import IUser from "../Interfaces/IUser";
import Cookies from "js-cookie";
import { useLocation, useNavigate } from "react-router-dom";
import api from "../api/axiosConfig";
import IChatRoom from "../Interfaces/IChatRoom";

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
  const navigate = useNavigate();
  const location = useLocation();

  //Get token from cookie
  useEffect(() =>
  {
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
      api.interceptors.request.use(
        config => {
          config.headers['Authorization'] = "Bearer " + token;        
          return config;
        },
        error => Promise.reject(error)
      );

      getUser();
      getMyGroups();
    }
    else if (location.pathname !== "/login" && location.pathname !== "/register")
      navigate("/login")
    
  }, [])

  return (
    <AuthContext.Provider value={{jwtToken,setJwtToken, user, myGroups, setMyGroups}}>
      {children}
    </AuthContext.Provider>
  )
}

export { AuthContextProvider, AuthContext }