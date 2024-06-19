using BusinessObject.Models;
using DataAccess.Repository.Interface;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for ProductDetail.xaml
    /// </summary>
    public partial class ProductDetail : Window
    {
        Product? editProduct;
        MyDbContext db;
        IProductRepository ProductRepository = new ProductRepository();
        bool isNew;
        public ProductDetail(Product? Product, MyDbContext dbContext, bool IsNew)
        {
            InitializeComponent();
            editProduct = Product;
            db = dbContext;
            isNew = IsNew;
            if (!isNew && editProduct != null)
            {
                InitData(editProduct);
                lbTitle.Content = "Edit Product";
            }
            else
            {
                lbTitle.Content = "Create Product";
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void InitData(Product Product)
        {
            txtProductId.Text = Product.ProductId.ToString();
            txtProductName.Text = Product.ProductName;
            txtCategoryId.Text = Product.CategoryId.ToString();
            txtWeight.Text = Product.Weight.ToString(); 
            txtUnitPrice.Text = Product.UnitPrice.ToString();
            txtUnitsInStock.Text = Product.UnitsInStock.ToString();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            editProduct = GetProduct();
            if(editProduct == null) { return; }
            if (isNew)
            {
                CreateProduct(editProduct);
            }
            else
            {
                UpdateProduct(editProduct);
            }
        }
        private Product? GetProduct()
        {
            if (!int.TryParse(txtCategoryId.Text, out int categoryId)) {
                MessageBox.Show("Invalid Category ID");
                return null;
            }
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice))
            {
                MessageBox.Show("Invalid Price");
                return null;
            }
            if (!int.TryParse(txtUnitsInStock.Text, out int unitsInStock))
            {
                MessageBox.Show("Invalid Unit");
                return null;
            }
            return new Product()
            {
                ProductName = txtProductName.Text,
                CategoryId = categoryId,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                Weight = txtWeight.Text,
            };
        }
        private void CreateProduct(Product Product)
        {
            try
            {
                if(Product == null) {
                    return;
                }
                ProductRepository.InsertProduct(Product, db);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Create successfully");
            this.Close();
        }
        private void UpdateProduct(Product Product)
        {
            if(Product == null) {
                return;
            }
            try
            {
                if (int.TryParse(txtProductId.Text, out int ProductId))
                {
                    Product.ProductId = ProductId;
                }
                else
                {
                    throw new Exception("ID is not valid");
                }
                ProductRepository.UpdateProduct(Product, db);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ;
            }
            MessageBox.Show("Update successfully");
            this.Close();
        }
    }
}
