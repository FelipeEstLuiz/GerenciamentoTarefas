namespace Domain.Entities;

public class ServerSide<T>(T? data, int totalData)
{
    public T? Data { get; } = data;
    public int TotalData { get; } = totalData;
}
