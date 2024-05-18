using System.Data;

namespace Application.Data
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetDatabaseConnection();
    }
}
