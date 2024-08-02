import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { ScreenWidthContextProvider } from './Contexts/ScreenWidthContext.tsx'
import { AuthContextProvider } from './Contexts/AuthContext.tsx'
import { BrowserRouter } from 'react-router-dom'
import { ToastContextProvider } from './Contexts/ToastContext.tsx'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <BrowserRouter>
    <ToastContextProvider>
      <AuthContextProvider>
        <ScreenWidthContextProvider>
          <App />
        </ScreenWidthContextProvider>
      </AuthContextProvider>
    </ToastContextProvider>
  </BrowserRouter>

)
