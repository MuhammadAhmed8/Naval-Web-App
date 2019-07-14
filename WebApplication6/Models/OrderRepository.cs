using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class OrderRepository:IOrderRepository
    {
        public ApplicationDbContext context = new ApplicationDbContext();

        public IEnumerable<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                string query = @"
	                                SELECT Orders.*,Products.Name, Orderlines.Quantity FROM OrderLines
	                                INNER JOIN ORDERS
	                                ON OrderLines.OrderId = Orders.Id
	                                INNER JOIN PRODUCTS
	                                ON Orderlines.ProductId = Products.Id
	                                WHERE isDelivered = 0
	                                Order By Orders.Id
                               ";

                SqlCommand sqlcmd = new SqlCommand(query, con);
                
                //sqlcmd.Parameters.AddWithValue("@isDelivered");
                con.Open();
                SqlDataReader reader = sqlcmd.ExecuteReader();
                
                Order order = new Order();
                int i = 0;
                int prev = -1;
                while (reader.Read())
                {
                    if (i != 0 && prev != (int)reader[0])
                    {
                        orders.Add(order);
                    }
                    if (i == 0 || prev != (int)reader[0])
                    {   
                        order.OrderID = (int)reader[0];
                        order.Name = reader[1].ToString();
                        order.Address = reader[2].ToString();
                        order.Phone = reader[3].ToString();
                        order.AdditionalDetails = reader[4].ToString();
                        order.isDelivered = (bool)reader[5];
                        order.OrderLines = new List<CartLine>();
                    }
                    CartLine line = new CartLine();
                    Product p = new Product
                    {
                        Name = reader[6].ToString(),
                    };
                    line.Product = p;
                    order.OrderLines.Add(line);

                  
                    prev = (int)reader[0];

                    i++;

                }
                con.Close();


            }

            return orders;
        }
       public void MarkDelivered(int OrderId)
        {
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                SqlCommand sqlcmd = new SqlCommand("UPDATE Orders SET isDelivered = 1 WHERE Id = @orderId", con);
                sqlcmd.Parameters.AddWithValue("@orderId", OrderId);
                con.Open();
                sqlcmd.ExecuteNonQuery();
                con.Close();

            }
        }
        public void CancelOrder(int OrderId)
        {
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                SqlCommand sqlcmd = new SqlCommand("DELETE FROM ORDERLINES WHERE ORDERID = @orderId;DELETE FROM ORDERS WHERE ID = @orderId", con);
                sqlcmd.Parameters.AddWithValue("@orderId", OrderId);
                con.Open();
                sqlcmd.ExecuteNonQuery();
                con.Close();

            }
        }
        public void SaveOrder(Order order)
        {        
            using (SqlConnection con = new SqlConnection(context.Conn_str))
            {
                SqlCommand sqlcmd = new SqlCommand("INSERT INTO Orders(Name,Address,Phone,Details) VALUES(@name, @address, @phone, @details); SELECT CONVERT(int, SCOPE_IDENTITY()) AS OrderId", con);
                sqlcmd.Parameters.AddWithValue("@name", order.Name);
                sqlcmd.Parameters.AddWithValue("@address", order.Address);
                sqlcmd.Parameters.AddWithValue("@phone",order.Phone);
                sqlcmd.Parameters.AddWithValue("@details", order.AdditionalDetails);

                con.Open();
                
                int LastOrderId = (int)sqlcmd.ExecuteScalar();

                
                foreach(var orderline in order.OrderLines)
                {
                    int? ProductId  = orderline.Product.Id;
                    sqlcmd = new SqlCommand("CreateOrderline", con);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("orderId", LastOrderId);
                    sqlcmd.Parameters.AddWithValue("productId", ProductId);
                    sqlcmd.Parameters.AddWithValue("quantity", orderline.Quantity);
                    sqlcmd.ExecuteNonQuery();
                }

                con.Close();
            
        }
    }
}
}
