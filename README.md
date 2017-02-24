# AutoMapper.ExtendedConverters
AutoMapper.ExtendedConverters is a small set of useful AutoMapper custom Type Converters

```cs
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using AutoMapper.ExtendedConverters;

class Model
{
    public int Id { get; set; }
    public string Text { get; set; }
}

class Entity
{
    public int Id { get; set; }
    public string Text { get; set; }
}

class Program
{
    static IMapper Mapper;
    
    static void Configure()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Model, Entity>();
            // create custom mapper for lists
            cfg.CreateMap<List<Model>, List<Entity>>()
                // configure 'Id' property selectors for Model and Entity
                .UsingListConverter(m => m.Id, e => e.Id);
        });
        
        Mapper = config.CreateMapper();
    }
    
    static void Main()
    {
        // values from Server
        var dest = new List<Entity>
        {
            new Entity { Id = 1, Text = "a" },
            new Entity { Id = 2, Text = "b" },
        };
        // values from Client
        var src = new List<Model>
        {
            new Model { Id = 1, Text = "A" },
            new Model { Id = 3, Text = "C" },
        };
        
        List<Entity> res = Mapper.Map(src, dest);
        
        // now res is equivalent to
        new List<Entity>
        {
            // Entity with Id == 1 is updated
            new Entity { Id = 1, Text = "A" },
            // Entity with Id == 2 is removed
            // ...
            // Entity with Id == 3 is added
            new Entity { Id = 3, Text = "C" },
        }
    }
}
```
