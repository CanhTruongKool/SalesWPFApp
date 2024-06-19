using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Management
{
    public class ProductManagement
    {
        //Using singleton Pattern
        private static ProductManagement? instance = null;
        private static readonly object instanceLock = new object();
        private ProductManagement() { }
        public static ProductManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductManagement();
                    }
                    return instance;
                }
            }
        }
        public void DeleteProduct(Product product, MyDbContext dbContext)
        {
            try
            {
                var oldProduct = GetProductByID(product.ProductId, dbContext);
                if (oldProduct != null && dbContext != null)
                {
                    dbContext.Products.Remove(product);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The Product has not been exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Product? GetProductByID(int id, MyDbContext dbContext)
        {
            Product? Product;
            try
            {
                if (dbContext == null)
                {
                    throw new Exception("DBContext is null");
                }
                Product = dbContext.Products.FirstOrDefault(Product => Product.ProductId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Product;
        }

        public IEnumerable<Product> GetProducts(MyDbContext dbContext)
        {
            List<Product> Products;
            if (dbContext.Products == null)
            {
                return new List<Product>();
            }
            try
            {
                Products = dbContext.Products.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Products;
        }

        public void InsertProduct(Product Product, MyDbContext dbContext)
        {
            if(!CheckValidCategory(Product, dbContext)) {
                throw new Exception("The category does not exist. ");
            }
            try
            {
                var oldProduct = GetProductByID(Product.ProductId, dbContext);
                if (oldProduct == null)
                {
                    dbContext.Products.Add(Product);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The Product has been exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProduct(Product Product, MyDbContext dbContext)
        {
            if (!CheckValidCategory(Product, dbContext))
            {
                throw new Exception("The category does not exist. ");
            }
            try
            {
                // Find the existing entity in the database
                var existingProduct = dbContext.Products.Find(Product.ProductId);
                if (existingProduct == null)
                {
                    throw new Exception("The Product does not exist.");
                }
                if (Product != null)
                {
                    // Update the existing entity's properties
                    dbContext.Entry(existingProduct).CurrentValues.SetValues(Product);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The Product does not exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private bool CheckValidCategory(Product product, MyDbContext dbContext)
        {
            var isValidCate = false;
            var categoryList = dbContext.Categories.ToList();
            foreach (var category in categoryList)
            {
                if (product.CategoryId == category.CategoryId)
                {
                    isValidCate = true;
                }
            }
            return isValidCate;
        }
    }
}
