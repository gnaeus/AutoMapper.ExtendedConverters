using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoMapper.ExtendedConverters.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        class Model
        {
            public int Id { get; set; }
        }

        class Entity
        {
            public int Id { get; set; }
        }

        [TestMethod]
        public void ShouldProduceValidConfiguration()
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<Model, Entity>().UsingCompiledConverter();

                cfg.CreateMap<List<Model>, List<Entity>>()
                    .UsingListConverter(m => m.Id, e => e.Id);

                cfg.CreateMap<IEnumerable<Model>, List<Entity>>()
                    .UsingCollectionConverter((Model m) => m.Id, (Entity e) => e.Id);
            });

            config.AssertConfigurationIsValid();
        }
    }
}
