using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication6.Models
{
    public class ProductRepository : IProductRepository
    {
        // private string conn_str = "Server=(localdb)\\MSSQLLocalDB;Database=DairyShop;Trusted_Connection=True;MultipleActiveResultSets=true";
        public ApplicationDbContext context = new ApplicationDbContext();

        public void SaveProduct(Product product)
        {
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                SqlCommand sqlcmd = new SqlCommand("SaveProduct", con);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@id", product?.Id ?? -1);
                sqlcmd.Parameters.AddWithValue("@name", product.Name);
                sqlcmd.Parameters.AddWithValue("@price", product.Price);
                sqlcmd.Parameters.AddWithValue("@description", product.Description);
                sqlcmd.Parameters.AddWithValue("@quantity", product.Quantity);
                sqlcmd.Parameters.AddWithValue("@image", product.Image);
                sqlcmd.Parameters.AddWithValue("@isAvailable", product.isAvailable);
                sqlcmd.Parameters.AddWithValue("@categoryId", product.CategoryId);


                con.Open();
                sqlcmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public IQueryable<Product> GetAllProducts()
        {
            List<Product> Products = new List<Product>();
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                SqlCommand sqlcmd = new SqlCommand("GetAllProducts", con);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                
                con.Open();
                SqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {
                    Product p = new Product();
                    p.Id = Convert.ToInt32(reader[0]);
                    p.Name = reader[1].ToString();
                    p.Price = Convert.ToDecimal(reader[2]);
                    p.Description = reader[3].ToString();
                    p.Image = reader[5].ToString();
                    p.isAvailable = Convert.ToBoolean(reader[6]);
                    p.CategoryId = Convert.ToInt32(reader[7]);
                    
                    p.CategoryName = reader[8].ToString();
                    Products.Add(p);
                }
                con.Close();
            }
            return Products.AsQueryable();
        }
        public IQueryable<Product> GetProductsByCategory(string category)
        {
            
            List<Product> Products = new List<Product>();
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                try
                {
                    SqlCommand sqlcmd = new SqlCommand(@"SELECT Products.* FROM Products

                                                     INNER JOIN Categories

                                                     ON Products.CategoryId = Categories.Id

                                                     WHERE Categories.Name = @name", con);
                    sqlcmd.Parameters.AddWithValue("@name", category);
                    con.Open();
                    SqlDataReader reader = sqlcmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Product p = new Product();
                        p.Id = Convert.ToInt32(reader[0]);
                        p.Name = reader[1].ToString();
                        p.Price = Convert.ToDecimal(reader[2]);
                        p.Description = reader[3].ToString();
                        p.Image = reader[5].ToString();
                        p.isAvailable = Convert.ToBoolean(reader[6]);
                        Products.Add(p);
                    }
                }
                catch (Exception e) {

                }
                finally {
                    con.Close();

                }

                return Products.AsQueryable();
            }
        }

  
    }
}
