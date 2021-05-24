using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UntoldApp.Models
{
    public class ApplicationUserModel
    {
        [Key]
        [Column(TypeName = "nvarchar(450)")]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        public string Role { get; set; }
    }
}
