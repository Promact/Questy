
using System;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Test
{
    public class TestIpAddressAC
    {
        public int Id { get; set; }

        public string IpAddress { get; set; }

        public int TestId { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

    }
}
