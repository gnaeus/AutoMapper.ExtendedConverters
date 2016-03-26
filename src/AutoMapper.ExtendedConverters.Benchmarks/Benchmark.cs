using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace AutoMapper.ExtendedConverters.Benchmarks
{
    using SampleClasses;

    public class Benchmark
    {
        public static readonly object Data = SampleBuilder.DepartmentsAggregate();

        private readonly IMapper AutoMapper;
        private readonly IMapper AutoMapperCompiledConverter;
        private readonly IMapper AutoMapperCompiledAndListConverter;

        public Benchmark()
        {
            MapperConfiguration config;

            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Address, Address>();
                cfg.CreateMap<Customer, Customer>();
                cfg.CreateMap<Department, Department>();
                cfg.CreateMap<Employee, Employee>();
                cfg.CreateMap<Order, Order>();
                cfg.CreateMap<Person, Person>();
                cfg.CreateMap<Product, Product>();
                cfg.CreateMap<ProductCategory, ProductCategory>();
            });
            config.AssertConfigurationIsValid();
            AutoMapper = config.CreateMapper();

            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Address, Address>().UsingCompiledConverter();
                cfg.CreateMap<Customer, Customer>().UsingCompiledConverter();
                cfg.CreateMap<Department, Department>().UsingCompiledConverter();
                cfg.CreateMap<Employee, Employee>().UsingCompiledConverter();
                cfg.CreateMap<Order, Order>().UsingCompiledConverter();
                cfg.CreateMap<Person, Person>().UsingCompiledConverter();
                cfg.CreateMap<Product, Product>().UsingCompiledConverter();
                cfg.CreateMap<ProductCategory, ProductCategory>().UsingCompiledConverter();
            });
            config.AssertConfigurationIsValid();
            AutoMapperCompiledConverter = config.CreateMapper();

            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Address, Address>().UsingCompiledConverter();
                cfg.CreateMap<Customer, Customer>().UsingCompiledConverter();
                cfg.CreateMap<Department, Department>().UsingCompiledConverter();
                cfg.CreateMap<Employee, Employee>().UsingCompiledConverter();
                cfg.CreateMap<Order, Order>().UsingCompiledConverter();
                cfg.CreateMap<Product, Product>().UsingCompiledConverter();
                cfg.CreateMap<ProductCategory, ProductCategory>().UsingCompiledConverter();

                cfg.CreateMap<List<Address>, List<Address>>().UsingListConverter(a => a.Id, a => a.Id);
                cfg.CreateMap<List<Customer>, List<Customer>>().UsingListConverter(c => c.Id, c => c.Id);
                cfg.CreateMap<List<Department>, List<Department>>().UsingListConverter(d => d.Id, d => d.Id);
                cfg.CreateMap<List<Employee>, List<Employee>>().UsingListConverter(e => e.Id, e => e.Id);
                cfg.CreateMap<List<Order>, List<Order>>().UsingListConverter(o => o.Id, o => o.Id);
                cfg.CreateMap<List<Product>, List<Product>>().UsingListConverter(p => p.Id, a => a.Id);
                cfg.CreateMap<List<ProductCategory>, List<ProductCategory>>().UsingListConverter(pc => pc.Id, pc => pc.Id);
            });
            config.AssertConfigurationIsValid();
            AutoMapperCompiledAndListConverter = config.CreateMapper();
        }

        [Benchmark(Description = "Building sample data")]
        public object BuildSampleDataTest()
        {
            return SampleBuilder.DepartmentsAggregate();
        }

        [Benchmark(Description = "Vanilla AutoMapper")]
        public object AutoMapperTest()
        {
            return AutoMapper.Map<List<Department>>(Data);
        }

        [Benchmark(Description = "AutoMapper + CompiledConverter")]
        public object AutoMapperCompiledConverterTest()
        {
            return AutoMapperCompiledConverter.Map<List<Department>>(Data);
        }

        [Benchmark(Description = "AutoMapper + CompiledConverter + ListConverter")]
        public object AutoMapperCompiledAndListConverterTest()
        {
            return AutoMapperCompiledAndListConverter.Map<List<Department>>(Data);
        }

        [Benchmark(Description = "Serialization of sample data by Json.NET")]
        public object NewtonsoftJsonSerializationTest()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(Data);
        }

        [Benchmark(Description = "Serialization of sample data by Jil serializer")]
        public object JilSerializationTest()
        {
            return Jil.JSON.Serialize(Data);
        }

        [Benchmark(Description = "Serialization and deserialization of sample data by Json.NET")]
        public object NewtonsoftJsonCloningTest()
        {
            string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(Data);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Department>>(serialized);
        }

        [Benchmark(Description = "Serialization and deserialization of sample data by Jil serializer")]
        public object JilCloningTest()
        {
            string serialized = Jil.JSON.Serialize(Data);
            return Jil.JSON.Deserialize<List<Department>>(serialized);
        }
    }
}
