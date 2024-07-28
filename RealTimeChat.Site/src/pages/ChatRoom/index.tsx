import Conversations from "../../Components/Conversations";
import ChatRoom from "../../Components/ChatRoom";
import { useContext } from "react";
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext";

export default function ChatRoomPage()
{
  const screenWidth = useContext(ScreenWidthContext);

  return (
    <>
      <div className="main-content">
        {screenWidth >= 768 && (
          <Conversations />
        )}
        <ChatRoom />
      </div>
    </>
  )
}