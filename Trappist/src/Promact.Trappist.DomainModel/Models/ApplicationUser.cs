using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Promact.Trappist.DomainModel.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string OrganizationName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDateTime { get; set; }
    }
}
