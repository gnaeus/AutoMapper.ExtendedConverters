using System;
using System.Collections.Generic;

namespace AutoMapper.ExtendedConverters.Benchmarks.SampleClasses
{
    public class Employee : Person
    {
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public string Position { get; set; }
        public Address Address { get; set; }
        public List<Order> Orders { get; set; }

        #region Samples

        public static Employee Create()
        {
            return new Employee {
                Id = Guid.NewGuid(),
                FirstName = SampleBuilder.RandomWord(),
                LastName = SampleBuilder.RandomWord(),
                Email = SampleBuilder.RandomPhrase(2),
                Phones = SampleBuilder.RandomStringList(3, 12, digits: true),
                Address = Address.Create(),
                Position = SampleBuilder.RandomPhrase(4),
            };
        }

        #endregion
    }
}
