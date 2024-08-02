
import { Link } from "react-router-dom"
import "./style.css"
import { useContext, useState } from "react"
import { AuthContext } from "../../Contexts/AuthContext";
import Cookies from "js-cookie";
import api from "../../api/axiosConfig";
import Toast from "../../Components/Toast";
import { ToastContext } from "../../Contexts/ToastContext";

export default function LoginPage()
{
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const authContext = useContext(AuthContext);
  const { setToastIsOpen, setToastMessage } = useContext(ToastContext);

  const handleSubmit = async () =>
  {
    var response = await api.post("/auth/login", { email: email, password: password})
    if (response.status === 200)
    { 
      var token = response.data.data.token;
      authContext.setJwtToken(token)
      Cookies.set("JwtToken", token, { secure: true, sameSite: "Strict"})
      setToastIsOpen(true);
      setToastMessage("Login successfully! Redirecting...")
      setTimeout(() => window.location.pathname = "/", 1000)
    }
  }  

  return (
    <div className="login-page-container">
      <div className="login-info">
        <h1>RealTimeChat - Login</h1>
        <p>Website for sending messages in real time</p>
      </div>
      <div className="form-container">
        <div className="login-input-container">
          <label htmlFor="email">Email</label>
          <input 
            type="text" 
            id="email"
            value={email}
            onChange={(e) => setEmail(e.currentTarget.value)} />
        </div>

        <div className="login-input-container">
          <label htmlFor="password">Password</label>
          <input 
            type="text" 
            id="password"
            value={password}
            onChange={(e) => setPassword(e.currentTarget.value)} /> 
        </div>

        <button className="login-button" onClick={() => handleSubmit()}>Login</button>
        <hr />
      </div>
      <div className="login-register-container">
        <p>Haven't registered yet?</p>
        <Link to={"/register"}>
          <button className="register-button">Register</button>
        </Link>
      </div>
      <Toast />
    </div>
  )
} 