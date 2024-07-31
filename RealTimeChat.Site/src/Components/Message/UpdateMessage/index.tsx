import { useEffect, useRef} from "react"
import "./style.css"
import hubConnection from "../../../SignalR/hubConnection";

export default function UpdateMessage(
  { messageId, 
    messageContent, 
    setMessageContent, 
    handleLeaveDropDown
  }: 
  { messageId: string,
    messageContent: string, 
    setMessageContent:React.Dispatch<React.SetStateAction<string>>, 
    handleLeaveDropDown: () => void })
{
  const inputRef = useRef<HTMLInputElement>(null);

  const handleUpdateAction = async () =>
  {
    await hubConnection.invoke("UpdateMessageAsync", messageId, messageContent)
    handleLeaveDropDown();
  }

  useEffect(() => {
    if (inputRef.current)
      inputRef.current.focus();
  }, [])

  return (
    <div className="update-message-container">
      <input ref={inputRef} type="text" value={messageContent} onChange={(ev) => setMessageContent(ev.currentTarget.value)} placeholder="Nova mensagem..."/>
      <div className="update-message-actions">
        <i title="Salvar" className="fas fa-check-circle" onClick={handleUpdateAction}></i>
      </div>
    </div>
  )
}