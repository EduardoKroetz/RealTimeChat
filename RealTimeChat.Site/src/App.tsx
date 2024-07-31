import './App.css'

import HomePage from './pages/Home/index.tsx';
import { useContext, useEffect, useState } from 'react';
import hubConnection from './SignalR/hubConnection.ts';
import { HubConnectionState } from '@microsoft/signalr';
import LoginPage from './pages/Login/index.tsx';
import { Route, Routes } from 'react-router-dom';
import { AuthContext } from './Contexts/AuthContext.tsx';
import RegisterPage from './pages/Register/index.tsx';
import BaseLayout from './Layout/BaseLayout.tsx';
import ChatRoom from './Components/ChatRoom/index.tsx';

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
    <Routes>
      <Route path="/" element={<BaseLayout/>}>
        <Route index Component={HomePage}/>
        <Route path="/chatrooms/:id" Component={() => <ChatRoom isConnected={isConnected}/>}/>
      </Route>
      <Route path="/login" Component={LoginPage} />
      <Route path="/register" Component={RegisterPage} />

    </Routes>
  )
}

export default App
