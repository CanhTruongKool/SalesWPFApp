using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Interface
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetOrderDetail(int orderId, MyDbContext myDbContext);
        OrderDetail? GetOderDetailByID(int id,int productID, MyDbContext myDbContext);
        void InsertOrderDetail(OrderDetail orderDetail, MyDbContext myDbContext);
        void UpdateOrderDetail(OrderDetail orderDetail, MyDbContext myDbContext);
        void DeleteOrderDetail(OrderDetail orderDetail, MyDbContext myDbContext);
    }
}
