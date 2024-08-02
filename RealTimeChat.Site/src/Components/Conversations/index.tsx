import "./style.css"
import Conversation from "../Conversation";
import { useContext } from "react";
import { AuthContext } from "../../Contexts/AuthContext";

const Conversations = () =>
{
  const { myGroups } = useContext(AuthContext);

  return (
    <div className="conversations-container">
      <div className="conversations-chatrooms">
        {myGroups.length === 0 &&
        (
          <h3 style={{margin: "30px"}}>No chat rooms... 😢</h3>
        )}
        {myGroups.map((chatRoom) => 
          <Conversation chatRoomName={chatRoom.name} chatRoomId={chatRoom.id} key={chatRoom.id} />
        )}
      </div>
    </div>
  )
}

export default Conversations