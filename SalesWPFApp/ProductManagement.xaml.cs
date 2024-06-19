using BusinessObject.Models;
using DataAccess.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for ProductManagement.xaml
    /// </summary>
    public partial class ProductManagement : UserControl
    {
        IProductRepository productRepository;
        MyDbContext dbContext;
        int _memberId;
        public ProductManagement(IProductRepository repository, MyDbContext myDbContext, int memberId)
        {
            InitializeComponent();
            productRepository = repository;
            dbContext = myDbContext;
            _memberId = memberId;
        }
        public void LoadProductList(MyDbContext db)
        {
            lvProducts.ItemsSource = productRepository.GetProducts(db);
        }
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadProductList(dbContext);
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            if (_memberId != 0)
            {
                MessageBox.Show("You are not admin, you cannot edit product");
                return;
            }
            else
            {
                ProductDetail detail = new ProductDetail(new Product(), dbContext, true);
                detail.Closed += MessageBoxWindow_Closed; // Subscribe to the Closed event
                detail.ShowDialog();
            }
            
        }
        private void MessageBoxWindow_Closed(object? sender, System.EventArgs e)
        {
            // Refresh the ListBox when the MessageBoxWindow is closed
            LoadProductList(dbContext);
            SetData();
        }
        private void SetData()
        {
            // Refresh item
            lvProducts.SelectedItem = null;  
        }
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtProductId.Text, out int ProductId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if(_memberId != 0)
            {
                MessageBox.Show("You are not admin, you cannot edit product");
                return;
            }
            var product = productRepository.GetProductByID(ProductId, dbContext);
            if (product != null)
            {
                ProductDetail detail = new ProductDetail(product, dbContext, false);
                detail.Closed += MessageBoxWindow_Closed; // Subscribe to the Closed event
                detail.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please enter ID");
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtProductId.Text, out int ProductId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (_memberId != 0)
            {
                MessageBox.Show("You are not admin, you cannot edit product");
                return;
            }
            var product = productRepository.GetProductByID(ProductId, dbContext);
            if (product != null)
            {
                productRepository.DeleteProduct(product, dbContext);
                LoadProductList(dbContext);
            }
            else
            {
                MessageBox.Show("Please enter ID");
            }
        }

        private void lvProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!int.TryParse(txtProductId.Text, out int ProductId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (_memberId != 0)
            {
                MessageBox.Show("You are not admin, you cannot edit product");
                return;
            }
            var product = productRepository.GetProductByID(ProductId, dbContext);
            if (product != null)
            {
                ProductDetail detail = new ProductDetail(product, dbContext, false);
                detail.Closed += MessageBoxWindow_Closed; // Subscribe to the Closed event
                detail.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please enter ID");
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
  
            var searchProducts = productRepository.GetProducts(dbContext);
            if(!txtProductId.Text.IsNullOrEmpty())
            {
                int searchID = int.Parse(txtProductId.Text);
                searchProducts = searchProducts.Where(item => item.ProductId == searchID);
            }
            if (!txtCategoryId.Text.IsNullOrEmpty())
            {
                int categoryId = int.Parse(txtCategoryId.Text);
                searchProducts = searchProducts.Where(item => item.CategoryId == categoryId);
            }
            if (!txtProductName.Text.IsNullOrEmpty())
            {
                var productName = txtProductName.Text;
                searchProducts = searchProducts.Where(item => item.ProductName.ToUpper().Contains(productName.ToUpper()));
            }
            if (!txtWeight.Text.IsNullOrEmpty())
            {
                var weight = txtWeight.Text;
                searchProducts = searchProducts.Where(item => item.Weight == weight);
            } 
            if (!txtUnitPrice.Text.IsNullOrEmpty())
            {
                decimal price = decimal.Parse(txtUnitPrice.Text);
                searchProducts = searchProducts.Where(item => item.UnitPrice == price);
            }

            if (!txtUnitInStock.Text.IsNullOrEmpty())
            {
                int unitInStock = int.Parse(txtUnitInStock.Text);
                searchProducts = searchProducts.Where(item => item.UnitsInStock == unitInStock);
            }
                
            lvProducts.ItemsSource = searchProducts;
        }
    }
}
