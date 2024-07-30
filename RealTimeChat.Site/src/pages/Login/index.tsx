
import { Link, useNavigate } from "react-router-dom"
import "./style.css"
import { useContext, useState } from "react"
import { AuthContext } from "../../Contexts/AuthContext";
import Cookies from "js-cookie";
import api from "../../api/axiosConfig";

export default function LoginPage()
{
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const authContext = useContext(AuthContext);
  const navigate = useNavigate();

  const handleSubmit = async () =>
  {
    var response = await api.post("/auth/login", { email: email, password: password})
    if (response.status === 200)
    { 
      var token = response.data.data.token;
      authContext.setJwtToken(token)
      Cookies.set("JwtToken", token, { secure: true, sameSite: "Strict"})
      navigate("/")
    }
  }  

  return (
    <div className="login-page-container">
      <div className="login-info">
        <h1>RealTimeChat</h1>
        <p>Site para envio de mensagens em tempo real</p>
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
          <label htmlFor="password">Senha</label>
          <input 
            type="text" 
            id="password"
            value={password}
            onChange={(e) => setPassword(e.currentTarget.value)} /> 
        </div>

        <button className="login-button" onClick={() => handleSubmit()}>Entrar</button>
        <hr />
      </div>
      <div className="login-register-container">
        <p>Ainda não se registrou?</p>
        <Link to={"/register"}>
          <button className="register-button">Registre-se</button>
        </Link>
      </div>
    </div>
  )
} 