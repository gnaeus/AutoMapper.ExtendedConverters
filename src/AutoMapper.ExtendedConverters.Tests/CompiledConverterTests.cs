using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            public Guid Struct { get; set; }
            public DateTime? Nullable { get; set; }
            public List<string> Collection { get; set; }

            public Props Nested { get; set; }
            public List<Props> NestedCollection { get; set; }
        }

        class Fields
        {
            public int Int;
            public string String;
            public Guid Struct;
            public DateTime? Nullable;
            public List<string> Collection;

            public Fields Nested;
            public List<Fields> NestedCollection;
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
        public void ShouldMap_PropsToProps()
        {
            var src = new Props {
                Int = 10,
                String = "test",
                Struct = Guid.NewGuid(),
                Nullable = DateTime.Now,
            };

            Props dest = Mapper.Map<Props>(src);

            Assert.IsNotNull(dest);
            Assert.AreNotSame(src, dest);

            Assert.AreEqual(src.Int, dest.Int);
            Assert.AreEqual(src.String, dest.String);
            Assert.AreEqual(src.Struct, dest.Struct);
            Assert.AreEqual(src.Nullable, dest.Nullable);
        }

        [TestMethod]
        public void ShouldMap_CollectionProps()
        {
            var src = new Props {
                Collection = new List<string> { "one", "two", "three" },
            };

            Props dest = Mapper.Map<Props>(src);

            Assert.IsNotNull(dest);
            Assert.AreNotSame(src, dest);
            Assert.IsNotNull(dest.Collection);
            Assert.AreNotSame(src.Collection, dest.Collection);

            Assert.IsTrue(src.Collection.SequenceEqual(dest.Collection));
        }

        [TestMethod]
        public void ShouldMap_NestedProps()
        {
            var src = new Props {
                Nested = new Props {
                    Int = 20,
                    String = "nested",
                    Struct = Guid.NewGuid(),
                    Nullable = DateTime.Now,
                },
            };

            Props dest = Mapper.Map<Props>(src);

            Assert.IsNotNull(dest);
            Assert.AreNotSame(src, dest);
            Assert.IsNotNull(dest.Nested);
            Assert.AreNotSame(src.Nested, dest.Nested);

            Assert.AreEqual(src.Nested.Int, dest.Nested.Int);
            Assert.AreEqual(src.Nested.String, dest.Nested.String);
            Assert.AreEqual(src.Nested.Struct, dest.Nested.Struct);
            Assert.AreEqual(src.Nested.Nullable, dest.Nested.Nullable);
        }

        [TestMethod]
        public void ShouldMap_NestedCollectionProps()
        {
            var src = new Props {
                NestedCollection = new List<Props> {
                    new Props { Int = 10 },
                    new Props { Int = 20 },
                    new Props { Int = 30 },
                },
            };

            Props dest = Mapper.Map<Props>(src);

            Assert.IsNotNull(dest);
            Assert.AreNotSame(src, dest);
            Assert.IsNotNull(dest.NestedCollection);
            Assert.AreNotSame(src.NestedCollection, dest.NestedCollection);

            Assert.IsTrue(
                src.NestedCollection.Select(s => s.Int)
                    .SequenceEqual(dest.NestedCollection.Select(d => d.Int))
            );
        }

        [TestMethod]
        public void ShouldMap_PropsToFields()
        {
            var src = new Props {
                Int = 10,
                String = "test",
                Struct = Guid.NewGuid(),
                Nullable = DateTime.Now,
            };

            Fields dest = Mapper.Map<Fields>(src);

            Assert.IsNotNull(dest);
            Assert.AreNotSame(src, dest);

            Assert.AreEqual(src.Int, dest.Int);
            Assert.AreEqual(src.String, dest.String);
            Assert.AreEqual(src.Struct, dest.Struct);
            Assert.AreEqual(src.Nullable, dest.Nullable);
        }

        [TestMethod]
        public void ShouldMap_FieldsToProps()
        {
            var src = new Fields {
                Int = 10,
                String = "test",
                Struct = Guid.NewGuid(),
                Nullable = DateTime.Now,
            };

            Props dest = Mapper.Map<Props>(src);

            Assert.IsNotNull(dest);
            Assert.AreNotSame(src, dest);

            Assert.AreEqual(src.Int, dest.Int);
            Assert.AreEqual(src.String, dest.String);
            Assert.AreEqual(src.Struct, dest.Struct);
            Assert.AreEqual(src.Nullable, dest.Nullable);
        }
        
        [TestMethod]
        public void ShouldMap_FieldsToFields()
        {
            var src = new Fields {
                Int = 10,
                String = "test",
                Struct = Guid.NewGuid(),
                Nullable = DateTime.Now,
            };

            Fields dest = Mapper.Map<Fields>(src);

            Assert.IsNotNull(dest);
            Assert.AreNotSame(src, dest);

            Assert.AreEqual(src.Int, dest.Int);
            Assert.AreEqual(src.String, dest.String);
            Assert.AreEqual(src.Struct, dest.Struct);
            Assert.AreEqual(src.Nullable, dest.Nullable);
        }

        [TestMethod]
        public void ShouldMap_NestedPropsToFields()
        {
            var src = new Props {
                Nested = new Props {
                    Int = 20,
                    String = "nested",
                    Struct = Guid.NewGuid(),
                    Nullable = DateTime.Now,
                },
            };

            Fields dest = Mapper.Map<Fields>(src);

            Assert.IsNotNull(dest);
            Assert.AreNotSame(src, dest);
            Assert.IsNotNull(dest.Nested);
            Assert.AreNotSame(src.Nested, dest.Nested);

            Assert.AreEqual(src.Nested.Int, dest.Nested.Int);
            Assert.AreEqual(src.Nested.String, dest.Nested.String);
            Assert.AreEqual(src.Nested.Struct, dest.Nested.Struct);
            Assert.AreEqual(src.Nested.Nullable, dest.Nested.Nullable);
        }

        [TestMethod]
        public void ShouldMap_NestedCollectionPropsToFields()
        {
            var src = new Props {
                NestedCollection = new List<Props> {
                    new Props { Int = 10 },
                    new Props { Int = 20 },
                    new Props { Int = 30 },
                },
            };

            Fields dest = Mapper.Map<Fields>(src);

            Assert.IsNotNull(dest);
            Assert.AreNotSame(src, dest);
            Assert.IsNotNull(dest.NestedCollection);
            Assert.AreNotSame(src.NestedCollection, dest.NestedCollection);

            Assert.IsTrue(
                src.NestedCollection.Select(s => s.Int)
                    .SequenceEqual(dest.NestedCollection.Select(d => d.Int))
            );
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
