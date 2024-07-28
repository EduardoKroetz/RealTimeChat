import { useEffect, useRef, useState } from "react";
import hubConnection from "../../SignalR/hubConnection";
import "./style.css"

interface ISendMessageProps
{
  chatRoomId: string
}

export default function SendMessage({ chatRoomId }: ISendMessageProps)
{
  const [newMessage, setNewMessage] = useState<string>("");
  const inputRef = useRef<any>();

  const SendMessage = async () => {
    if (newMessage.trim() === '')
      return;
    await hubConnection.invoke("SendMessageAsync", chatRoomId, "user-id", newMessage);
    setNewMessage("");
  }

  useEffect(()=>
  {
    inputRef.current.focus();
  }, [inputRef])


  return (
    <div className="send-message-container">
      <input 
        ref={inputRef}
        type="text" 
        placeholder="Mensagem"
        className="send-message-input"
        value={newMessage} 
        onChange={(e) => setNewMessage(e.target.value)}/>
      
      <button className="send-message-button" onClick={SendMessage}>Enviar</button>
    </div>
  )
}