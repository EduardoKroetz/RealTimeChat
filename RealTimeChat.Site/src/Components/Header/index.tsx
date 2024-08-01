import { Link } from "react-router-dom"
import "./style.css"
import { useContext, useRef, useState } from "react"
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext"
import SearchRooms from "../SearchRooms";
import UserInfo from "../UserDropDown";

export default function Header()
{
  const [userInfoIsOpen, setUserInfoIsOpen] = useState(false);
  const screenWidth = useContext(ScreenWidthContext);
  const userIconRef = useRef<HTMLElement>(null);

  return (
    <div className="header-container">
      <Link to={"/"}>
        <h1 className="header-title">RealTimeChat</h1>
      </Link>
      <div className="header-content">
        {screenWidth >= 425 && 
          <SearchRooms />
        }
        <div className="header-user-info">
          <i ref={userIconRef} title="Você" className="fas fa-user user-icon" onClick={() => setUserInfoIsOpen((prevValue) => !prevValue)}></i>
        </div>
      </div>
      <UserInfo setUserInfoIsOpen={setUserInfoIsOpen} userIconRef={userIconRef} userInfoIsOpen={userInfoIsOpen}/>
    </div>
  )
}