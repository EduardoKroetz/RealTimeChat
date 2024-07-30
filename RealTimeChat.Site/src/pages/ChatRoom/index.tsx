import Conversations from "../../Components/Conversations";
import ChatRoom from "../../Components/ChatRoom";
import { useContext } from "react";
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext";
import Header from "../../Components/Header";

export default function ChatRoomPage({ isConnected }: any)
{
  const screenWidth = useContext(ScreenWidthContext);

  return (
    <>
      <Header />
      <div className="main-content">
        {screenWidth >= 768 && (
          <Conversations />
        )}
        <ChatRoom isConnected={isConnected}/>
      </div>
    </>
  )
}