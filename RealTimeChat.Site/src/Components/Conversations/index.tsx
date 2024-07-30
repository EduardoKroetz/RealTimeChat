
import { useEffect, useState } from "react"
import ChatRoom from "../../Interfaces/IChatRoom";
import "./style.css"
import Conversation from "../Conversation";
import api from "../../api/axiosConfig";

export default function Conversations()
{
  const [chatRooms, setChatRooms] = useState<ChatRoom[]>([]);

  useEffect(() => 
  {
    const getUserChatRooms = async () =>
    {
      var response = await api.get("/chatrooms/users");
      setChatRooms(response.data.data);
    }

    getUserChatRooms();
  }, [])

  return (
    <div className="conversations-container">
      <div className="conversations-chatrooms">
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
}