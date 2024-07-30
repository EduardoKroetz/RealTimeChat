
export default interface IMessage
{
  id: string,
  content: string,
  timestamp: Date,
  senderId: string,
  chatRoomId: string
  sender: { id: string, username: string }
}