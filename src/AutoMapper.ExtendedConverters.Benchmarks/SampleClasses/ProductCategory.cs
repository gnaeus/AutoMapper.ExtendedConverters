using System;
using System.Collections.Generic;

namespace AutoMapper.ExtendedConverters.Benchmarks.SampleClasses
{
    public class ProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }

        #region Samples

        public static ProductCategory Create()
        {
            var category = new ProductCategory {
                Id = Guid.NewGuid(),
                Name = SampleBuilder.RandomPhrase(5),
                Products = SampleBuilder.RandomList(10, Product.Create)
            };
            foreach (Product product in category.Products) {
                product.ProductCategoryId = category.Id;
            }
            return category;
        }

        #endregion
    }
}
