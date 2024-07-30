import Conversations from "../../Components/Conversations";
import { useContext } from "react";
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext";
import "./style.css"
import DefaultChatRoom from "../../Components/DefaultChatRoom";
import Header from "../../Components/Header";

export default function HomePage()
{
  const screenWidth = useContext(ScreenWidthContext);

  return (
    <>
      <Header />
      <div className="main-content">
        <Conversations />
        {screenWidth >= 768 && (
          <DefaultChatRoom/>
        )}
      </div>
    </>
  )
}