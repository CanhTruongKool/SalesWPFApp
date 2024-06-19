using BusinessObject.Management;
using BusinessObject.Models;
using DataAccess.Repository.Interface;
using DataAccess.Repository.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository :IOrderRepository
    {
        public void DeleteOrder(Order Order, MyDbContext dbContext)
        {
            OrderManagement.Instance.DeleteOrder(Order, dbContext);
        }

        public Order? GetOrderByID(int id, MyDbContext db)
        {
            return OrderManagement.Instance.GetOrderByID(id, db);
        }

        public IEnumerable<Order> GetOrders(MyDbContext dbContext)
        {
            return OrderManagement.Instance.GetOrders(dbContext);
        }

        public void InsertOrder(Order Order, MyDbContext dbContext)
        {
            OrderManagement.Instance.InsertOrder(Order, dbContext);
        }

        public void UpdateOrder(Order Order, MyDbContext dbContext)
        {
            OrderManagement.Instance.UpdateOrder(Order, dbContext);
        }
    }
}
