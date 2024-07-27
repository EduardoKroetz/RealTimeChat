import './App.css'
import Conversations from './Components/Conversations/index.tsx'
import Sidebar from "./Components/Sidebar/index.tsx"

function App() {
  return (
    <div className='app'>
      <Sidebar/>
      <Conversations/>
    </div>
  )
}

export default App
