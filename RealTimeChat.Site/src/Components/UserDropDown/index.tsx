import { useContext, useEffect, useRef } from "react";
import { AuthContext } from "../../Contexts/AuthContext";
import { Link } from "react-router-dom";
import "./style.css";
import { format } from "date-fns";
import Cookies from "js-cookie";

interface UserInfoProps {
  userInfoIsOpen: boolean;
  userIconRef: React.RefObject<HTMLElement>;
  setUserInfoIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
}

export default function UserDropDown({
  setUserInfoIsOpen,
  userIconRef,
  userInfoIsOpen
}: UserInfoProps) {
  const userInfoContainerRef = useRef<HTMLDivElement>(null);
  const { user } = useContext(AuthContext);

  const handleLogout = () => {
    Cookies.set("JwtToken", "");
    window.location.pathname = "/login"
  }

  const handleClickOutside = (event: any) => {
    if (
      userInfoContainerRef.current &&
      userIconRef.current &&
      !userInfoContainerRef.current.contains(event.target) &&
      !userIconRef.current.contains(event.target)
    ) {
      setUserInfoIsOpen(false);
    }
  };

  useEffect(() => {
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [userInfoIsOpen]);

  if (!user)
  {
    return (
      <div className="user-drop-down-container">
        <Link to={"/login"}>Login</Link>
      </div>
    )
  }

  return (
    <div
      ref={userInfoContainerRef}
      className={`user-drop-down-container ${userInfoIsOpen ? "open" : ""}`}
    >
      <div className="user-drop-down-content">
        <div className="user-drop-down-content-container">
          <label>Id</label>
          <input type="text" disabled value={user?.id} />
        </div>
        <div className="user-drop-down-content-container">
          <label>Username</label>
          <input type="text" disabled value={user?.username} />
        </div>
        <div className="user-drop-down-content-container">
          <label>Email</label>
          <input type="text" disabled value={user?.email} />
        </div>
        <div className="user-drop-down-content-container">
          <label>Created At</label>
          <input
            type="text"
            disabled
            value={format(new Date(user.createdAt), "yyyy-MM-dd HH:mm")}
          />
        </div>
      </div>
      <div className="user-drop-down-actions">
        <i onClick={handleLogout} title="End session" className="fas fa-sign-out-alt logout"></i>
      </div>
    </div>
  );
}
