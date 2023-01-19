using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Helpers;
using Services.Providers;
using static Services.Providers.ResultService;

namespace Services.Interfaces
{
    public interface IResultService
    {
        Task<Result> GetResultFromTaskAsync(Task task, string information, ORMType ormType, MethodType methodType);
        void AddResults(IEnumerable<Result> results);
        void PrintResults();

    }
}
