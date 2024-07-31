import { useContext } from "react";
import { ScreenWidthContext } from "../../Contexts/ScreenWidthContext";
import "./style.css"
import DefaultChatRoom from "../../Components/DefaultChatRoom";

export default function HomePage()
{
  const screenWidth = useContext(ScreenWidthContext);

  return (
    <>
      {screenWidth >= 768 && (
        <DefaultChatRoom/>
      )}
    </>
  )
}