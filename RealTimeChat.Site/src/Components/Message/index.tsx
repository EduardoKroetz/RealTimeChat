import { useContext, useEffect, useRef, useState } from "react";
import IMessage from "../../Interfaces/IMessage";
import "./style.css"
import { AuthContext } from "../../Contexts/AuthContext";
import hubConnection from "../../SignalR/hubConnection";
import UpdateMessage from "./UpdateMessage";
import { differenceInMinutes } from "date-fns";

interface IMessageProps extends IMessage
{
  lastMessageRenderedTimestamp: Date | null
  lastMessageRenderedSenderId: string | null
}

export default function Message({ content, id, sender, senderId, timestamp, lastMessageRenderedSenderId, lastMessageRenderedTimestamp }: IMessageProps)
{
  const [messageContent, setMessageContent] = useState(content);
  const [updateMessage, setUpdateMessage] = useState(false);
  const { user } = useContext(AuthContext);
  const [messageActionDropDown, setMessageActionDropDown] = useState(false);
  const [initialMessage] = useState(content);
  const messageRef = useRef<HTMLDivElement>(null)
  const renderUserMessageGroup = senderId === lastMessageRenderedSenderId && differenceInMinutes(new Date(timestamp), new Date(lastMessageRenderedTimestamp ?? "")) < 10;
  const isMyMessage = senderId === user?.id;

  timestamp = new Date(timestamp);
  const minutes = timestamp.getMinutes();
  const hours = timestamp.getHours();
  const formatedTime = `${hours < 10 ? "0" + hours : hours}:${minutes < 10 ? "0" + minutes : minutes}`

  const handleDeleteAction = async () =>
  {
    await hubConnection.invoke("DeleteMessageAsync", id);
    handleLeaveDropDown();
  }

  const exitDropDown = () =>
  {
    setUpdateMessage(false);
    setMessageActionDropDown(false);
  }

  const handleLeaveDropDown = () =>
  {
    if (updateMessage)
    {
      setMessageContent(initialMessage)
    }
    exitDropDown();
  }

  const handleDropDown = (event: any) => 
  {
    event.stopPropagation();
    if (messageActionDropDown)
      handleLeaveDropDown()
    else
      setMessageActionDropDown(true);
  }

  const handleClickOutside = (event: any) => {
    if (messageRef.current && !messageRef.current.contains(event.target)) {
      if (messageActionDropDown)
        handleLeaveDropDown();
    }
  };

  useEffect(() => {
    //Add event to leave message action component when click outside
    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [messageActionDropDown,updateMessage]);


  return (
    <div
      className={`message-container 
        ${isMyMessage ? "message-container-flex-end" : ""}  
        ${renderUserMessageGroup ? "user-message-group-container" : "not-message-group-container"}`}
      id={id}
      key={id}
    >
      {!renderUserMessageGroup && !isMyMessage && 
      (
        <img className="user-img" src="/user-icon.png" alt="" />
      )}
      <div className={`${isMyMessage ? "my-message" : "message"} ${renderUserMessageGroup ? "user-message-group" : ""}`} ref={messageRef}>
        <div className="user-info">
          {!isMyMessage && (
            <p className="username">{sender.username}</p>
          )}
          {isMyMessage && (
            <div className="user-message-actions">
              <i className={messageActionDropDown ? "fas fa-times-circle" : "fas fa-ellipsis-h"} onClick={handleDropDown}></i>
            </div>
          )}
        </div>
        <div className="message-content-container">
          {updateMessage ? 
          (
            <UpdateMessage messageId={id} messageContent={messageContent} setMessageContent={setMessageContent} handleLeaveDropDown={exitDropDown}/>
          ) : 
          (
            <p className="message-content">{messageContent}</p>
          )}
          <p className="message-timestamp">{formatedTime}</p>
        </div>
        {messageActionDropDown &&
          (
            <div className="message-action-drop-down">
              <i className="fas fa-trash-alt" onClick={handleDeleteAction}></i> 
              <i className="fas fa-edit" onClick={() => setUpdateMessage(true)}></i>
            </div>
          )}
      </div>
    </div>
  )
}