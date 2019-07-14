using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class User
    {
        public int? UserId { get; set; }
        public string Username { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<UserRole> Roles { get; set; }
        public bool isLoggedIn { get; set; }

    }
}
