using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoMapper.ExtendedConverters
{
    public class CompiledConverter<TSrc, TDest> : ITypeConverter<TSrc, TDest>
        where TSrc : class
        where TDest : class, new()
    {
        protected readonly bool ShouldMapFields;

        public CompiledConverter(bool shouldMapFields = false)
        {
            ShouldMapFields = shouldMapFields;
        }

        public TDest Convert(ResolutionContext context)
        {
            var src = (TSrc)context.SourceValue;
            if (src == null) {
                return null;
            }
            var dest = (TDest)context.DestinationValue ?? new TDest();
            if (this.ShouldMapFields) {
                MapFields(context.Engine.Mapper, src, dest);
            }
            MapProps(context.Engine.Mapper, src, dest);
            return dest;
        }

        private static readonly Action<IMapper, TSrc, TDest> MapProps = MapPropsExpression().Compile();
        private static readonly Action<IMapper, TSrc, TDest> MapFields = MapFieldsExpression().Compile();

        private static Expression<Action<IMapper, TSrc, TDest>> MapPropsExpression()
        {
            const BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.Instance;

            var srcProps = typeof(TSrc).GetProperties(BindingFlags).Where(p => p.CanRead);
            var destProps = typeof(TDest).GetProperties(BindingFlags).Where(p => p.CanWrite);

            var srcPrimitive = srcProps.Where(p => IsValueTypeOrString(p.PropertyType));
            var destPrimitive = destProps.Where(p => IsValueTypeOrString(p.PropertyType));

            var srcComplex = srcProps.Where(p => !IsValueTypeOrString(p.PropertyType));
            var destComplex = destProps.Where(p => !IsValueTypeOrString(p.PropertyType));

            var mapper = Expression.Parameter(typeof(IMapper), "mapper");
            var src = Expression.Parameter(typeof(TSrc), "src");
            var dest = Expression.Parameter(typeof(TDest), "dest");

            var body = srcPrimitive.Join(destPrimitive, s => s.Name, d => d.Name, (s, d) =>
                Expression.Assign(
                    Expression.Property(dest, d), Expression.Property(src, s)
                )
            ).Concat(srcComplex.Join(destComplex, s => s.Name, d => d.Name, (s, d) =>
                Expression.Assign(
                    Expression.Property(dest, d), Expression.Call(mapper, "Map",
                        new[] { s.PropertyType, d.PropertyType },
                        Expression.Property(src, s), Expression.Property(dest, d)
                    )
                )
            )).ToArray();

            return Expression.Lambda<Action<IMapper, TSrc, TDest>>(
                body.Any() ? Expression.Block(body) : (Expression)Expression.Empty(),
                mapper, src, dest
            );
        }

        private static Expression<Action<IMapper, TSrc, TDest>> MapFieldsExpression()
        {
            const BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.Instance;

            var srcFields = typeof(TSrc).GetFields(BindingFlags);
            var destFields = typeof(TDest).GetFields(BindingFlags);

            var srcPrimitive = srcFields.Where(f => IsValueTypeOrString(f.FieldType));
            var destPrimitive = destFields.Where(f => IsValueTypeOrString(f.FieldType));

            var srcComplex = srcFields.Where(f => !IsValueTypeOrString(f.FieldType));
            var destComplex = destFields.Where(f => !IsValueTypeOrString(f.FieldType));

            var mapper = Expression.Parameter(typeof(IMapper), "mapper");
            var src = Expression.Parameter(typeof(TSrc), "src");
            var dest = Expression.Parameter(typeof(TDest), "dest");

            var body = srcPrimitive.Join(destPrimitive, s => s.Name, d => d.Name, (s, d) =>
                Expression.Assign(
                    Expression.Field(dest, d), Expression.Field(src, s)
                )
            ).Concat(srcComplex.Join(destComplex, s => s.Name, d => d.Name, (s, d) =>
                Expression.Assign(
                    Expression.Field(dest, d), Expression.Call(mapper, "Map",
                        new[] { s.FieldType, d.FieldType },
                        Expression.Field(src, s), Expression.Field(dest, d)
                    )
                )
            )).ToArray();

            return Expression.Lambda<Action<IMapper, TSrc, TDest>>(
                body.Any() ? Expression.Block(body) : (Expression)Expression.Empty(),
                mapper, src, dest
            );
        }

        private static bool IsValueTypeOrString(Type type)
        {
            return type == typeof(string) || type.IsValueType;
        }
    }
}
