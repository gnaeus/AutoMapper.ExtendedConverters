using System;

namespace AutoMapper.ExtendedConverters.SampleClasses
{
    public class Address
    {
        public Guid Id { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }

        public float Latitude;
        public float Longitude;

        #region Samples

        public static Address Create()
        {
            return new Address {
                Id = Guid.NewGuid(),
                ZipCode = Samples.RandomString(6, digits: true),
                Country = Samples.RandomWord(),
                State = Samples.RandomPhrase(2),
                City = Samples.RandomWord(),
                Street = Samples.RandomPhrase(3),
                Building = Samples.RandomString(3, digits: true),
            };
        }

        #endregion
    }
}
