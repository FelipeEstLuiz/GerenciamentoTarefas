using System.Data;

namespace Domain.Repositories;
public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
