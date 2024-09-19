import { useCallback, useContext, useEffect, useRef, useState } from "react"
import "./style.css"
import api from "../../api/axiosConfig";
import { AuthContext } from "../../Contexts/AuthContext";
import { ToastContext } from "../../Contexts/ToastContext";

export default function ChatRoomActions()
{
  const icon1Ref = useRef<HTMLElement>(null)
  const icon2Ref = useRef<HTMLElement>(null)
  const containerRef = useRef<HTMLDivElement>(null);
  const [newChatName, setNewChatName] = useState("");
  const [createChatIsOpen, setCreateChatIsOpen] = useState(false);
  const { setMyGroups } = useContext(AuthContext);
  const { setToastIsOpen, setToastMessage } = useContext(ToastContext);
  const [isCreating, setIsCreating] = useState(false);

  const handleCreateChatRoom = async () =>
  {
    if (isCreating === true)
      return;

    setIsCreating(true);
    const response = await api.post(`/chatrooms?name=${newChatName}`)
    if (response.status === 200)
    {
      var getChatResponse = await api.get(`/chatrooms/${response.data.data.id}`)
      if (getChatResponse.status === 200)
      {
        setMyGroups((prevValue) => [getChatResponse.data.data, ...prevValue])
        setToastIsOpen(true);
        setToastMessage("Chat room created successfully!");
      }
    }
    setIsCreating(false);
  }

  const handleMouseEnter = () =>
  {
    if (icon1Ref.current && icon2Ref.current)
    {
      icon1Ref.current.style.color = "white";
      icon2Ref.current.style.color = "white";
    }
  }

  const handleMouseLeave = () =>
  {
    if (icon1Ref.current && icon2Ref.current)
    {
      icon1Ref.current.style.color = "var(--secondary-color)";;
      icon2Ref.current.style.color = "var(--secondary-color)";;
    }
  }

  const handleClickOut = useCallback((event: any) => {
    if (containerRef.current && !containerRef.current.contains(event.target) && createChatIsOpen)
      setCreateChatIsOpen(false);
  }, [createChatIsOpen]) 

  useEffect(() =>
  {
    if (createChatIsOpen)
      document.addEventListener("click", handleClickOut)
    return () => {
      document.removeEventListener("click", handleClickOut);
    };
  }, [createChatIsOpen])


  return (
    <div 
      ref={containerRef} 
      className="chat-room-actions-container"> 
      <div 
          title="New chat" 
          onClick={() => setCreateChatIsOpen(true)}
          onMouseEnter={handleMouseEnter} 
          onMouseLeave={handleMouseLeave} 
          className="new-room-container">
          <i ref={icon1Ref} className="fas fa-users"></i> <i ref={icon2Ref} className="fas fa-plus"></i>
      </div>
      {isCreating ? <h4>Criando...</h4> : 
        <div 
          className={`create-chat-container ${createChatIsOpen ? "open" : ""}`}
          >
          <input onChange={(e) => setNewChatName(e.target.value)} type="text" className="create-chat-input" placeholder="New chat room name"/>
          <i className="fas fa-plus create-chat-icon" style={{backgroundColor: isCreating ? "whitesmoke" : "",}} onClick={handleCreateChatRoom}></i>
        </div>
      }
    </div>
  )
}