import "./style.css"
import Conversation from "../Conversation";
import { useContext } from "react";
import { AuthContext } from "../../Contexts/AuthContext";
import ChatRoomActions from "../ChatRoomActions";

const Conversations = () =>
{
  const { myGroups } = useContext(AuthContext);

  return (
    <div className="conversations-container">
      <div className="conversations-chatrooms">
        {myGroups.length === 0 &&
        (
          <h3>No chat rooms... 😢</h3>
        )}
        {myGroups.map((chatRoom) => 
          <Conversation chatRoomName={chatRoom.name} chatRoomId={chatRoom.id} key={chatRoom.id} />
        )}
      </div>
      <ChatRoomActions />
    </div>
  )
}

export default Conversations