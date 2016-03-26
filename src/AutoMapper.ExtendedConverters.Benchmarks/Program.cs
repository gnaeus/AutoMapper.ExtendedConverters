using System.IO;
using BenchmarkDotNet;
using Newtonsoft.Json;

namespace AutoMapper.ExtendedConverters.Benchmarks
{
    using SampleClasses;
    
    class Program
    {
        static void Main(string[] args)
        {
            var data = SampleBuilder.DepartmentsAggregate();
            File.WriteAllText("benchmark.json", JsonConvert.SerializeObject(data, Formatting.Indented));
            File.WriteAllText("benchmark.min.json", JsonConvert.SerializeObject(data));
        }
    }
}
