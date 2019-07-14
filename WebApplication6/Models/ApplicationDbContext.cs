using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class ApplicationDbContext
    {
        public string Conn_str { get; } = "Server=tcp:mywebserver2.database.windows.net,1433;Initial Catalog=naval;Persist Security Info=False;User ID=ahrisha;Password=4/ln(lne)=INF;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        //public DbSet<Product> Products { get; set; }
        //public DbSet<Category> Categories { get; set; }
    }
}
