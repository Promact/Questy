using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.ApplicationClasses
{
    public class EmailResponse
    {
        public bool IsMailSent { get; set; }
        public string ErrorMessage { get; set; }
    }
}
