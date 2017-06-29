using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoMapper.ExtendedConverters
{
    public class ListConverter<TSrc, TDest, TKey> : ITypeConverter<List<TSrc>, List<TDest>>
    {
        protected readonly Func<TSrc, TKey> SrcKey;
        protected readonly Func<TDest, TKey> DestKey;

        public ListConverter(Func<TSrc, TKey> srcKey, Func<TDest, TKey> destKey)
        {
            SrcKey = srcKey;
            DestKey = destKey;
        }

        public List<TDest> Convert(
            List<TSrc> srcList, List<TDest> destList, ResolutionContext context)
        {
            if (srcList == null) {
                return null;
            }

            IMapper mapper = context.Mapper;

            var result = new List<TDest>(srcList.Count);
            
            if (destList == null) {
                result.AddRange(srcList.Select(mapper.Map<TSrc, TDest>));
                return result;
            }

            ILookup<TKey, TDest> destLookup = destList.ToLookup(DestKey);

            foreach (TSrc src in srcList) {
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
