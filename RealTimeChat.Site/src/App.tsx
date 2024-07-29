import './App.css'

import { Route, BrowserRouter as Router, Routes} from 'react-router-dom';
import ChatRoomPage from './pages/ChatRoom/index.tsx';
import HomePage from './pages/Home/index.tsx';
import Header from './Components/Header/index.tsx';
import { useEffect, useState } from 'react';
import hubConnection from './SignalR/hubConnection.ts';
import { HubConnectionState } from '@microsoft/signalr';

function App() {
  const [isConnected, setIsConnected] = useState<boolean>(false);

  useEffect(() => {
    const connect = async () => {
      if (hubConnection.state === HubConnectionState.Disconnected) {
        try {
          await hubConnection.start();
          setIsConnected(true);
          console.log("Connected to SignalR hub");
        } catch (err) {
          console.error("Error connecting to SignalR hub", err);
        }
      }
    };

    connect();

    return () => {
      hubConnection.stop();
    };
  }, []);

  return (
    <Router >
      <div className="app">
        <Header/>
        <Routes>
          <Route path="/" Component={HomePage}/>
          <Route path="/chatrooms/:id" Component={() => <ChatRoomPage isConnected={isConnected}/>}/>
        </Routes>
      </div>
    </Router>
  )
}

export default App
