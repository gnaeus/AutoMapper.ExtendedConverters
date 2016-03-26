using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoMapper.ExtendedConverters.Tests
{
    [TestClass]
    public class CollectionConverterTests
    {
        class Model
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        class Entity : IComparable<Entity>
        {
            public int Id { get; set; }
            public string Text { get; set; }

            public int CompareTo(Entity other)
            {
                return Id.CompareTo(other.Id);
            }
        }

        private IMapper Mapper;

        [TestInitialize]
        public void Initialize()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Model, Entity>();

                cfg.CreateMap<Model[], SortedSet<Entity>>()
                    .UsingCollectionConverter((Model m) => m.Id, (Entity e) => e.Id);

                cfg.CreateMap<IEnumerable<Model>, LinkedList<Entity>>()
                    .UsingCollectionConverter((Model m) => m.Id, (Entity e) => e.Id);
            });
            Mapper = config.CreateMapper();
        }

        [TestMethod]
        public void ShouldMapNullSource_WithoutDestination()
        {
            SortedSet<Entity> res = Mapper.Map<SortedSet<Entity>>(null);

            Assert.IsNull(res);
        }

        [TestMethod]
        public void ShouldMapNullSource_ToExistingDestination()
        {
            var dest = new SortedSet<Entity> {
                new Entity { Id = 1, Text = "a" },
                new Entity { Id = 2, Text = "b" },
                new Entity { Id = 3, Text = "c" },
            };

            SortedSet<Entity> res = Mapper.Map((Model[])null, dest);

            Assert.IsNull(res);
        }

        [TestMethod]
        public void ShouldMapSource_WithoutDestination()
        {
            var src = new[] {
                new Model { Id = 1, Text = "A" },
                new Model { Id = 2, Text = "B" },
                new Model { Id = 3, Text = "C" },
                new Model { Id = 4, Text = "D" },
            };

            SortedSet<Entity> res = Mapper.Map<SortedSet<Entity>>(src);

            Assert.IsNotNull(res);
            Assert.AreEqual(src.Length, res.Count);

            Assert.IsTrue(src.Select(m => m.Id).SequenceEqual(res.Select(e => e.Id)));
            Assert.IsTrue(src.Select(m => m.Text).SequenceEqual(res.Select(e => e.Text)));
        }

        [TestMethod]
        public void ShouldMapSource_ToExistingDestination()
        {
            var src = new[] {
                new Model { Id = 1, Text = "A" },
                new Model { Id = 2, Text = "B" },
                new Model { Id = 4, Text = "D" },
                new Model { Id = 5, Text = "E" },
            };

            var dest = new SortedSet<Entity> {
                new Entity { Id = 1, Text = "a" },
                new Entity { Id = 2, Text = "b" },
                new Entity { Id = 3, Text = "c" },
            };

            var destArray = new Entity[3];
            dest.CopyTo(destArray, 0);

            SortedSet<Entity> res = Mapper.Map(src, dest);

            var resArray = new Entity[4];
            res.CopyTo(resArray, 0);

            Assert.IsNotNull(res);
            Assert.AreEqual(src.Length, res.Count);

            // should preserve objects with keys both in source and destination
            Assert.AreSame(destArray[0], resArray[0]);
            Assert.AreSame(destArray[1], resArray[1]);
            // should add objects with keys in source but not in destination
            Assert.IsNotNull(resArray[2]);
            Assert.IsNotNull(resArray[3]);
            // should remove objects with keys in destination but not in source
            Assert.AreNotSame(destArray[2], resArray[2]);
            Assert.AreNotSame(destArray[2], resArray[3]);
            
            // should map values of collection items
            Assert.IsTrue(src.OrderBy(m => m.Id).Select(m => m.Id).SequenceEqual(res.Select(e => e.Id)));
            Assert.IsTrue(src.OrderBy(m => m.Id).Select(m => m.Text).SequenceEqual(res.Select(e => e.Text)));
        }

        private static IEnumerable<Model> BuildSource()
        {
            yield return new Model { Id = 1, Text = "A" };
            yield return new Model { Id = 2, Text = "B" };
            yield return new Model { Id = 4, Text = "D" };
            yield return new Model { Id = 5, Text = "E" };
        }

        [TestMethod]
        public void ShouldMapAnyIEnumerable_ToAnyICollection()
        {
            ICollection<Entity> dest = new LinkedList<Entity>(new[] {
                new Entity { Id = 1, Text = "a" },
                new Entity { Id = 2, Text = "b" },
                new Entity { Id = 3, Text = "c" },
            });
            
            var destArray = new Entity[3];
            dest.CopyTo(destArray, 0);

            ICollection<Entity> res = Mapper.Map(BuildSource(), dest);

            var resArray = new Entity[4];
            res.CopyTo(resArray, 0);

            // should preserve objects with keys both in source and destination
            Assert.AreSame(destArray[0], resArray[0]);
            Assert.AreSame(destArray[1], resArray[1]);
            // should add objects with keys in source but not in destination
            Assert.IsNotNull(resArray[2]);
            Assert.IsNotNull(resArray[3]);
            // should remove objects with keys in destination but not in source
            Assert.AreNotSame(destArray[2], resArray[2]);
            Assert.AreNotSame(destArray[2], resArray[3]);

            // should map values of collection items
            Assert.IsTrue(BuildSource().Select(m => m.Id).SequenceEqual(res.Select(e => e.Id)));
            Assert.IsTrue(BuildSource().Select(m => m.Text).SequenceEqual(res.Select(e => e.Text)));
        }
    }
}
