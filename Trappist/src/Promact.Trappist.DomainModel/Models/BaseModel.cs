using System;

namespace Promact.Trappist.DomainModel.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }
    }
}
