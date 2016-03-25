﻿using System;
using System.Collections.Generic;

namespace AutoMapper.ExtendedConverters.SampleClasses
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
                FirstName = Samples.RandomWord(),
                LastName = Samples.RandomWord(),
                Email = Samples.RandomPhrase(2),
                Phones = Samples.RandomStringList(3, 12, digits: true),
                Addresses = Samples.RandomList(2, Address.Create),
                RegistrationDate = DateTime.Now,
            };
        }

        #endregion
    }
}
