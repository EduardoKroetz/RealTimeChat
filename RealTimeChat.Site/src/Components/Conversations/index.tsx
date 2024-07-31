
import { useEffect, useState } from "react"
import ChatRoom from "../../Interfaces/IChatRoom";
import "./style.css"
import Conversation from "../Conversation";
import api from "../../api/axiosConfig";
import React from "react";

const Conversations = React.memo(() =>
{
  const [chatRooms, setChatRooms] = useState<ChatRoom[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => 
  {
    setIsLoading(true)
    const getUserChatRooms = async () =>
    {
      var response = await api.get("/chatrooms/users");
      setChatRooms(response.data.data);
      setIsLoading(false)
    }

    getUserChatRooms();
  }, [])

  return (
    <div className="conversations-container">
      <div className="conversations-chatrooms">
        {!isLoading && chatRooms.length === 0 &&
        (
          <h3>No chat rooms... 😢</h3>
        )}
        {chatRooms.map((chatRoom) => 
          (
            <div key={chatRoom.id}>
              <Conversation chatRoomName={chatRoom.name} chatRoomId={chatRoom.id} />
            </div>
          )
        )}
      </div>
    
    </div>
  )
})

export default Conversations