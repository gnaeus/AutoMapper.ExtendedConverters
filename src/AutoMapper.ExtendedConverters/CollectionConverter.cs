using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoMapper.ExtendedConverters
{
    public class CollectionConverter<TSrcCollection, TDestCollection, TSrc, TDest, TKey>
        : ITypeConverter<TSrcCollection, TDestCollection>
        where TSrcCollection : class, IEnumerable<TSrc>
        where TDestCollection : class, ICollection<TDest>
    {
        protected readonly Func<TSrc, TKey> SrcKey;
        protected readonly Func<TDest, TKey> DestKey;

        public CollectionConverter(Func<TSrc, TKey> srcKey, Func<TDest, TKey> destKey)
        {
            SrcKey = srcKey;
            DestKey = destKey;
        }

        public TDestCollection Convert(
            TSrcCollection srcCollection, TDestCollection destCollection, ResolutionContext context)
        {
            if (srcCollection == null) {
                return null;
            }

            IMapper mapper = context.Mapper;

            TDestCollection result;

            if (destCollection == null) {
                result = typeof(TDestCollection) == typeof(ICollection<TDest>)
                    ? (TDestCollection)(object)new List<TDest>()
                    : Activator.CreateInstance<TDestCollection>();

                foreach (TSrc src in srcCollection) {
                    result.Add(mapper.Map<TSrc, TDest>(src));
                }
                return result;
            } else {
                result = (TDestCollection)Activator.CreateInstance(destCollection.GetType());
            }

            ILookup<TKey, TDest> destLookup = destCollection.ToLookup(DestKey);

            foreach (TSrc src in srcCollection) {
                TKey key = SrcKey(src);
                
                if (destLookup.Contains(key)) {
                    TDest dest = destLookup[key].First();

                    mapper.Map(src, dest);
                    result.Add(dest);
                } else {
                    result.Add(mapper.Map<TSrc, TDest>(src));    
                }
            }
            return result;
        }
    }
}