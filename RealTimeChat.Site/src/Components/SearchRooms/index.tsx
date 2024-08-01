import { useRef, useState } from "react"
import "./style.css"
import RoomsResultsDropDown from "./RoomsResultsDropDown";
import IChatRoom from "../../Interfaces/IChatRoom";
import api from "../../api/axiosConfig";

export default function SearchRooms()
{
  const inputRef = useRef<HTMLInputElement>(null)
  const searchContainerRef = useRef<HTMLDivElement>(null);
  const [resultsIsOpen, setResultsIsOpen] = useState(false);
  const [results, setResults] = useState<IChatRoom[]>([]);
  const [name, setName] = useState("");

  const handleRequestResults = async () => {
    setResultsIsOpen(true);
    const response = await api.get("/chatrooms/search?name="+name); 
    if (response.status === 200)
    {
      setResults(response.data.data)
    }
  }

  const handleKeyDown = async (event:React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === "Enter")
      handleRequestResults();
  }

  return (
    <div ref={searchContainerRef} className="search-rooms-container">
      <input 
        type="text" 
        ref={inputRef} 
        value={name}
        onChange={(e) => setName(e.target.value)}
        onKeyDown={(e) => handleKeyDown(e)} 
        placeholder="Search rooms" 
        className="search-rooms-input"/>
      <i className="fas fa-search" onClick={handleRequestResults}></i>

      <RoomsResultsDropDown results={results} searchContainerRef={searchContainerRef} resultsIsOpen={resultsIsOpen} setResultsIsOpen={setResultsIsOpen}/>
    </div>

  )
}