interface ConversationProps
{
  chatRoomName: string,
}

export default function Conversation({ chatRoomName } : ConversationProps )
{

  return (
    <div>
      <h1>{chatRoomName}</h1>
    </div> 
  )
}