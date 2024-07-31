import { Outlet, useLocation } from "react-router-dom";
import Conversations from "../Components/Conversations";
import Header from "../Components/Header";
import { useContext } from "react";
import { ScreenWidthContext } from "../Contexts/ScreenWidthContext";
import SearchRooms from "../Components/SearchRooms";


export default function BaseLayout()
{
  const screenWidth = useContext(ScreenWidthContext);
  const location = useLocation();
  const isHomePage = location.pathname === "/"
  return (
    <div className="app">
      <Header />
      {screenWidth < 425 && isHomePage && 
        <SearchRooms />
      }
      <div className="main-content">
        {isHomePage && <Conversations />}
        {screenWidth >= 768 && <Conversations />}
        <Outlet />
      </div>
    </div>
  )
}