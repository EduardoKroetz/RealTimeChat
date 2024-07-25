namespace RealTimeChat.Core.DTOs;

public class PagedResult : Result
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }

    public new static PagedResult SuccessResult(object data, int pageNumber, int pageSize, int totalItems ,string message = "")
    {
        return new PagedResult { Success = true, Data = data, Message = message, PageNumber = pageNumber, PageSize = pageSize, TotalItems = totalItems  };
    }
}
