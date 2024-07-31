
import { Link } from "react-router-dom"
import "./style.css"
import { useContext, useState } from "react"
import { AuthContext } from "../../Contexts/AuthContext";
import Cookies from "js-cookie";
import api from "../../api/axiosConfig";

export default function RegisterPage()
{
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [username, setUsername] = useState<string>("");
  const authContext = useContext(AuthContext);

  const handleSubmit = async () =>
  {
    var response = await api.post("/auth/register", { username: username ,email: email, password: password})
    if (response.status === 200)
    { 
      var token = response.data.data.token;
      authContext.setJwtToken(token)
      Cookies.set("JwtToken", token, { secure: true, sameSite: "Strict"})
      window.location.pathname = "/"
    }
  }  

  return (
    <div className="register-page-container">
      <div className="register-info">
        <h1>RealTimeChat - Register</h1>
        <p>Website for sending messages in real time</p>
      </div>
      <div className="form-container">
        <div className="register-input-container">
          <label htmlFor="username">Username</label>
          <input 
            type="text" 
            id="username"
            value={username}
            onChange={(e) => setUsername(e.currentTarget.value)} />
        </div>

        <div className="register-input-container">
          <label htmlFor="email">Email</label>
          <input 
            type="email" 
            id="email"
            value={email}
            onChange={(e) => setEmail(e.currentTarget.value)} />
        </div>

        <div className="register-input-container">
          <label htmlFor="password">Password</label>
          <input 
            type="text" 
            id="password"
            value={password}
            onChange={(e) => setPassword(e.currentTarget.value)} /> 
        </div>

        <button className="register-button" onClick={() => handleSubmit()}>Register</button>
        <hr />
      </div>
      <div className="register-login-container">
        <p>Already registered?</p>
        <Link to={"/login"}>
          <button className="login-button">Login</button>
        </Link>
      </div>
    </div>
  )
} 