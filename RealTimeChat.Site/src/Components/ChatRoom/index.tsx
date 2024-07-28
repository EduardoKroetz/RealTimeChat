import { useContext, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import api from "../../api/axiosConfig";
import IMessage from "../../Interfaces/IMessage";
import IChatRoom from "../../Interfaces/IChatRoom";
import "./style.css"
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext";
import Message from "../Message";

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
  const screenWidth = useContext(ScreenWidthContext);

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
  }, [id])

  return (
    <div className="chatroom-container">
      <div className="chatroom-info">
        {screenWidth < 768 && (
          <Link to={"/"} className="back-link">
            <img src="/arrow-left.png" alt="Voltar" />
          </Link>
        )}
        <h1>{chatRoom?.name}</h1>
      </div>
      <div className="chatroom-messages">
      {messages.map((msg) => 
        (
          <Message chatRoomId={msg.chatRoomId} content={msg.content} id={msg.id} senderId={msg.senderId} timestamp={msg.timestamp}/>
        ))}
      </div>

    </div>
  )
}