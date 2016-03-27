using System.Collections.Generic;
using System.Linq;

namespace AutoMapper.ExtendedConverters.Benchmarks
{
    using SampleClasses;

    public class ManualMapper
    {
        public Address Map(Address src, Address dest = null)
        {
            if (src == null) { return null; }
            if (dest == null) { dest = new Address(); }
            dest.Id = src.Id;
            dest.ZipCode = src.ZipCode;
            dest.Country = src.Country;
            dest.State = src.State;
            dest.City = src.City;
            dest.Street = src.Street;
            dest.Building = src.Building;
            dest.Latitude = src.Latitude;
            dest.Longitude = src.Longitude;
            return dest;
        }

        public Customer Map(Customer src, Customer dest = null)
        {
            if (src == null) { return null; }
            if (dest == null) { dest = new Customer(); }
            dest.Id = src.Id;
            dest.FirstName = src.FirstName;
            dest.LastName = src.LastName;
            dest.Email = src.Email;
            dest.Phones = Map(src.Phones);
            dest.RegistrationDate = src.RegistrationDate;
            dest.Addresses = Map(src.Addresses);
            dest.Orders = Map(src.Orders);
            return dest;
        }

        public Department Map(Department src, Department dest = null)
        {
            if (src == null) { return null; }
            if (dest == null) { dest = new Department(); }
            dest.Id = src.Id;
            dest.Name = src.Name;
            dest.Email = src.Email;
            dest.Phones = Map(src.Phones);
            dest.Address = Map(src.Address);
            dest.Director = Map(src.Director);
            dest.Salesmans = Map(src.Salesmans);
            return dest;
        }

        public Employee Map(Employee src, Employee dest = null)
        {
            if (src == null) { return null; }
            if (dest == null) { dest = new Employee(); }
            dest.Id = src.Id;
            dest.FirstName = src.FirstName;
            dest.LastName = src.LastName;
            dest.Email = src.Email;
            dest.Phones = Map(src.Phones);
            dest.DepartmentId = src.DepartmentId;
            dest.Department = Map(src.Department);
            dest.Position = src.Position;
            dest.Address = Map(src.Address);
            dest.Orders = Map(src.Orders);
            return dest;
        }

        public Order Map(Order src, Order dest = null)
        {
            if (src == null) { return null; }
            if (dest == null) { dest = new Order(); }
            dest.Id = src.Id;
            dest.CustomerId = src.CustomerId;
            dest.Customer = Map(src.Customer);
            dest.EmployeeId = src.EmployeeId;
            dest.Employee = Map(src.Employee);
            dest.ProductId = src.ProductId;
            dest.Product = Map(src.Product);
            dest.Amount = src.Amount;
            dest.CreationDate = src.CreationDate;
            dest.IsShipped = src.IsShipped;
            dest.ShippingDate = src.ShippingDate;
            dest.ShippingAddress = Map(src.ShippingAddress);
            return dest;
        }

        public Product Map(Product src, Product dest = null)
        {
            if (src == null) { return null; }
            if (dest == null) { dest = new Product(); }
            dest.Id = src.Id;
            dest.ProductCategoryId = src.ProductCategoryId;
            dest.ProductCategory = Map(src.ProductCategory);
            dest.Name = src.Name;
            dest.ShortDescription = src.ShortDescription;
            dest.Description = src.Description;
            dest.Location = src.Location;
            dest.Weight = src.Weight;
            dest.Price = src.Price;
            dest.Count = src.Count;
            dest.Rating = src.Rating;
            return dest;
        }

        public ProductCategory Map(ProductCategory src, ProductCategory dest = null)
        {
            if (src == null) { return null; }
            if (dest == null) { dest = new ProductCategory(); }
            dest.Id = src.Id;
            dest.Name = src.Name;
            dest.Products = src.Products.Select(x => Map(x)).ToList();
            return dest;
        }

        public List<string> Map(List<string> src)
        {
            return src == null ? null : src.ToList();
        }

        public List<T> Map<T>(List<T> src)
            where T : struct
        {
            return src == null ? null : src.ToList();
        }

        public List<Address> Map(List<Address> src)
        {
            return src == null ? null : src.Select(x => Map(x)).ToList();
        }

        public List<Customer> Map(List<Customer> src)
        {
            return src == null ? null : src.Select(x => Map(x)).ToList();
        }

        public List<Department> Map(List<Department> src)
        {
            return src == null ? null : src.Select(x => Map(x)).ToList();
        }

        public List<Employee> Map(List<Employee> src)
        {
            return src == null ? null : src.Select(x => Map(x)).ToList();
        }

        public List<Order> Map(List<Order> src)
        {
            return src == null ? null : src.Select(x => Map(x)).ToList();
        }

        public List<Product> Map(List<Product> src)
        {
            return src == null ? null : src.Select(x => Map(x)).ToList();
        }

        public List<ProductCategory> Map(List<ProductCategory> src)
        {
            return src == null ? null : src.Select(x => Map(x)).ToList();
        }
    }
}
