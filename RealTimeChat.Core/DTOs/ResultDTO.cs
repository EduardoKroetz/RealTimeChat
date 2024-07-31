

namespace RealTimeChat.Core.DTOs;


public class ResultDTO
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }

    public static ResultDTO SuccessResult(object data, string message = "")
    {
        return new ResultDTO { Success = true, Data = data, Message = message };
    }

    public static ResultDTO FailureResult(string message)
    {
        return new ResultDTO { Success = false, Message = message };
    }
}


