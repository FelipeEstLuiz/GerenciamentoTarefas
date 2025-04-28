namespace Application.DTOs;

public class ServerSideDto<T>
{
    public T? Data { get; set; }
    public int TotalData { get; set; }
    public int TotalPages { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
}
