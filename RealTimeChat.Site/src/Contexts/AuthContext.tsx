import { createContext, useEffect, useState } from "react";
import IUser from "../Interfaces/IUser";
import Cookies from "js-cookie";
import { useNavigate } from "react-router-dom";
import api from "../api/axiosConfig";

interface IAuthContextData
{
  jwtToken: string | null,
  setJwtToken: React.Dispatch<React.SetStateAction<string>>,
  user: IUser | null,
}

const AuthContext = createContext<IAuthContextData>({ jwtToken: null, setJwtToken: null! , user: null! });

const AuthContextProvider = ({children}: any) =>
{
  const [jwtToken, setJwtToken] = useState<string>("");
  const [user, setUser] = useState<IUser | null>(null);
  const navigate = useNavigate();

  //Get token from cookie
  useEffect(() =>
  {
    const getUser = async () =>{
      var response = await api.get(`/users`, { headers: { Authorization: "Bearer "+ token } });
      setUser(response.data.data);
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
    }
    else
      navigate("/login")
    


   
  }, [])

  return (
    <AuthContext.Provider value={{jwtToken,setJwtToken, user}}>
      {children}
    </AuthContext.Provider>
  )
}

export { AuthContextProvider, AuthContext }