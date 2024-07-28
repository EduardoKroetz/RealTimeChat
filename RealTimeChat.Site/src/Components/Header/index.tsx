import { Link } from "react-router-dom"
import "./style.css"

export default function Header()
{
  return (
    <div className="header-container">
      <Link to={"/"}>
        <h1 className="header-title">RealTimeChat</h1>
      </Link>
    </div>
  )
}