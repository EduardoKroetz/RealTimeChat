import { useParams } from "react-router-dom";
import ChatRoom from "../ChatRoom";

export default function ChatRoomWrapper({isConnected}:any)
{
  const { id } = useParams();

  if (!id)
    return <></>

  return <ChatRoom key={id} isConnected={isConnected} id={id}/> 
}