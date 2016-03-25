using System;
using System.Collections.Generic;

namespace AutoMapper.ExtendedConverters.SampleClasses
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Phones { get; set; }
    }
}
