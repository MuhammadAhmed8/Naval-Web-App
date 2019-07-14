using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        public ApplicationDbContext context = new ApplicationDbContext();
        public IEnumerable<Category> GetCategories()
        {
            List<Category> Categories = new List<Category>();
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                SqlCommand sqlcmd = new SqlCommand("SELECT * FROM Categories ", con);

                con.Open();
                SqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {
                    Category c = new Category();
                    c.Id = Convert.ToInt32(reader[0]);
                    c.Name = reader[1].ToString();
                    Categories.Add(c);
                }
                con.Close();
            }
            return Categories;
        }
    }
}
