using System;
using System.IO;
using BenchmarkDotNet.Running;
using Newtonsoft.Json;

namespace AutoMapper.ExtendedConverters.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();

            File.WriteAllText("benchmark.json", JsonConvert.SerializeObject(Benchmark.Data, Formatting.Indented));
            File.WriteAllText("benchmark.min.json", JsonConvert.SerializeObject(Benchmark.Data));

            Console.WriteLine("You can see benchmark data in following files: benchmark.json, benchmark.min.json");
        }
    }
}
