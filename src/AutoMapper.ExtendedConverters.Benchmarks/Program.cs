using BenchmarkDotNet;

namespace AutoMapper.ExtendedConverters.Benchmarks
{
    using System.IO;
    using SampleClasses;
    using Newtonsoft.Json;

    class Program
    {
        static void Main(string[] args)
        {
            var data = Samples.DepartmentsAggregate();
            File.WriteAllText("benchmark.json", JsonConvert.SerializeObject(data, Formatting.Indented));
            File.WriteAllText("benchmark.min.json", JsonConvert.SerializeObject(data));
        }
    }
}
