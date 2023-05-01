using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Users
{
    public class UserAddRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [StringLength(31, MinimumLength = 1)]     
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
        [Required]
        public string AvatarUrl { get; set; }
        [Required]
        public string TenantId { get; set; }
    }
}
