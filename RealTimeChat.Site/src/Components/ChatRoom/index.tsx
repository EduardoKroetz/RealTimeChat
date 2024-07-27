import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import api from "../../api/axiosConfig";
import IMessage from "../../Interfaces/Message";
import IChatRoom from "../../Interfaces/ChatRoom";
import "./style.css"

async function GetMessagesFromChatRoom(pageNumber: number, pageSize: number, chatRoomId: string) : Promise<IMessage[]>
{
  var response = await api.get(`/messages/chatrooms/${chatRoomId}?pageSize=${pageSize}&pageNumber=${pageNumber}`)
  return response.data.data;
}


export default function ChatRoom()
{
  const { id } = useParams();

  const [chatRoom, setChatRoom] = useState<IChatRoom>();
  const [messages, setMessages] = useState<IMessage[]>([])
  const [page, setPage] = useState<number>(1);

  useEffect(() => 
  {

    const getAsync = async () => {
      //Get chat room
      const response = await api.get(`/chatrooms/${id}`);
      setChatRoom(response.data.data);

      //Get first 20 messages of chat room
      var initialMessages = await GetMessagesFromChatRoom(1, 20, id!);
      setMessages(initialMessages);
    }

    getAsync();
  }, [])

  return (
    <div className="chatroom-container">
      <h1>{chatRoom?.name}</h1>
      {messages.map((msg) => 
        (
          <div>
            {msg.content}
          </div>
        ))}
    </div>
  )
}