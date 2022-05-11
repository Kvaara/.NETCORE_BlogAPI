using BlogWebAPI.Models;

namespace BlogWebAPI.Services.Models;

/// <summary>
/// Serializes Data for all service-level responses
/// which use non-paginated data
/// </summary>
/// <typeparam name="T"></typeparam>
public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public ServiceError Error { get; set; }
}

/// <summary>
/// Serializes Data for all service-based responses
/// which use paginated data
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagedServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public PaginationResult<T> Data { get; set; }
    public ServiceError Error{ get; set; }
}

/// <summary>
/// Standard error model for the Service layer
/// </summary>
public class ServiceError
{
    public string Message { get; set; }
    public string Stacktrace { get; set; }
}