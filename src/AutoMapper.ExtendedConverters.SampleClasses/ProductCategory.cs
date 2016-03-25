using System;
using System.Collections.Generic;

namespace AutoMapper.ExtendedConverters.SampleClasses
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
                Name = Samples.RandomPhrase(5),
                Products = Samples.RandomList(10, Product.Create)
            };
            foreach (Product product in category.Products) {
                product.ProductCategoryId = category.Id;
            }
            return category;
        }

        #endregion
    }
}
