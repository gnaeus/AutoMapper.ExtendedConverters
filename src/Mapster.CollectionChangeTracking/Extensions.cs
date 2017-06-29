using System;
using System.Collections.Generic;
using System.Linq;

namespace Mapster.CollectionChangeTracking
{
    public static class Extensions
    {
        public static TypeAdapterSetter<List<TSrc>, List<TDest>> TrackListChanges<TSrc, TDest, TKey>(
            this TypeAdapterSetter<List<TSrc>, List<TDest>> setter,
            Func<TSrc, TKey> srcKey,
            Func<TDest, TKey> destKey)
        {
            return setter.MapToTargetWith((src, dest) => DetectChanges(src, dest, srcKey, destKey));
        }

        public static TypeAdapterSetter<TSrcCollection, TDestCollection> TrackCollectionChanges<TSrcCollection, TDestCollection, TSrc, TDest, TKey>(
            this TypeAdapterSetter<TSrcCollection, TDestCollection> setter,
            Func<TSrc, TKey> srcKey,
            Func<TDest, TKey> destKey)
            where TSrcCollection : class, IEnumerable<TSrc>
            where TDestCollection : class, ICollection<TDest>
        {
            return setter.MapToTargetWith((src, dest) => DetectChanges(src, dest, srcKey, destKey));
        }

        private static List<TDest> DetectChanges<TSrc, TDest, TKey>(
            List<TSrc> srcList, List<TDest> destList,
            Func<TSrc, TKey> srcKey, Func<TDest, TKey> destKey)
        {
            if (srcList == null)
            {
                return null;
            }

            var result = new List<TDest>(srcList.Count);

            if (destList == null)
            {
                result.AddRange(srcList.Select(TypeAdapter.Adapt<TSrc, TDest>));
                return result;
            }
            
            ILookup<TKey, TDest> destLookup = destList.ToLookup(destKey);

            foreach (TSrc src in srcList)
            {
                TKey key = srcKey(src);

                if (destLookup.Contains(key))
                {
                    TDest dest = destLookup[key].First();

                    TypeAdapter.Adapt(src, dest);
                    result.Add(dest);
                }
                else
                {
                    result.Add(TypeAdapter.Adapt<TSrc, TDest>(src));
                }
            }
            return result;
        }

        //private static TDestCollection DetectChanges<TSrcCollection, TDestCollection, TSrc, TDest, TKey>(
        //    TSrcCollection srcCollection, TDestCollection destCollection,
        //    Func<TSrc, TKey> srcKey, Func<TDest, TKey> destKey)
        //    where TSrcCollection : class, IEnumerable<TSrc>
        //    where TDestCollection : class, ICollection<TDest>, new()
        //{
        //    if (srcCollection == null)
        //    {
        //        return null;
        //    }

        //    var result = new TDestCollection();

        //    if (destCollection == null)
        //    {
        //        foreach (TSrc src in srcCollection)
        //        {
        //            result.Add(TypeAdapter.Adapt<TSrc, TDest>(src));
        //        }
        //        return result;
        //    }

        //    ILookup<TKey, TDest> destLookup = destCollection.ToLookup(destKey);

        //    foreach (TSrc src in srcCollection)
        //    {
        //        TKey key = srcKey(src);

        //        if (destLookup.Contains(key))
        //        {
        //            TDest dest = destLookup[key].First();

        //            TypeAdapter.Adapt(src, dest);
        //            result.Add(dest);
        //        }
        //        else
        //        {
        //            result.Add(TypeAdapter.Adapt<TSrc, TDest>(src));
        //        }
        //    }
        //    return result;
        //}

        private static TDestCollection DetectChanges<TSrcCollection, TDestCollection, TSrc, TDest, TKey>(
            TSrcCollection srcCollection, TDestCollection destCollection,
            Func<TSrc, TKey> srcKey, Func<TDest, TKey> destKey)
            where TSrcCollection : class, IEnumerable<TSrc>
            where TDestCollection : class, ICollection<TDest>
        {
            if (srcCollection == null)
            {
                return null;
            }

            TDestCollection result;

            if (destCollection == null)
            {
                result = typeof(TDestCollection) == typeof(ICollection<TDest>)
                    ? (TDestCollection)(object)new List<TDest>()
                    : Activator.CreateInstance<TDestCollection>();

                foreach (TSrc src in srcCollection)
                {
                    result.Add(TypeAdapter.Adapt<TSrc, TDest>(src));
                }
                return result;
            }
            else
            {
                result = (TDestCollection)Activator.CreateInstance(destCollection.GetType());
            }

            ILookup<TKey, TDest> destLookup = destCollection.ToLookup(destKey);

            foreach (TSrc src in srcCollection)
            {
                TKey key = srcKey(src);

                if (destLookup.Contains(key))
                {
                    TDest dest = destLookup[key].First();

                    TypeAdapter.Adapt(src, dest);
                    result.Add(dest);
                }
                else
                {
                    result.Add(TypeAdapter.Adapt<TSrc, TDest>(src));
                }
            }
            return result;
        }
    }
}
