namespace RealTimeChat.IntegrationTests.Utils;


public record Result<T>(bool success, string message, T data);
public record DataToken(string token);
public record DataRegisterUser(string token ,Guid id);
public record DataId(Guid id);
public record PagedResult<T>(bool success, string message, T data, int pageSize, int pageNumber, int totalItems);