using BusinessObject.Models;
using DataAccess.Repository.Interface;
using DataAccess.Repository.Management;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void DeleteOrderDetail(OrderDetail OrderDetail, MyDbContext dbContext)
        {
            OrderDetailManagement.Instance.DeleteOrderDetail(OrderDetail, dbContext);
        }

        public OrderDetail? GetOderDetailByID(int id, int productId, MyDbContext myDbContext)
        {
            return OrderDetailManagement.Instance.GetOrderDetailByID(id, productId, myDbContext);
        }

        public IEnumerable<OrderDetail> GetOrderDetail(int orderId, MyDbContext myDbContext)
        {
            return OrderDetailManagement.Instance.GetOrderDetails(orderId, myDbContext);
        }


        public void InsertOrderDetail(OrderDetail OrderDetail, MyDbContext dbContext)
        {
            OrderDetailManagement.Instance.InsertOrderDetail(OrderDetail, dbContext);
        }

        public void UpdateOrderDetail(OrderDetail OrderDetail, MyDbContext dbContext)
        {
            OrderDetailManagement.Instance.UpdateOrderDetail(OrderDetail, dbContext);
        }
    }
}
