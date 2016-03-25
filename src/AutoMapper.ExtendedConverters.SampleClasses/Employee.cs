using System;
using System.Collections.Generic;

namespace AutoMapper.ExtendedConverters.SampleClasses
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
                FirstName = Samples.RandomWord(),
                LastName = Samples.RandomWord(),
                Email = Samples.RandomPhrase(2),
                Phones = Samples.RandomStringList(3, 12, digits: true),
                Address = Address.Create(),
                Position = Samples.RandomPhrase(4),
            };
        }

        #endregion
    }
}
