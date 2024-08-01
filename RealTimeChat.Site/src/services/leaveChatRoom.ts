import api from "../api/axiosConfig";

export const leaveChatRoom = async (id: string) : Promise<boolean> => {
  const response = await api.delete(`/chatrooms/leave/${id}`)
  if (response.status === 200)
    return true;
  else
   return false
}