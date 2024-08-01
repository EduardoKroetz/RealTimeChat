import { format } from "date-fns";
import IChatRoom from "../../../../Interfaces/IChatRoom";
import { SetStateAction, useContext, useEffect, useState } from "react";
import { AuthContext } from "../../../../Contexts/AuthContext";
import "./style.css"
import api from "../../../../api/axiosConfig";
import { Link } from "react-router-dom";

export default function RoomResult({room, setDropDownIsOpen }: { room:IChatRoom, setDropDownIsOpen: React.Dispatch<SetStateAction<boolean>> })
{
  const [userAlreadyInGroup, setUserAlreadyInGroup] = useState(false);
  const { myGroups, setMyGroups } = useContext(AuthContext);

  const handleJoinGroup = async () => {
    const response = await api.post(`/chatrooms/join/${room.id}`)
    if (response.status === 200)
    {
      const chatRoomResponse = await api.get(`/chatrooms/${response.data.data.chatRoomId}`);
      if (chatRoomResponse.status === 200)
      {
        setMyGroups((prevValue) => [...prevValue, chatRoomResponse.data.data])
      }
    }
  }

  const handleLeaveGroup = async () => {
    const response = await api.delete(`/chatrooms/leave/${room.id}`)
    if (response.status === 200)
    {
      setMyGroups((prevValue) => prevValue.filter((group) => group.id !== room.id))
      setUserAlreadyInGroup(false);
    }
  }


  useEffect(() =>
  {
    if (myGroups.find(x => x.id === room.id))
      setUserAlreadyInGroup(true);
  }, [myGroups])

  return (
    <div className="room-search-result"  key={room.id}>
      <div className="room-search-content">
        <div>{room.name}</div>
        <div className="created-at">{format(new Date(room.createdAt), "yyyy-MM-dd")}</div>
      </div>
      <div className="room-search-actions">
        {userAlreadyInGroup ? 
          <> 
            <i title="Leave" onClick={handleLeaveGroup} style={{marginRight: "25px"}} className="fas fa-sign-out-alt leave"></i>
            <Link to={`/chatrooms/${room.id}`} ><i title="Talk" className="fas fa-comments talk" onClick={() => setDropDownIsOpen(false)}></i></Link>
          </>
          :
          <i title="Join" className="fas fa-user-plus join" onClick={handleJoinGroup}></i> 
        }
      </div>
    </div>
  )
}