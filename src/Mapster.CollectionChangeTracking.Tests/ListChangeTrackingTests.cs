using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mapster.CollectionChangeTracking.Tests
{
    [TestClass]
    public class ListChangeTrackingTests
    {
        public class Model
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        public class Entity
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }
        
        [TestInitialize]
        public void Initialize()
        {
            TypeAdapterConfig<List<Model>, List<Entity>>.NewConfig()
                .TrackListChanges(m => m.Id, e => e.Id);
        }
        
        [TestMethod]
        public void ShouldMapNullSource_WithoutDestination()
        {
            List<Entity> res = TypeAdapter.Adapt<List<Model>, List<Entity>>(null);

            Assert.IsNull(res);
        }

        [TestMethod]
        public void ShouldMapNullSource_ToExistingDestination()
        {
            var dest = new List<Entity> {
                new Entity { Id = 1, Text = "a" },
                new Entity { Id = 2, Text = "b" },
                new Entity { Id = 3, Text = "c" },
            };

            List<Entity> res = TypeAdapter.Adapt<List<Model>, List<Entity>>(null, dest);

            Assert.IsNull(res);
        }

        [TestMethod]
        public void ShouldMapSource_WithoutDestination()
        {
            var src = new List<Model> {
                new Model { Id = 1, Text = "A" },
                new Model { Id = 2, Text = "B" },
                new Model { Id = 3, Text = "C" },
                new Model { Id = 4, Text = "D" },
            };

            List<Entity> res = TypeAdapter.Adapt<List<Model>, List<Entity>>(src);

            Assert.IsNotNull(res);
            Assert.AreEqual(src.Count, res.Count);

            Assert.IsTrue(src.Select(m => m.Id).SequenceEqual(res.Select(e => e.Id)));
            Assert.IsTrue(src.Select(m => m.Text).SequenceEqual(res.Select(e => e.Text)));
        }
                
        [TestMethod]
        public void ShouldMapSource_ToExistingDestination()
        {
            var src = new List<Model> {
                new Model { Id = 1, Text = "A" },
                new Model { Id = 2, Text = "B" },
                new Model { Id = 4, Text = "D" },
                new Model { Id = 5, Text = "E" },
            };

            var dest = new List<Entity> {
                new Entity { Id = 1, Text = "a" },
                new Entity { Id = 2, Text = "b" },
                new Entity { Id = 3, Text = "c" },
            };

            List<Entity> res = TypeAdapter.Adapt(src, dest);

            Assert.IsNotNull(res);
            Assert.AreEqual(src.Count, res.Count);

            // should preserve objects with keys both in source and destination
            Assert.AreSame(dest[0], res[0]);
            Assert.AreSame(dest[1], res[1]);
            // should add objects with keys in source but not in destination
            Assert.IsNotNull(res[2]);
            Assert.IsNotNull(res[3]);
            // should remove objects with keys in destination but not in source
            Assert.AreNotSame(dest[2], res[2]);
            Assert.AreNotSame(dest[2], res[3]);

            // should preserve objects order from source
            Assert.IsTrue(src.Select(m => m.Id).SequenceEqual(res.Select(e => e.Id)));
            Assert.IsTrue(src.Select(m => m.Text).SequenceEqual(res.Select(e => e.Text)));
        }
    }
}
