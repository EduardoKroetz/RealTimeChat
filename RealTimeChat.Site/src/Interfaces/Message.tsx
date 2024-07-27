
export default interface Message
{
  id: string,
  content: string,
  timestamp: Date,
  senderId: string,
  chatRoomId: string
}