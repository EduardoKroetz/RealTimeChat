import IMessage from "../../Interfaces/IMessage";
import "./style.css"

export default function Message({ content, id, sender, timestamp }: IMessage)
{
  timestamp = new Date(timestamp);
  const minutes = timestamp.getMinutes();
  const hours = timestamp.getHours();
  const formatedTime = `${hours < 10 ? "0" + hours : hours}:${minutes < 10 ? "0" + minutes : minutes}`
  
  return (
    <div className="message-container" key={id}>
      <img className="user-img" src="/user-icon.png" alt="" />
      <div className="message">
        <p className="username">{sender.username}</p>
        <div className="message-content-container">
          <p className="message-content">{content}</p>
          <p>{formatedTime}</p>
        </div>
      </div>
    </div>
  )
}