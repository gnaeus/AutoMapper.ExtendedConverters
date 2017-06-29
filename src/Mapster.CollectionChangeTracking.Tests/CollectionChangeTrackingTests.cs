using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mapster.CollectionChangeTracking.Tests
{
    [TestClass]
    public class CollectionChangeTrackingTests
    {
        public class Model
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        public class Entity : IComparable<Entity>
        {
            public int Id { get; set; }
            public string Text { get; set; }

            public int CompareTo(Entity other)
            {
                return Id.CompareTo(other.Id);
            }
        }
        
        [TestInitialize]
        public void Initialize()
        {
            TypeAdapterConfig<Model[], SortedSet<Entity>>.NewConfig()
                .TrackCollectionChanges((Model m) => m.Id, (Entity e) => e.Id);

            TypeAdapterConfig<IEnumerable<Model>, LinkedList<Entity>>.NewConfig()
                .TrackCollectionChanges((Model m) => m.Id, (Entity e) => e.Id);

            //TypeAdapterConfig<IEnumerable<Model>, ICollection<Entity>>.NewConfig()
            //    .TrackCollectionChanges((Model m) => m.Id, (Entity e) => e.Id);
        }

        [TestMethod]
        public void ShouldMapNullSource_WithoutDestination()
        {
            SortedSet<Entity> res = TypeAdapter.Adapt<Model[], SortedSet<Entity>>(null);

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

            SortedSet<Entity> res = TypeAdapter.Adapt<Model[], SortedSet<Entity>>(null, dest);

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

            SortedSet<Entity> res = TypeAdapter.Adapt<Model[], SortedSet<Entity>>(src);

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

            SortedSet<Entity> res = TypeAdapter.Adapt(src, dest);

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

        public class ModelWrapper
        {
            public IEnumerable<Model> Collection { get; set; }
        }

        public class EntityWrapper
        {
            public ICollection<Entity> Collection { get; set; }
        }

        [TestMethod]
        public void ShouldMapAnyIEnumerable_ToAnyICollection()
        {
            var dest = new LinkedList<Entity>(new[] {
                new Entity { Id = 1, Text = "a" },
                new Entity { Id = 2, Text = "b" },
                new Entity { Id = 3, Text = "c" },
            });
            
            var destArray = new Entity[3];
            dest.CopyTo(destArray, 0);

            var destWrapper = new EntityWrapper { Collection = dest };
            var srcWrapper = new ModelWrapper { Collection = BuildSource() };

            var res = TypeAdapter.Adapt(srcWrapper, destWrapper);

            var resArray = new Entity[4];
            res.Collection.CopyTo(resArray, 0);

            // should preserve collection type
            Assert.AreEqual(dest.GetType(), res.Collection.GetType());
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
            Assert.IsTrue(BuildSource().Select(m => m.Id).SequenceEqual(res.Collection.Select(e => e.Id)));
            Assert.IsTrue(BuildSource().Select(m => m.Text).SequenceEqual(res.Collection.Select(e => e.Text)));
        }
    }
}
