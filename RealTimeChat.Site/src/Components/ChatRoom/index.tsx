import { useContext, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import IMessage from "../../Interfaces/IMessage";
import IChatRoom from "../../Interfaces/IChatRoom";
import "./style.css";
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext";
import Message from "../Message";
import hubConnection from "../../SignalR/hubConnection";
import SendMessage from "../SendMessage";
import { HubConnectionState } from "@microsoft/signalr";
import api from "../../api/axiosConfig";

interface chatRoomProps
{
  isConnected: boolean
}

export default function ChatRoom({isConnected}: chatRoomProps) {
  const { id } = useParams();
  const [chatRoom, setChatRoom] = useState<IChatRoom>();
  const [messages, setMessages] = useState<IMessage[]>([]);
  const screenWidth = useContext(ScreenWidthContext);
  const [currentGroupId, setCurrentGroupId] = useState<string | null>(null);

  // Get messages from chat room with pagination
  const GetMessagesFromChatRoom = async (pageNumber: number, pageSize: number, chatRoomId: string): Promise<IMessage[]> => {
    const response = await api.get(`/messages/chatrooms/${chatRoomId}?pageSize=${pageSize}&pageNumber=${pageNumber}`);
    return response.data.data;
  };

  // Method for join hub group
  const JoinGroup = async (chatRoomId: string) => {
    if (hubConnection.state === HubConnectionState.Connected && currentGroupId !== chatRoomId) {
      if (currentGroupId) {
        await hubConnection.invoke("LeaveGroupAsync", currentGroupId);
      }
      await hubConnection.invoke("JoinGroupAsync", chatRoomId);
      setCurrentGroupId(chatRoomId);
    }
  };

  // Method for leave hub group
  const LeaveGroup = async () => {
    if (hubConnection.state === HubConnectionState.Connected && currentGroupId) {
      await hubConnection.invoke("LeaveGroupAsync", currentGroupId);
      setCurrentGroupId(null);
    }
  };

  // Call API to get data
  useEffect(() => {
    const getAsync = async () => {
      // Get chat room
      const response = await api.get(`/chatrooms/${id}`);
      setChatRoom(response.data.data);

      // Get first 20 messages of chat room
      const initialMessages = await GetMessagesFromChatRoom(1, 20, id!);
      setMessages(initialMessages);

      JoinGroup(id!);
    };

    getAsync();

    // Cleanup function to leave group
    return () => {
      LeaveGroup();
    };
  }, [id]);

  // Connect and subscribe to chat hub events with SignalR
  useEffect(() => {
    const subscribeEvents = async () => {
      if (hubConnection.state === HubConnectionState.Connected) {
        try {
          hubConnection.on("ReceiveMessage", (message: IMessage) => {
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
          });

          JoinGroup(id!);
        } catch (err) {
          console.error("Error connecting to SignalR hub", err);
        }
      }
    };

    subscribeEvents();

    return () => {
      LeaveGroup();
    };
  }, [isConnected]);


  //Component

  if (!chatRoom) {
    return <h1>Erro ao carregar a sala de bate papo</h1>;
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
        {messages.map((msg) => (
          <Message 
            key={msg.id}
            sender={msg.sender}
            chatRoomId={msg.chatRoomId} 
            content={msg.content} 
            id={msg.id} 
            senderId={msg.senderId} 
            timestamp={msg.timestamp} />
        ))}
      </div>
      <SendMessage chatRoomId={chatRoom.id} />
    </div>
  );
}
