import { useCallback, useEffect, useState } from "react";
import "./style.css"
import IChatRoom from "../../Interfaces/IChatRoom";

interface IEditChatNameProps
{
  isEditChatName: boolean,
  setIsEditChatName: React.Dispatch<React.SetStateAction<boolean>>
  containerRef: React.RefObject<HTMLDivElement>,
  chatRoom: IChatRoom,
  setChatRoom: React.Dispatch<React.SetStateAction<IChatRoom | undefined>>,
}

export default function ChatName({ isEditChatName, setIsEditChatName, containerRef, chatRoom, setChatRoom }: IEditChatNameProps)
{
  const [initialName] = useState(chatRoom.name);

  const handleClickOut = useCallback((event: any) => {
    if (containerRef.current && !containerRef.current.contains(event.target) && isEditChatName)
    {
      setIsEditChatName(false);
      setChatRoom({...chatRoom,name: initialName})
    }
  }, [isEditChatName]) 

  useEffect(() =>
  {
    if (isEditChatName)
      document.addEventListener("click", handleClickOut)
    else
      document.removeEventListener("click", handleClickOut)
    return () => {
      document.removeEventListener("click", handleClickOut);
    };
  }, [isEditChatName])

  return (
    <>
      {isEditChatName ? (       
        <input 
        className="edit-chat-name-input"
        type="text" 
        value={chatRoom.name} 
        onChange={(e) => setChatRoom({...chatRoom,name: e.target.value})}/> 
        ) : (
          <h2>{chatRoom?.name}</h2>
        )}

    </>
  )
}