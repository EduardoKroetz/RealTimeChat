import { useContext, useEffect, useRef, useState } from "react";
import hubConnection from "../../SignalR/hubConnection";
import "./style.css"
import { AuthContext } from "../../Contexts/AuthContext";

interface ISendMessageProps
{
  chatRoomId: string
}

export default function SendMessage({ chatRoomId }: ISendMessageProps)
{
  const [newMessage, setNewMessage] = useState<string>("");
  const inputRef = useRef<any>();
  const { user } = useContext(AuthContext);

  const SendMessage = async () => {
    if (newMessage.trim() === '')
      return;
    await hubConnection.invoke("SendMessageAsync", chatRoomId, user?.id, newMessage);
    setNewMessage("");
  }

  useEffect(()=>
  {
    inputRef.current.focus();
  }, [inputRef])

  const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => 
  {
    if (event.key === "Enter")
      SendMessage();
  }


  return (
    <div className="send-message-container">
      <input 
        ref={inputRef}
        type="text" 
        placeholder="Mensagem"
        className="send-message-input"
        value={newMessage} 
        onKeyDown={(e) => handleKeyDown(e)}
        onChange={(e) => setNewMessage(e.target.value)}/>
      
      <button className="send-message-button" onClick={SendMessage}>
        <img className="send-message-icon" src="/send-message-icon.png" alt="Enviar" />
      </button>
    </div>
  )
}