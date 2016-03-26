using System;

namespace AutoMapper.ExtendedConverters.Benchmarks.SampleClasses
{
    public class Product
    {
        public Guid Id { get; set; }
        public Guid ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public float Weight { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public float Rating;

        #region Samples

        public static Product Create()
        {
            return new Product {
                Id = Guid.NewGuid(),
                Name = SampleBuilder.RandomPhrase(2),
                ShortDescription = SampleBuilder.RandomPhrase(5),
                Description = SampleBuilder.RandomPhrase(15),
                Location = SampleBuilder.RandomPhrase(8),
                Weight = 10 * (float)SampleBuilder.Random.NextDouble(),
                Price = 100 * (decimal)SampleBuilder.Random.NextDouble(),
                Count = SampleBuilder.Random.Next(100),
            };
        }

        #endregion
    }
}
