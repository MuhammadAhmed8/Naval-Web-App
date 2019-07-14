using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class UserRepository : IUserRepository
    {
        public ApplicationDbContext context = new ApplicationDbContext();

        public User GetUser(string username)
        {
            User user = null;
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                try
                {
                    SqlCommand sqlcmd = new SqlCommand("FindUser", con);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@username", username);
                   

                    con.Open();
                    SqlDataReader reader = sqlcmd.ExecuteReader();
                    int i = 0;
                    while (reader.Read())
                    {
                        if (i == 0)
                        {
                            user = new User
                            {
                                UserId = Convert.ToInt32(reader[0]),
                                Username = reader[1].ToString(),
                                FirstName = reader[3].ToString(),
                                LastName = reader[4].ToString(),
                                Email = reader[5].ToString(),
                                Phone = reader[6].ToString(),
                                Roles = new List<UserRole>()
                            };
                        }
                        user.Roles.Add(new UserRole
                        {
                            Name = reader[7].ToString()
                        });
                        i++;

                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    con.Close();
                }
            }
            return user;
        }
        public string GetPassword(string username)
        {
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                try
                {
                    SqlCommand sqlcmd = new SqlCommand("VerifyPass", con);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@username", username);

                    con.Open();
                    SqlDataReader reader = sqlcmd.ExecuteReader();
                    int i = 0;
                    while (reader.Read())
                    {
                        return reader[0].ToString();
                    }
                }

                catch (Exception)
                {

                }
                finally
                {
                    con.Close();
                }
            }
            return null;

        }

        public Auth_tokens GetTokens(string selector)
        {
            var auth = new Auth_tokens();
            bool flag = false;
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                
                try
                {
                    SqlCommand sqlcmd = new SqlCommand("SELECT Validator,UserID,Expires FROM Auth_tokens WHERE Selector = @selector", con);
                    sqlcmd.Parameters.AddWithValue("@selector", selector);
                    con.Open();
                    SqlDataReader reader = sqlcmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        auth.Validator = reader[0].ToString();
                        auth.UserId = (int)reader[1];
                        auth.Expires = (DateTime)reader[2];
                        flag = true;

                    }
                }

                catch (Exception)
                {

                }
                finally
                {
                    con.Close();
                }
               

            }
            return flag == true ? auth : null;
        }
        public void SetTokens(string selector,string token,int username,DateTime expires)
        {
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                try
                {
                    SqlCommand sqlcmd = new SqlCommand("set_tokens", con);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@username", username);
                    sqlcmd.Parameters.AddWithValue("@selector", selector);
                    sqlcmd.Parameters.AddWithValue("@token",token );
                    sqlcmd.Parameters.AddWithValue("@expires", expires);


                    con.Open();
                    sqlcmd.ExecuteNonQuery();
                }

                catch (Exception)
                {

                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
