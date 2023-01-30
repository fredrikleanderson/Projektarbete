using Data;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Providers
{
    public abstract class DataService
    {
        protected readonly DataContext _context;
        protected readonly string _connectionString = null!;
        protected readonly IMappingService _mappingService;
        protected readonly IQueryStringService _queryStringService;

        public DataService(DataContext context, IOptions<DatabaseSettings> dbOptions, IMappingService dataHandler, IQueryStringService queryStringService)
        {
            _context = context;
            _connectionString = dbOptions.Value.ConnectionString;
            _mappingService = dataHandler;
            _queryStringService = queryStringService;
        }
    }
}
