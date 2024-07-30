
export default interface IResponse<T>
{
  message: string,
  success: boolean,
  data: T
}