using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoMapper.ExtendedConverters.Tests
{
    [TestClass]
    public class CompiledConverterTests
    {
        class Props
        {
            public int Int { get; set; }
            public string String { get; set; }
            public DateTime Struct { get; set; }
            public DateTime? Nullable { get; set; }
            public Props Nested { get; set; }
            public List<string> Collection { get; set; }
            public List<Props> NestedCollection { get; set; }
        }

        class Fields
        {
            public int Int;
            public string String;
            public DateTime Struct;
            public DateTime? Nullable;
            public Fields Nested;
            public List<string> Collection;
            public List<Fields> NestedCollection;
        }

        class First
        {
            public string ShouldBeMapped { get; set; }

            public string FirstProp { get; set; }
            public string FirstField;
        }

        class Second
        {
            public string ShouldBeMapped { get; set; }
            
            public string SecondProp { get; set; }
            public string SecondField;
        }

        private IMapper Mapper;

        [TestInitialize]
        public void Initialize()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Props, Props>().UsingCompiledConverter();
                cfg.CreateMap<Props, Fields>().UsingCompiledConverter();
                cfg.CreateMap<Fields, Props>().UsingCompiledConverter();
                cfg.CreateMap<Fields, Fields>().UsingCompiledConverter();

                cfg.CreateMap<First, Second>().UsingCompiledConverter();
            });
            Mapper = config.CreateMapper();
        }

        [TestMethod]
        public void ShouldNotMap_MembersWithDifferentNames()
        {
            var first = new First {
                ShouldBeMapped = "ShouldBeMapped_First",
                FirstField = "FirstField",
                FirstProp = "FirstProp",
            };
            var second = new Second {
                ShouldBeMapped = "ShouldBeMapped_Second",
                SecondField = "SecondField",
                SecondProp = "SecondProp",
            };

            Mapper.Map(first, second);

            // should map members with same names
            Assert.AreEqual(first.ShouldBeMapped, second.ShouldBeMapped);

            // should preserve members with different names
            Assert.AreEqual("SecondField", second.SecondField);
            Assert.AreEqual("SecondProp", second.SecondProp);
        }
    }
}
