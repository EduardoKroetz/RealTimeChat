import { useContext, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import api from "../../api/axiosConfig";
import IMessage from "../../Interfaces/IMessage";
import IChatRoom from "../../Interfaces/IChatRoom";
import "./style.css"
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext";
import Message from "../Message";
import hubConnection from "../../SignalR/hubConnection";
import SendMessage from "../SendMessage";
import { HubConnectionState } from "@microsoft/signalr";

export default function ChatRoom()
{
  const { id } = useParams();

  const [chatRoom, setChatRoom] = useState<IChatRoom>();
  const [messages, setMessages] = useState<IMessage[]>([])
  const screenWidth = useContext(ScreenWidthContext);

  //Get messages from chat room with pagination
  const GetMessagesFromChatRoom = async (pageNumber: number, pageSize: number, chatRoomId: string) : Promise<IMessage[]> =>
    {
      var response = await api.get(`/messages/chatrooms/${chatRoomId}?pageSize=${pageSize}&pageNumber=${pageNumber}`)
      return response.data.data;
    }

  //Call api to get data
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


  //Connect and to subscribe chathub events with signalR
  useEffect(() =>
  {
    const connect = async () => {
      if (hubConnection.state === HubConnectionState.Disconnected) {
        try {
          await hubConnection.start();
          console.log("Connected to SignalR hub");

          hubConnection.invoke("JoinGroupAsync", id);

          hubConnection.on("ReceiveMessage", (message: IMessage) => {
            console.log("Message received!")
            setMessages((prevMessages) => [...prevMessages, message]);
          });

          hubConnection.on("DeleteMessage", (messageId: string) => {
            setMessages((prevMessages) =>
              prevMessages.filter((msg) => msg.id !== messageId)
            );
          });

          hubConnection.on("UpdateMessage", (messageId: string, newMessage: string) => {
              setMessages((prevMessages) =>
                prevMessages.map((msg) =>
                  msg.id === messageId ? { ...msg, content: newMessage } : msg
                )
              );
            }
          );
        } catch (err) {
          console.error("Error connecting to SignalR hub", err);
        }
      }
    };
    
    connect();
  }, [])

  if (chatRoom == undefined)
  {
    return (
      <h1>Erro ao carregar a sala de bate papo</h1>
    )
  }

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
          <Message 
            key={msg.id}
            chatRoomId={msg.chatRoomId} 
            content={msg.content} 
            id={msg.id} 
            senderId={msg.senderId} 
            timestamp={msg.timestamp}/>
        ))}
      </div>
        <SendMessage chatRoomId={chatRoom.id} />
    </div>
  )
}