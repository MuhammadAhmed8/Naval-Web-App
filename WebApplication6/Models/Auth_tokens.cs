using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class Auth_tokens
    {
        public string Validator { get; set; }
        public string Selector { get; set; }
        public int? UserId { get; set; }
        public DateTime? Expires { get; set; }

    }
}
