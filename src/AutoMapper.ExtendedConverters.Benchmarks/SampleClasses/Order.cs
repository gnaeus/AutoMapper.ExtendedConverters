using System;

namespace AutoMapper.ExtendedConverters.Benchmarks.SampleClasses
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsShipped { get; set; }
        public DateTime? ShippingDate { get; set; }
        public Address ShippingAddress { get; set; }

        public decimal Cost;

        #region Samples

        public static Order Create()
        {
            return new Order {
                Id = Guid.NewGuid(),
                Amount = SampleBuilder.Random.Next(10),
                CreationDate = DateTime.Now.Date,
                IsShipped = (SampleBuilder.Random.Next(2) == 1),
                ShippingDate = (SampleBuilder.Random.Next(2) == 1)
                    ? DateTime.Now.Date.AddDays(1)
                    : (DateTime?)null,
                ShippingAddress = Address.Create(),
            };
        }

        #endregion
    }
}
