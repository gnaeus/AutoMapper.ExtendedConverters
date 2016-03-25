using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper.ExtendedConverters.SampleClasses;

namespace AutoMapper.ExtendedConverters.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void TestConfigurationExtensions()
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<Customer, Customer>().UsingCompiledConverter();

                cfg.CreateMap<List<Customer>, List<Customer>>()
                    .UsingListConverter(s => s.Id, d => d.Id);

                cfg.CreateMap<IEnumerable<Customer>, List<Customer>>()
                    .UsingCollectionConverter((Customer s) => s.Id, (Customer d) => d.Id);
            });

            config.AssertConfigurationIsValid();
        }
    }
}
