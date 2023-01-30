using Microsoft.VisualBasic;
using Services.Helpers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Providers
{
    public class ResultService: IResultService
    {
        private readonly List<Result> results = new();

        public async Task<Result> GetResultFromTaskAsync(Task task, string description, ORMType ormType, MethodType methodType)
        {
            Console.WriteLine($"Task started: {ormType.FormatString()} {methodType} - {description}...");
            Stopwatch stopwatch = Stopwatch.StartNew();
            await task;
            stopwatch.Stop();
            return new Result(description, stopwatch.Elapsed, ormType, methodType);
        }

        public void AddResults(IEnumerable<Result> results)
        {
            this.results.AddRange(results);
        }

        public void PrintResults()
        {
            var columns = (results.OrderByDescending(result => result.Description.Length).First().Description.Length + 10, 20, 20);
            Console.WriteLine("\n" + "* * * * * * * * * * RESULTS * * * * * * * * * *".Center(columns.Item1 + columns.Item2 + columns.Item3 + 2) + "\n");
            PrintLine(columns.Item1 + columns.Item2 + columns.Item3 + 2);
            PrintRow(columns, "Job", ORMType.Dapper.FormatString(), ORMType.EntityFrameWork.FormatString());
            PrintLine(columns.Item1 + columns.Item2 + columns.Item3 + 2);

            results.Skip(2)
                .GroupBy(result => result.Description)
                .OrderBy(groups => groups.First().MethodType)
                .ThenByDescending(groups => groups.First().Description)
                .ToList()
                .ForEach(group =>
            {
                var description = group.Key;
                var dapperAverage = group.Where(result => result.ORMType == ORMType.Dapper).Average(result => result.TimeStamp.TotalSeconds);
                var efAverage = group.Where(result => result.ORMType == ORMType.EntityFrameWork).Average(result => result.TimeStamp.TotalSeconds);
                PrintRow(columns, group.First().MethodType + ": " + description, String.Format("{0:F3}", dapperAverage), String.Format("{0:F3}", efAverage));
            });

            PrintLine(columns.Item1 + columns.Item2 + columns.Item3 + 2);
        }

        private void PrintRow((int, int, int) columns, string colOneContent, string colTwoContent, string colThreeContent)
        {
            string format = "|{0," + columns.Item1 + "}|{1," + columns.Item2 + "}|{2," + columns.Item3 + "}|";
            Console.WriteLine(String.Format(format, colOneContent.Center(columns.Item1), colTwoContent.Center(columns.Item2), colThreeContent.Center(columns.Item3)));
        }

        private void PrintLine(int width)
        {
            Console.WriteLine(new string('_', width));
        }

        public struct Result
        {
            public string Description { get; set; } = null!;
            public TimeSpan TimeStamp { get; set; }
            public ORMType ORMType { get; set; }
            public MethodType MethodType { get; set; }
            public Result(string description, TimeSpan timeStamp, ORMType ormType, MethodType methodType)
            {
                Description = description;
                TimeStamp = timeStamp;
                ORMType = ormType;
                MethodType = methodType;
            }
        }
    }
}
