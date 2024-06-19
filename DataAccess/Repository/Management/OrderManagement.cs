using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Management
{
    public class OrderManagement
    {
        //Using singleton Pattern
        private static OrderManagement? instance = null;
        private static readonly object instanceLock = new object();
        private OrderManagement() { }
        public static OrderManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderManagement();
                    }
                    return instance;
                }
            }
        }
        public void DeleteOrder(Order Order, MyDbContext dbContext)
        {
            try
            {
                var oldOrder = GetOrderByID(Order.OrderId, dbContext);
                if (oldOrder != null && dbContext != null)
                {
                    dbContext.Orders.Remove(Order);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The Order has not been exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Order? GetOrderByID(int id, MyDbContext dbContext)
        {
            Order? Order;
            try
            {
                if (dbContext == null)
                {
                    throw new Exception("DBContext is null");
                }
                Order = dbContext.Orders.FirstOrDefault(Order => Order.OrderId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Order;
        }

        public IEnumerable<Order> GetOrders(MyDbContext dbContext)
        {
            List<Order> Orders;
            if (dbContext.Orders == null)
            {
                return new List<Order>();
            }
            try
            {
                Orders = dbContext.Orders.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Orders;
        }

        public void InsertOrder(Order Order, MyDbContext dbContext)
        {
            try
            {
                var oldOrder = GetOrderByID(Order.OrderId, dbContext);
                if (oldOrder == null)
                {
                    dbContext.Orders.Add(Order);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The Order has been exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrder(Order Order, MyDbContext dbContext)
        {
            try
            {
                // Find the existing entity in the database
                var existingOrder = dbContext.Orders.Find(Order.OrderId);
                if (existingOrder == null)
                {
                    throw new Exception("The Order does not exist.");
                }
                if (Order != null)
                {
                    // Update the existing entity's properties
                    dbContext.Entry(existingOrder).CurrentValues.SetValues(Order);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The Order does not exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
