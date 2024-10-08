import { Link } from "react-router-dom"
import "./style.css"

interface ConversationProps
{
  chatRoomName: string,
  chatRoomId: string
}

export default function Conversation({ chatRoomName, chatRoomId } : ConversationProps )
{

  return (
    <Link to={`/chatrooms/${chatRoomId}`}>
      <div className="conversation-container">
        <h3>{chatRoomName}</h3>
      </div>
    </Link> 
  )
}