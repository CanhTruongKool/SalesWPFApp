using BusinessObject.Models;

namespace DataAccess.Repository.Interface
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts(MyDbContext dbContext);
        Product? GetProductByID(int id, MyDbContext dbContext);
        void InsertProduct(Product product, MyDbContext dbContext);
        void UpdateProduct(Product product, MyDbContext dbContext);
        void DeleteProduct(Product product, MyDbContext dbContext);
    }
}
