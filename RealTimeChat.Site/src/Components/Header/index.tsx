import { Link } from "react-router-dom"
import "./style.css"
import { useContext } from "react"
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext"
import SearchRooms from "../SearchRooms";

export default function Header()
{
  const screenWidth = useContext(ScreenWidthContext);

  return (
    <div className="header-container">
      <Link to={"/"}>
        <h1 className="header-title">RealTimeChat</h1>
      </Link>
      <div className="header-content">
        {screenWidth > 425 && 
          <SearchRooms />
        }
        <div className="header-user-info">
          <i title="Você" className="fas fa-user user-icon"></i>
        </div>
      </div>
    </div>
  )
}