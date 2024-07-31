import { useContext, useEffect, useRef, useState } from "react";
import { Link } from "react-router-dom";
import IMessage from "../../Interfaces/IMessage";
import IChatRoom from "../../Interfaces/IChatRoom";
import "./style.css";
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext";
import Message from "../Message";
import hubConnection from "../../SignalR/hubConnection";
import SendMessage from "../SendMessage";
import { HubConnectionState } from "@microsoft/signalr";
import api from "../../api/axiosConfig";
import { AxiosResponse } from "axios";
import IResponse from "../../Interfaces/IResponse";
import { format,  isToday, isYesterday } from "date-fns";

interface chatRoomProps
{
  isConnected: boolean,
  id: string
}

export default function ChatRoom({isConnected, id}: chatRoomProps) {
  const [chatRoom, setChatRoom] = useState<IChatRoom>();
  const [messages, setMessages] = useState<IMessage[]>([]);
  const screenWidth = useContext(ScreenWidthContext);
  const [currentGroupId, setCurrentGroupId] = useState<string | null>(null);
  const [chatRoomLoaded, setChatRoomLoaded] = useState<boolean>(false);
  const [page, setPage] = useState<number>(1);
  const [isLimitMessages, setIsLimitMessages] = useState(false);
  const [loadingMessages, setLoadingMessages] = useState(false);
  const [firstElement, setFirstElement] = useState<HTMLElement>(null!);
  const messagesRef = useRef<HTMLDivElement>(null); 

  // Get messages from chat room with pagination
  const GetMessagesFromChatRoom = async () => {
    if (!isLimitMessages)
    {
      if (messages.length > 0)
      {
        const firstElementId = messages[0].id;
        const firstElement = document.getElementById(firstElementId)
        if (firstElement)
        {
          setFirstElement(firstElement);
        }
      }
      const response: AxiosResponse<IResponse<Array<IMessage>>> = await api.get(`/messages/chatrooms/${id}?pageSize=${20}&pageNumber=${page}`);
      if (response.data.data.length === 0)
      {
        setIsLimitMessages(true);
        const container = messagesRef.current;
        if (container)
          container.scrollTop = 0;
        return;
      }
      setPage((prevPage) => prevPage + 1);
      setMessages((prevMessages) => [...response.data.data.reverse(),...prevMessages]);

    }
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

  useEffect(() => {
    if (messagesRef.current && !loadingMessages) 
      messagesRef.current.scrollTop = messagesRef.current.scrollHeight;
    if (loadingMessages)
    {
      const container = messagesRef.current;
      if (container)
      {
        container.scrollTo({
          top: firstElement.offsetTop - 120,
          behavior: 'smooth'
        })
      }
      setLoadingMessages(false);
    }

  }, [messages])

  // Call API to get data
  useEffect(() => {
    const getAsync = async () => {
      // Get chat room
      const response = await api.get(`/chatrooms/${id}`);
      if (response.status === 200)
      {
        setChatRoom(response.data.data);
      }
      setChatRoomLoaded(true)

      // Get first 20 messages of chat room
      await GetMessagesFromChatRoom();

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

          hubConnection.on("UpdateMessage", (message: IMessage) => {
            setMessages((prevMessages) => {
              const messagesCopy = [...prevMessages];
              const index = messagesCopy.findIndex(x => x.id === message.id);
              if (index !== -1) {
                messagesCopy[index] = message;
              }
              return messagesCopy;
            });
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

  // Helper function to render date dividers
  const renderDateDivider = (timestamp: Date) => {
    if (isToday(timestamp)) {
      return <div className="line-with-message">Hoje</div>;
    } else if (isYesterday(timestamp)) {
      return <div className="line-with-message">Ontem</div>;
    } else {
      return <div className="line-with-message">{format(new Date(timestamp), "dd/MM/yyyy")}</div>;
    }
  };

  // Função para verificar a rolagem
  const handleScroll = async () => {
    if (messagesRef.current) {
      const { scrollTop } = messagesRef.current;
      if (scrollTop === 0 && !isLimitMessages) {
        setLoadingMessages(true);
        await GetMessagesFromChatRoom();
      }
    }
  };

  useEffect(() => {
    const container = messagesRef.current;
    if (container) {
      container.addEventListener('scroll', handleScroll);
      return () => container.removeEventListener('scroll', handleScroll);
    }
  }, [handleScroll]);

  //Component

  if (!chatRoom && chatRoomLoaded) {
    return <h1 className="error-loading-chat-room">Erro ao carregar a sala de bate papo</h1>;
  }

  return (
    <div className="chatroom-container">
      <div className="chatroom-info">
        {screenWidth < 768 && (
          <Link to={"/"} className="back-link">
            <img src="/arrow-left.png" alt="Voltar" />
          </Link>
        )}
        <h2>{chatRoom?.name}</h2>
      </div>
      <div className="chatroom-messages" ref={messagesRef}>
        <>
          {messages.map((msg, index) => (
            <div key={`${msg.id}-${msg.content}`}>
              {}
              {(index === 0 || new Date(msg.timestamp).toDateString() !== new Date(messages[index - 1].timestamp).toDateString()) && 
                renderDateDivider(msg.timestamp)}
              <Message 
              lastMessageRenderedSenderId={index > 0 ? messages[index - 1].senderId : null}
              lastMessageRenderedTimestamp={index > 0 ? messages[index - 1].timestamp : null}
              sender={msg.sender}
              chatRoomId={msg.chatRoomId} 
              content={msg.content} 
              id={msg.id} 
              senderId={msg.senderId} 
              timestamp={msg.timestamp} />
            </div>
          ))}
        </>
      </div>
      <SendMessage chatRoomId={chatRoom?.id} />
    </div>
  );
}
