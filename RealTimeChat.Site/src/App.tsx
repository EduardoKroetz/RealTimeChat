import './App.css'

import ChatRoomPage from './pages/ChatRoom/index.tsx';
import HomePage from './pages/Home/index.tsx';
import { useContext, useEffect, useState } from 'react';
import hubConnection from './SignalR/hubConnection.ts';
import { HubConnectionState } from '@microsoft/signalr';
import LoginPage from './pages/Login/index.tsx';
import { Route, Routes } from 'react-router-dom';
import { AuthContext } from './Contexts/AuthContext.tsx';
import RegisterPage from './pages/Register/index.tsx';

function App() {
  const [isConnected, setIsConnected] = useState<boolean>(false);
  const { jwtToken } = useContext(AuthContext);

  //Connect to chat hub
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

  if (!jwtToken)
    return <></>

  return (
    <div className="app">
      <Routes>
        <Route path="/" Component={HomePage}/>
        <Route path="/login" Component={LoginPage} />
        <Route path="/register" Component={RegisterPage} />
        <Route path="/chatrooms/:id" Component={() => <ChatRoomPage isConnected={isConnected}/>}/>
      </Routes>
    </div>
  )
}

export default App
