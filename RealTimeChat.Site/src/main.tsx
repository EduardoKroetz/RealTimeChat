import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { ScreenWidthContextProvider } from './Contexts/ScreenWidthContext.tsx'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <ScreenWidthContextProvider>
    <App />
  </ScreenWidthContextProvider>
)
