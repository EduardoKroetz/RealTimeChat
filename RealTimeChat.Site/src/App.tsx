import { useEffect, useState } from 'react';
import './App.css'
import ChatRoom from './Components/ChatRoom/index.tsx';
import Conversations from './Components/Conversations/index.tsx'
import Sidebar from "./Components/Sidebar/index.tsx"
import { Route, BrowserRouter as Router, Routes} from 'react-router-dom';
import DefaultChatRoom from './Components/DefaultChatRoom/index.tsx';

function App() {
  const [screenWidth, setScreenWidth] = useState(window.innerWidth);

  useEffect(() =>
  {
    window.addEventListener("resize", () => setScreenWidth(window.innerWidth))
  }, [])

  return (
    <Router >
      <div className="app">
        <Sidebar/>
        <div className="main-content">
          <Conversations />
          {screenWidth >= 768 && 
          (
            <Routes>
              <Route path="/" element={<DefaultChatRoom />} />
              <Route path="/chatrooms/:id" element={<ChatRoom />} />
            </Routes>
          )}
        </div>
      </div>
    </Router>
  )
}

export default App
