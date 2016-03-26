using System;
using System.Collections.Generic;

namespace AutoMapper.ExtendedConverters.Benchmarks.SampleClasses
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Phones { get; set; }
        public Address Address { get; set; }
        public Employee Director { get; set; }
        public List<Employee> Salesmans { get; set; }
        
        #region Samples

        public static Department Create()
        {
            var department = new Department {
                Id = Guid.NewGuid(),
                Name = SampleBuilder.RandomPhrase(4),
                Email = SampleBuilder.RandomPhrase(2),
                Phones = SampleBuilder.RandomStringList(3, 12, digits: true),
                Address = Address.Create(),
                Director = Employee.Create(),
                Salesmans = SampleBuilder.RandomList(10, Employee.Create),
            };
            department.Director.DepartmentId = department.Id;
            foreach (Employee salesman in department.Salesmans) {
                salesman.DepartmentId = department.Id;
            }
            return department;
        }

        #endregion
    }
}
