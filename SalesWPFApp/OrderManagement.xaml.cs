using BusinessObject.Models;
using DataAccess.Repository.Interface;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for OrderManagement.xaml
    /// </summary>
    public partial class OrderManagement : UserControl
    {
        IOrderRepository OrderRepository;
        MyDbContext dbContext;
        int _memberId;
        public OrderManagement(IOrderRepository repository, MyDbContext myDbContext, int memberId)
        {
            InitializeComponent();
            OrderRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            dbContext = myDbContext;
            _memberId = memberId;
        }
        public void LoadOrderList(MyDbContext db)
        {
            if(_memberId == 0)
            {
                lvOrders.ItemsSource = OrderRepository.GetOrders(db);
            }
            else
            {
                var source = OrderRepository.GetOrders(db).Where(item => item.MemberId == _memberId);
                lvOrders.ItemsSource = source;  
            }
        }
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadOrderList(dbContext);
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            if (_memberId != 0)
            {
                MessageBox.Show("You are not admin, you cannot add order");
                return;
            }
            OrderList detail = new OrderList(OrderRepository ,new Order(), dbContext, true , _memberId);
            detail.Closed += MessageBoxWindow_Closed; // Subscribe to the Closed event
            detail.ShowDialog();
        }
        private void MessageBoxWindow_Closed(object? sender, System.EventArgs e)
        {
            // Refresh the ListBox when the MessageBoxWindow is closed
            lvOrders.ItemsSource = null;
            LoadOrderList(dbContext); 
        }
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtOrderId.Text, out int OrderId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            var Order = OrderRepository.GetOrderByID(OrderId, dbContext);
            if (Order != null)
            {
                
                OrderList detail = new OrderList(OrderRepository, Order, dbContext, false, _memberId);
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
            if (!int.TryParse(txtOrderId.Text, out int OrderId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (!int.TryParse(txtMemberId.Text, out int memberId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (_memberId != memberId)
            {
                MessageBox.Show("You are not admin, you cannot edit order");
                return;
            }
            var Order = OrderRepository.GetOrderByID(OrderId, dbContext);
            if (Order != null)
            {
                OrderRepository.DeleteOrder(Order, dbContext);
                LoadOrderList(dbContext);
            }
            else
            {
                MessageBox.Show("Please enter ID");
            }
        }

        private void lvOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!int.TryParse(txtOrderId.Text, out int OrderId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (!int.TryParse(txtMemberId.Text, out int memberId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (_memberId != memberId && _memberId != 0)
            {
                MessageBox.Show("You are not admin, you cannot edit order");
                return;
            }
            var Order = OrderRepository.GetOrderByID(OrderId, dbContext);
            if (Order != null)
            {

                OrderList detail = new OrderList(OrderRepository, Order, dbContext, false, _memberId);
                detail.Closed += MessageBoxWindow_Closed; // Subscribe to the Closed event
                detail.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please enter ID");
            }
        }
    }
}
