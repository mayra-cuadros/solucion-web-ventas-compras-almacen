using Microsoft.Data.SqlClient;

namespace proyectoWEBSITESmeall.Infrastructure
{
    public interface IDbConnectionFactory
    {
        SqlConnection Create();
    }

    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _cs;
        public SqlConnectionFactory(IConfiguration cfg)
            => _cs = cfg.GetConnectionString("bbddSmeallConn")!;

        public SqlConnection Create() => new SqlConnection(_cs);
    }
}
