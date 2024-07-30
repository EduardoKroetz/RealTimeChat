import IMessage from "../../Interfaces/IMessage";
import "./style.css"

export default function Message({ content, id, sender, timestamp }: IMessage)
{
  timestamp = new Date(timestamp);
  
  return (
    <div className="message-container" key={id}>
      <img className="user-img" src="/user-icon.png" alt="" />
      <div className="message">
        <p className="username">{sender.username}</p>
        <div className="message-content">
          <p>{content}</p>
          <p>{`${timestamp.getHours()}:${timestamp.getMinutes()}`}</p>
        </div>
      </div>
    </div>
  )
}