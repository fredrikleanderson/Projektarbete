using DatabaseOptions;
using Microsoft.Extensions.Options;

namespace DapperConnection
{
    public class DapperDataService: IDapperDataService
    {
        private readonly string _connectionString = null!;

        public DapperDataService(IOptions<DatabaseSettings> settings)
        {
            _connectionString = settings.Value.ConnectionString;
        }
    }
}