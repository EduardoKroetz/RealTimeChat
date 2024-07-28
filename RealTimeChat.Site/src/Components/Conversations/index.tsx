
import { useEffect, useState } from "react"
import api from "../../api/axiosConfig"
import ChatRoom from "../../Interfaces/IChatRoom";
import "./style.css"
import Conversation from "../Conversation";

export default function Conversations()
{
  const [chatRooms, setChatRooms] = useState<ChatRoom[]>([]);

  useEffect(() => 
  {
    const getUserChatRooms = async () =>
    {
      var response = await api.get("/chatrooms/users/e0c60f68-bf1a-4236-8161-956a190306bb");
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