import { useContext, useEffect } from "react";
import { ToastContext } from "../../Contexts/ToastContext";
import "./style.css"

export default function Toast()
{
  const { toastIsOpen, setToastIsOpen, toastMessage, setToastMessage, toastColor, setToastColor } = useContext(ToastContext);

  const handleCloseToast = () =>
  {
    setToastIsOpen(false);
    setToastMessage("");
    setToastColor("");
  }

  useEffect(() =>
  {
    if (toastIsOpen)
      setTimeout(handleCloseToast, 4000);
  },[toastIsOpen])
  
  return (
    <div className="toast-container">
      <div className={`toast-content ${toastIsOpen ? "open" : ""}`} style={{backgroundColor: toastColor != "" ? toastColor : ""}}>
        <i className="fas fa-times" onClick={handleCloseToast}></i>
        <div>{toastMessage}</div>
      </div>
    </div>
  )

}