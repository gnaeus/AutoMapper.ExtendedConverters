# AutoMapper.ExtendedConverters
AutoMapper.ExtendedConverters is a small set of useful [AutoMapper](https://github.com/AutoMapper/AutoMapper) (and [Mapster](https://github.com/eswann/Mapster)) custom Type Converters

__ListConverter__ and more general __CollectionConverter__ are usefull for updating data in existing collections.

For example, we have list of entites from database and it's modified version from client.
Then with `ListConverter` we can update all database entities from client objects with same `Id`.

```cs
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

### Mapping collections other than `List<T>`
```cs
var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<IEnumerable<Model>, ICollection<Entity>>()
        .UsingCollectionConverter((Model m) => m.Id, (Entity e) => e.Id);
});   
```

## Using with [Mapster](https://github.com/eswann/Mapster)

```cs
using Mapster;
using Mapster.CollectionChangeTracking;

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
    static void Configure()
    {
        TypeAdapterConfig<List<Model>, List<Entity>>.NewConfig()
            .TrackListChanges(m => m.Id, e => e.Id);
        
        // for mapping collections other than lists
        TypeAdapterConfig<Model[], SortedSet<Entity>>.NewConfig()
            .TrackCollectionChanges((Model m) => m.Id, (Entity e) => e.Id);
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
        
        List<Entity> res = TypeAdapter.Adapt(src, dest);
        
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
