import { createContext, useState } from "react";

interface ToastProps
{
  toastIsOpen: boolean,
  setToastIsOpen: React.Dispatch<React.SetStateAction<boolean>>,
  toastColor: string
  setToastColor: React.Dispatch<React.SetStateAction<string>>,
  toastMessage: string,
  setToastMessage: React.Dispatch<React.SetStateAction<string>>
}

const ToastContext = createContext<ToastProps>({toastIsOpen: false, setToastIsOpen: null!, toastColor: "", setToastColor: null!, toastMessage: "", setToastMessage: null! })

const ToastContextProvider = ({children}: any) =>
{
  const [toastIsOpen, setToastIsOpen] = useState(false);
  const [toastColor, setToastColor] = useState("");
  const [toastMessage, setToastMessage] = useState("");
  
  return (
    <ToastContext.Provider value={{toastIsOpen, setToastIsOpen, toastColor, setToastColor, toastMessage, setToastMessage}}>
      {children}
    </ToastContext.Provider>
  )
}

export { ToastContext, ToastContextProvider }