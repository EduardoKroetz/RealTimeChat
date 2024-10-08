import { useContext, useEffect, useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
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
import { leaveChatRoom } from "../../services/leaveChatRoom";
import { AuthContext } from "../../Contexts/AuthContext";
import ChatName from "../ChatName";
import { ToastContext } from "../../Contexts/ToastContext";

interface chatRoomProps
{
  isConnected: boolean,
  id: string
}

export default function ChatRoom({isConnected, id}: chatRoomProps) {
  const { setMyGroups, myGroups, user } = useContext(AuthContext);
  const screenWidth = useContext(ScreenWidthContext);
  const { setToastColor, setToastIsOpen, setToastMessage } = useContext(ToastContext);

  const [chatRoom, setChatRoom] = useState<IChatRoom>();
  const [messages, setMessages] = useState<IMessage[]>([]);
  const [currentGroupId, setCurrentGroupId] = useState<string | null>(null);
  const [chatRoomLoaded, setChatRoomLoaded] = useState<boolean>(false);
  const [page, setPage] = useState<number>(1);
  const [isLimitMessages, setIsLimitMessages] = useState(false);
  const [loadingMessages, setLoadingMessages] = useState(false);
  const [firstElement, setFirstElement] = useState<HTMLElement>(null!);
  const [isEditChatName, setIsEditChatName] = useState(false);
  const [isTheOwner, setIsTheOwner] = useState(chatRoom?.createdBy === user?.id);
  const [isLoadingFirstMessages, setIsLoadingFirstMessages] = useState(true);

  const navigate = useNavigate();
  const messagesRef = useRef<HTMLDivElement>(null); 

  const chatRoomInfoRef = useRef<HTMLDivElement>(null);

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

  useEffect(() => 
  {
    if (myGroups && chatRoom)
    {
      const alreadyJoin = myGroups.find(x => x.id === chatRoom.id);
      if (!alreadyJoin)
        {
          setToastIsOpen(true);
          setToastColor("var(--accent-color)");
          setToastMessage("You are not allowed to chat in rooms you have not entered");
          navigate("/")
        }
    }
  }, [myGroups, chatRoom])

  // Call API to get data
  useEffect(() => {
    const getAsync = async () => {
      // Get chat room
      const response = await api.get(`/chatrooms/${id}`);
      if (response.status === 200)
      {
        const chatRoom = response.data.data;
        setChatRoom(chatRoom);


      }
      setChatRoomLoaded(true)

      // Get first 20 messages of chat room
      await GetMessagesFromChatRoom();
      setIsLoadingFirstMessages(false);

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
      return <div className="line-with-message">Today</div>;
    } else if (isYesterday(timestamp)) {
      return <div className="line-with-message">Yesterday</div>;
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


  const handleLeaveGroup = async () =>
  {
    if (!chatRoom)
      return
    
    const leave = await leaveChatRoom(chatRoom.id);
    if (leave)
    {
      setToastIsOpen(true);
      setToastMessage("You have successfully left the chat room!")
      setMyGroups((prevValue) => prevValue.filter((group) => group.id !== chatRoom.id))
      navigate("/")
    }
  }

  
  const handleUpdateChatRoom = async () => {
    if (!chatRoom)
      return
    const response = await api.put(`/chatrooms/${chatRoom.id}?name=${chatRoom?.name}`)
    if (response.status === 200)
    {
      setMyGroups((prevGroups) =>
      {
        const groupsCopy = [...prevGroups];
        const index = groupsCopy.findIndex(x => x.id === chatRoom.id);
        if (index !== -1) {
          groupsCopy[index] = chatRoom;
        }
        return groupsCopy;
      });
      //Open toast
      setToastIsOpen(true);
      setToastMessage("Chat room updated successfully!");
    }
    setIsEditChatName(false);
  }

  const handleDeleteChatRoom = async () => 
  {
    if (!chatRoom)
      return
    const response = await api.delete(`/chatrooms/${chatRoom.id}`)
    if (response.status === 200)
    {
      setToastIsOpen(true);
      setToastMessage("Chat room successfully deleted!")
      setMyGroups((prevValue) => prevValue.filter((group) => group.id !== chatRoom.id))
      navigate("/")
    }
  }

  useEffect(() => {
    if (chatRoom && user)
      setIsTheOwner(chatRoom.createdBy === user.id);
  }, [chatRoom, user])

  //Component

  if (!chatRoom && chatRoomLoaded) {
    return <h1 className="error-loading-chat-room">Erro ao carregar a sala de bate papo</h1>;
  }

  //load empty container
  if (!chatRoom)
    return (
      <div className="chatroom-container">
        <div className="chatroom-info"></div>
        <div className="chatroom-messages"></div>
      </div>
  )

  return (
    <div className="chatroom-container">
      <div className="chatroom-info" ref={chatRoomInfoRef}>
        {screenWidth < 768 && (
          <Link to={"/"} >
            <i className="fas fa-arrow-left back-icon" title="Back"></i>
          </Link>
        )}           
        <ChatName 
          isEditChatName={isEditChatName} 
          setIsEditChatName={setIsEditChatName} 
          containerRef={chatRoomInfoRef} 
          chatRoom={chatRoom} 
          setChatRoom={setChatRoom}/>
        <div className="actions-container">
          {isTheOwner ? (
            <>
              {!isEditChatName ? (
                <i className="fas fa-edit talk" onClick={() => setIsEditChatName(true)}></i>
              ) : (
                <i onClick={handleUpdateChatRoom} className="fas fa-check"></i>
              )}
              <i className="fas fa-trash-alt" onClick={handleDeleteChatRoom}></i>
            </> 
          ): (
            <i title="Leave" onClick={handleLeaveGroup} className="fas fa-sign-out-alt leave"></i>
          )}   
   
        </div>
      </div>
      <div className="chatroom-messages" ref={messagesRef}>
        <>
          {!isLoadingFirstMessages && messages.length === 0 && (
            <h1 style={{margin: "auto"}}>Be the first to send a message!</h1>
          )}
          {messages.map((msg, index) => (
            <div key={`${msg.id}-${msg.content}`}>
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
