using BusinessObject.Management;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Management
{
    public class OrderDetailManagement
    {

        //Using singleton Pattern
        private static OrderDetailManagement? instance = null;
        private static readonly object instanceLock = new object();
        private OrderDetailManagement() { }
        public static OrderDetailManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailManagement();
                    }
                    return instance;
                }
            }
        }
        public void DeleteOrderDetail(OrderDetail OrderDetail, MyDbContext dbContext)
        {
            try
            {
                var oldOrderDetail = GetOrderDetailByID(OrderDetail.OrderId, OrderDetail.ProductId, dbContext);
                if (oldOrderDetail != null && dbContext != null)
                {
                    dbContext.OrderDetails.Remove(OrderDetail);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The OrderDetail has not been exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public OrderDetail? GetOrderDetailByID(int id,int prouctId, MyDbContext dbContext)
        {
            OrderDetail? OrderDetail;
            try
            {
                if (dbContext == null)
                {
                    throw new Exception("DBContext is null");
                }
                OrderDetail = dbContext.OrderDetails.
                    FirstOrDefault(OrderDetail => OrderDetail.OrderId == id &&
                    OrderDetail.ProductId == prouctId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return OrderDetail;
        }

        public IEnumerable<OrderDetail> GetOrderDetails(int orderId,MyDbContext dbContext)
        {
            List<OrderDetail> OrderDetails;
            if (dbContext.OrderDetails == null)
            {
                return new List<OrderDetail>();
            }
            try
            {
                OrderDetails = dbContext.OrderDetails.Where(item => item.OrderId == orderId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return OrderDetails;
        }

        public void InsertOrderDetail(OrderDetail OrderDetail, MyDbContext dbContext)
        {
            try
            {
                var oldOrderDetail = GetOrderDetailByID(OrderDetail.OrderId,OrderDetail.ProductId, dbContext);
                if (oldOrderDetail == null)
                {
                    dbContext.OrderDetails.Add(OrderDetail);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The OrderDetail has been exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrderDetail(OrderDetail OrderDetail, MyDbContext dbContext)
        {
            try
            {
                // Find the existing entity in the database
                var existingOrderDetail = GetOrderDetailByID(OrderDetail.OrderId, OrderDetail.ProductId, dbContext);
                if (existingOrderDetail == null)
                {
                    throw new Exception("The OrderDetail does not exist.");
                }
                if (OrderDetail != null)
                {
                    // Update the existing entity's properties
                    dbContext.Entry(existingOrderDetail).CurrentValues.SetValues(OrderDetail);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The OrderDetail does not exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
