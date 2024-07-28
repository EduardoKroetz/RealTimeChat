import { useEffect, useState } from "react";
import IMessage from "../../Interfaces/IMessage";
import "./style.css"
import IUser from "../../Interfaces/IUser";
import api from "../../api/axiosConfig";

export default function Message({ chatRoomId, content, id ,senderId, timestamp }: IMessage)
{
  const [user,setUser] = useState<IUser>();
  timestamp = new Date(timestamp);

  useEffect(() => {

    const getUser = async () =>
    {
      var response = await api.get(`/users/${senderId}`);
      setUser(response.data.data);
    }

    getUser();
  })

  return (
    <div className="message-container" key={id}>
      <img className="user-img" src="/user-icon.png" alt="" />
      <div className="message">
        <p className="username">{user?.username}</p>
        <div className="message-content">
          <p>{content}</p>
          <p>{`${timestamp.getHours()}:${timestamp.getMinutes()}`}</p>
        </div>
      </div>
    </div>
  )
}