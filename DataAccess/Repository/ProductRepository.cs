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
    public class ProductRepository : IProductRepository
    {
        public void DeleteProduct(Product Product, MyDbContext dbContext)
        {
            ProductManagement.Instance.DeleteProduct(Product, dbContext);
        }

        public Product? GetProductByID(int id, MyDbContext db)
        {
            return ProductManagement.Instance.GetProductByID(id, db);
        }

        public IEnumerable<Product> GetProducts(MyDbContext dbContext)
        {
            return ProductManagement.Instance.GetProducts(dbContext);
        }

        public void InsertProduct(Product Product, MyDbContext dbContext)
        {
            ProductManagement.Instance.InsertProduct(Product, dbContext);
        }

        public void UpdateProduct(Product Product, MyDbContext dbContext)
        {
            ProductManagement.Instance.UpdateProduct(Product, dbContext);
        }
    }
}