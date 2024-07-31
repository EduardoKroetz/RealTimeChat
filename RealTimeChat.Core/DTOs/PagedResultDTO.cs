namespace RealTimeChat.Core.DTOs;

public class PagedResultDTO : ResultDTO
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }

    public new static PagedResultDTO SuccessResult(object data, int pageNumber, int pageSize, int totalItems ,string message = "")
    {
        return new PagedResultDTO { Success = true, Data = data, Message = message, PageNumber = pageNumber, PageSize = pageSize, TotalItems = totalItems  };
    }
}
