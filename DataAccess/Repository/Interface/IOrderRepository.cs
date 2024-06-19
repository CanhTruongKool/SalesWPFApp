using BusinessObject.Models;

namespace DataAccess.Repository.Interface
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders(MyDbContext myDbContext);
        Order? GetOrderByID(int id, MyDbContext dbContext);
        void InsertOrder(Order order, MyDbContext dbContext);
        void UpdateOrder(Order order, MyDbContext dbContext);
        void DeleteOrder(Order order, MyDbContext dbContext);
    }
}
