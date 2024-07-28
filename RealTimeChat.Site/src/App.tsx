import './App.css'

import { Route, BrowserRouter as Router, Routes} from 'react-router-dom';
import ChatRoomPage from './pages/ChatRoom/index.tsx';
import HomePage from './pages/Home/index.tsx';
import Header from './Components/Header/index.tsx';

function App() {
  return (
    <Router >
      <div className="app">
        <Header/>
        <Routes>
          <Route path="/" Component={HomePage}/>
          <Route path="/chatrooms/:id" Component={ChatRoomPage}/>
        </Routes>
      </div>
    </Router>
  )
}

export default App
