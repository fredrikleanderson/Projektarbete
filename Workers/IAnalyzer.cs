using Services.Interfaces;

namespace Workers
{
    public interface IAnalyzer
    {

    }

    public class Analyzer: IAnalyzer
    {
        private readonly IDataService _dapperService;
    }
}