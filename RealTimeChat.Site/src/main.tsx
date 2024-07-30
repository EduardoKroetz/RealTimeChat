import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { ScreenWidthContextProvider } from './Contexts/ScreenWidthContext.tsx'
import { AuthContextProvider } from './Contexts/AuthContext.tsx'
import { BrowserRouter } from 'react-router-dom'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <BrowserRouter>
    <AuthContextProvider>
      <ScreenWidthContextProvider>
        <App />
      </ScreenWidthContextProvider>
    </AuthContextProvider>
  </BrowserRouter>

)
