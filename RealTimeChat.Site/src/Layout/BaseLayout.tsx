import { Outlet, useLocation } from "react-router-dom";
import Conversations from "../Components/Conversations";
import Header from "../Components/Header";
import { useContext } from "react";
import { ScreenWidthContext } from "../Contexts/ScreenWidthContext";
import SearchRooms from "../Components/SearchRooms";
import { AuthContext } from "../Contexts/AuthContext";
import Toast from "../Components/Toast";
import ChatRoomActions from "../Components/ChatRoomActions";


export default function BaseLayout()
{
  const screenWidth = useContext(ScreenWidthContext);
  const location = useLocation();
  const isHomePage = location.pathname === "/"
  const { jwtToken } = useContext(AuthContext);

  if (!jwtToken)
    return <></>

  return (
    <div className="app">
      <Header />
      {screenWidth < 425 && isHomePage && 
        <SearchRooms />
      }
      <div className="main-content">
        {isHomePage && (
          <div className="chats">
            <Conversations />
            <ChatRoomActions />
          </div>
        )}
        {screenWidth >= 768 && !isHomePage && (
          <div className="chats">
            <Conversations />
            <ChatRoomActions />
          </div>
        )}
        <Outlet />
      </div>
      <Toast />
    </div>
  )
}