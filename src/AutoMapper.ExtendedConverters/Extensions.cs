using System;
using System.Collections.Generic;

namespace AutoMapper.ExtendedConverters
{
    public static class Extensions
    {
        public static void UsingCompiledConverter<TSrc, TDest>(
            this IMappingExpression<TSrc, TDest> mapping)
            where TSrc : class
            where TDest : class, new()
        {
            mapping.ConvertUsing(new CompiledConverter<TSrc, TDest>());
        }

        public static void UsingListConverter<TSrc, TDest, TKey>(
            this IMappingExpression<List<TSrc>, List<TDest>> mapping,
            Func<TSrc, TKey> srcKey,
            Func<TDest, TKey> destKey)
        {
            mapping.ConvertUsing(new ListConverter<TSrc, TDest, TKey>(srcKey, destKey));
        }

        public static void UsingCollectionConverter<TSrcCollection, TDestCollection, TSrc, TDest, TKey>(
            this IMappingExpression<TSrcCollection, TDestCollection> mapping,
            Func<TSrc, TKey> srcKey,
            Func<TDest, TKey> destKey)
            where TSrcCollection : class, IEnumerable<TSrc>
            where TDestCollection : class, ICollection<TDest>
        {
            mapping.ConvertUsing(new CollectionConverter<TSrcCollection, TDestCollection, TSrc, TDest, TKey>(srcKey, destKey));
        }
    }
}
