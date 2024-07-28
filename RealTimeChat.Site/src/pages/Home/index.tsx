import Conversations from "../../Components/Conversations";
import { useContext } from "react";
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext";
import "./style.css"
import DefaultChatRoom from "../../Components/DefaultChatRoom";

export default function HomePage()
{
  const screenWidth = useContext(ScreenWidthContext);

  return (
    <>
      <div className="homepage-main-content">
        <Conversations />
        {screenWidth >= 768 && (
          <DefaultChatRoom/>
        )}
      </div>
    </>
  )
}