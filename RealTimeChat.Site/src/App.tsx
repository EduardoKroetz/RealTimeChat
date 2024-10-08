import './App.css'

import HomePage from './pages/Home/index.tsx';
import { useEffect, useState } from 'react';
import hubConnection from './SignalR/hubConnection.ts';
import { HubConnectionState } from '@microsoft/signalr';
import LoginPage from './pages/Login/index.tsx';
import { Route, Routes } from 'react-router-dom';
import RegisterPage from './pages/Register/index.tsx';
import BaseLayout from './Layout/BaseLayout.tsx';
import ChatRoomWrapper from './Components/ChatRoomWrapper/index.tsx';

function App() {
  const [isConnected, setIsConnected] = useState<boolean>(false);

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

  return (
    <Routes>
      <Route path="/" element={<BaseLayout />}>
        <Route index element={<HomePage />}/>
        <Route path="/chatrooms/:id" element={<ChatRoomWrapper isConnecte={isConnected}/>}/>
      </Route>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
    </Routes>
  )
}

export default App
