import "./style.css"

interface ConversationProps
{
  chatRoomName: string,
}

export default function Conversation({ chatRoomName } : ConversationProps )
{

  return (
    <div className="conversation-container">
      <h2>{chatRoomName}</h2>
    </div> 
  )
}