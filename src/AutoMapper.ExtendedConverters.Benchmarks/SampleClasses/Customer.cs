using System;
using System.Collections.Generic;

namespace AutoMapper.ExtendedConverters.Benchmarks.SampleClasses
{
    public class Customer : Person
    {
        public DateTime RegistrationDate { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Order> Orders { get; set; }

        #region Samples

        public static Customer Create()
        {
            return new Customer {
                Id = Guid.NewGuid(),
                FirstName = SampleBuilder.RandomWord(),
                LastName = SampleBuilder.RandomWord(),
                Email = SampleBuilder.RandomPhrase(2),
                Phones = SampleBuilder.RandomStringList(3, 12, digits: true),
                Addresses = SampleBuilder.RandomList(2, Address.Create),
                RegistrationDate = DateTime.Now,
            };
        }

        #endregion
    }
}
