using System.Data;

namespace API
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetDatabaseConnection();
    }
}
