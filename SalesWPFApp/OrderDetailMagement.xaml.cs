using BusinessObject.Models;
using DataAccess.Repository.Interface;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for OrderDetail.xaml
    /// </summary>
    public partial class OrderDetailMagement : Window
    {
        OrderDetail? editOrder;
        MyDbContext db;
        IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        IProductRepository productRepository = new ProductRepository();
        bool isNew;
        public OrderDetailMagement(OrderDetail? Order, MyDbContext dbContext, bool IsNew)
        {
            InitializeComponent();
            editOrder = Order;
            db = dbContext;
            isNew = IsNew;
            if (!isNew && editOrder != null)
            {
                InitData(editOrder);
                lbTitle.Content = "Edit Order detail";
                txtProductId.IsEnabled = false;
            }
            else
            {
                txtOrderId.Text = Order?.OrderId.ToString();
                lbTitle.Content = "Create Order detail";
            }
            DataContext = new ProductModel(dbContext, Order?.ProductId);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void InitData(OrderDetail orderDetail)
        {
            txtOrderId.Text = orderDetail.OrderId.ToString();
            txtProductId.Text = orderDetail.ProductId.ToString();
            txtQuantity.Text = orderDetail.Quantity.ToString();
            txtDiscount.Text = orderDetail.Discount.ToString();
            txtUnitPrice.Text = orderDetail.UnitPrice.ToString();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            editOrder = GetOrderDetail();
            if (editOrder == null) { return; }
            if (isNew)
            {
                CreateOrderDetail(editOrder);
            }
            else
            {
                UpdateOrderDetail(editOrder);
            }
        }
        private OrderDetail? GetOrderDetail()
        {
            var productId = productRepository.GetProducts(db)
                .Where(item => item.ProductName == txtProductId.Text )
                .Select(item => item.ProductId)
                .FirstOrDefault();
            if (!int.TryParse(txtOrderId.Text, out int orderId))
            {
                MessageBox.Show("Invalid orderID ");
                return null;
            }
            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Invalid quantity ");
                return null;
            }

            decimal unitPrice;
            if (string.IsNullOrEmpty(txtUnitPrice.Text))
            {
                unitPrice = productRepository.GetProductByID(productId, db).UnitPrice;
            }
            else
            {
                if (!decimal.TryParse(txtUnitPrice.Text, out unitPrice))
                {
                    MessageBox.Show("Invalid Price");
                    return null;
                }
            }
          
            if (!int.TryParse(txtDiscount.Text, out int discount))
            {
                MessageBox.Show("Invalid discount");
                return null;
            }
            return new OrderDetail()
            {
                OrderId = orderId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Discount = discount,
                ProductId = productId,
            };
        }
        private void CreateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                if (orderDetail == null)
                {
                    return;
                }
                orderDetailRepository.InsertOrderDetail(orderDetail, db);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Create successfully");
            this.Close();
        }
        private void UpdateOrderDetail(OrderDetail Order)
        {
            if (Order == null)
            {
                return;
            }
            try
            {
                if (int.TryParse(txtOrderId.Text, out int OrderId))
                {
                    Order.OrderId = OrderId;
                }
                else
                {
                    throw new Exception("ID is not valid");
                }
                orderDetailRepository.UpdateOrderDetail(Order, db);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Update successfully");
            this.Close();
        }

        private void txtProductId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtProductId.SelectedItem != null)
            {
                // Assuming your ComboBox is bound to a collection of products
                var selectedProduct = txtProductId.SelectedItem as Product;

                if (selectedProduct != null)
                {
                    var productPrice = productRepository.GetProducts(db)
                        .Where(item => item.ProductName == selectedProduct.ProductName)
                        .Select(item => item.UnitPrice)
                        .FirstOrDefault();

                    txtUnitPrice.Text = productPrice.ToString();
                }
            }
        }
    }
    public class ProductModel
    {
        public ObservableCollection<Product> lvProducts { get; set; }
        IProductRepository _productRepository = new ProductRepository();
        public int SelectedProductId { get; set; }
        public ProductModel(MyDbContext myDbContext, int? product)
        {
            var products = _productRepository.GetProducts(myDbContext);
            // Populate the collection with sample data
            lvProducts = new ObservableCollection<Product>(products);
            if(product != null)
            {
                SelectedProductId = products.Where(item => item.ProductId == product).
                                               Select(item => item.ProductId).FirstOrDefault();
            }
        }
    }
}
