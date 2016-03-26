using System;

namespace AutoMapper.ExtendedConverters.Benchmarks.SampleClasses
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
                ZipCode = SampleBuilder.RandomString(6, digits: true),
                Country = SampleBuilder.RandomWord(),
                State = SampleBuilder.RandomPhrase(2),
                City = SampleBuilder.RandomWord(),
                Street = SampleBuilder.RandomPhrase(3),
                Building = SampleBuilder.RandomString(3, digits: true),
            };
        }

        #endregion
    }
}
