using System;

namespace Blobzor.Core.Model.Domain
{
    public class BusinessObject : IEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public string City { get; set; }
    }
}