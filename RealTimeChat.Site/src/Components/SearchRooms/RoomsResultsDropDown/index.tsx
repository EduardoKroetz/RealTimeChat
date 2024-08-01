import "./style.css"

import IChatRoom from "../../../Interfaces/IChatRoom"
import { useEffect } from "react"
import RoomResult from "./RoomResult";

interface ResultsProps 
{
  results: IChatRoom[],
  resultsIsOpen: boolean,
  setResultsIsOpen: React.Dispatch<React.SetStateAction<boolean>>,
  searchContainerRef: React.RefObject<HTMLDivElement>
}

export default function RoomsResultsDropDown({ results, resultsIsOpen, setResultsIsOpen, searchContainerRef }: ResultsProps)
{
  const handleLeaveDropDown = (event:any) =>
  {
    if (searchContainerRef.current && !searchContainerRef.current.contains(event.target))
      setResultsIsOpen(false);
  }

  useEffect(() =>
  {
    document.addEventListener("click", handleLeaveDropDown)
    return () => {
      document.removeEventListener("click", handleLeaveDropDown)
    }
  }, [resultsIsOpen])

  return (
    <div 
      className={`room-search-drop-down ${resultsIsOpen ? "open" : ""}`}
      style={{ height: resultsIsOpen ? results.length * 50 > 500 ? "500px " : `${results.length * 50}px` : "0px" }}>
        {results.length === 0 && <div className="not-found-chat-rooms">No chat rooms found 😭...</div>}
        {results.map((room) => (
          <RoomResult room={room} key={room.id} setDropDownIsOpen={setResultsIsOpen}/>
        ))}
    </div>
  )

}