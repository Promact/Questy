
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Test
{
    public class TestIpAddress:BaseModel
    {
        public string IpAddress { get; set; }

        public int TestId { get; set; }

        [ForeignKey("TestId")]
        public virtual Test Test { get; set; }
    }
}
