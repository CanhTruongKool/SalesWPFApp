using BusinessObject.Models;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for OrderList.xaml
    /// </summary>
    public partial class OrderList : Window
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository = new OrderDetailRepository();
        IMemberRepository _memberRepository = new MemberRepository();
        public string SelectedMemberId { get; set; }
        Order _order;
        MyDbContext dbContext;
        bool _isNew = false;
        int _memberID;
        public OrderList(IOrderRepository orderRepository,
            Order order,
            MyDbContext myDbContext,
            bool isNew,
            int memberID)
        {
            InitializeComponent();
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _order = order;
            _isNew = isNew;
            dbContext = myDbContext;
            _memberID = memberID;
            InitOrder(order);
            SetTodayDate();
            DataContext = new MemberModel(myDbContext, order);
            
        }
        public void LoadOrderList(MyDbContext db)
        {
            var orderId = int.Parse(txtOrderId.Text);
            lvOrderDetails.ItemsSource = _orderDetailRepository.GetOrderDetail(orderId, db);
        }
        private void SetTodayDate()
        {
            txtOrderDate.DisplayDateStart = DateTime.Today;
            txtRequiredDate.DisplayDateStart = DateTime.Today;
            txtShippedDate.DisplayDateStart = DateTime.Today;
        }
        private void InitOrder(Order order)
        {
            if (_isNew) { lbTitle.Content = "Create order"; return; }
            lbTitle.Content = "Update order";
            txtOrderId.Text = order.OrderId.ToString();
            txtFreightName.Text = order.Freight.ToString();
            txtOrderDate.Text = order.OrderDate.ToString();
            txtRequiredDate.Text = order.RequiredDate.ToString();
            txtShippedDate.Text = order.ShippedDate.ToString();

            if(_memberID != 0)
            {
                txtMemberId.IsEnabled = false;
                txtOrderId.IsEnabled = false;
                txtFreightName.IsEnabled = false;
                txtOrderDate.IsEnabled = false;
                txtProductId.IsEnabled = false;
                txtRequiredDate.IsEnabled = false;
                txtShippedDate.IsEnabled = false;
            }
        }
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadOrderList(dbContext);
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            if (_memberID != 0)
            {
                MessageBox.Show("You cannot edit order ");
                return;
            }
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.OrderId = int.Parse(txtOrderId.Text);   
            OrderDetailMagement detail = new OrderDetailMagement(orderDetail, dbContext, true);
            detail.Closed += MessageBoxWindow_Closed; // Subscribe to the Closed event
            detail.ShowDialog();
        }
        private void MessageBoxWindow_Closed(object? sender, System.EventArgs e)
        {
            // Refresh the ListBox when the MessageBoxWindow is closed
            LoadOrderList(dbContext);
        }
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if(_memberID != 0)
            {
                MessageBox.Show("You cannot edit order ");
                return;
            }
            if (!int.TryParse(txtOrderId.Text, out int orderId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            
            if (!int.TryParse(txtProductId.Text, out int productId))
            {
                return;
            };
            var Order = _orderDetailRepository.GetOderDetailByID(orderId, productId, dbContext);
            if (Order != null)
            {
                OrderDetailMagement detail = new OrderDetailMagement(Order, dbContext, false);
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
            if (_memberID != 0)
            {
                MessageBox.Show("You cannot edit order ");
                return;
            }

            if (!int.TryParse(txtOrderId.Text, out int OrderId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (!int.TryParse(txtProductId.Text, out int productId))
            {
                return;
            };
            var Order = _orderDetailRepository.GetOderDetailByID(OrderId, productId, dbContext);
            if (Order != null)
            {
                _orderDetailRepository.DeleteOrderDetail(Order, dbContext);
                LoadOrderList(dbContext);
            }
            else
            {
                MessageBox.Show("Please enter ID");
            }
        }
        private Order? GetOrder()
        {

            var memberId = _memberRepository.GetMembers(dbContext)
                         .Where(item => item.Fullname.Equals(txtMemberId.Text))
                         .Select(item => item.MemberId)
                         .FirstOrDefault();

            if (!decimal.TryParse(txtFreightName.Text, out decimal Freight))
            {
                MessageBox.Show("Freight is not valid"); return null;
            }

            if (!DateTime.TryParse(txtOrderDate.Text, out DateTime orderDate))
            {
                MessageBox.Show("Order date not valid"); return null;
            }

            DateTime? _requiredDate, _shipDate;

            if (txtRequiredDate.Text.IsNullOrEmpty()) _requiredDate = null;
            else
            {
                if (!DateTime.TryParse(txtRequiredDate.Text, out DateTime requiredDate))
                {
                    MessageBox.Show("Required date not valid"); return null;
                }
                else
                {
                    _requiredDate = requiredDate;
                }
            }

            if (txtShippedDate.Text.IsNullOrEmpty()) _shipDate = null;
            else
            {
                if (!DateTime.TryParse(txtShippedDate.Text, out DateTime shipDate))
                {
                    MessageBox.Show("Shipped Date not valid"); return null;
                }
                else
                {
                    _shipDate = shipDate;
                }
            }


            return new Order()
            {
                MemberId = memberId,
                Freight = Freight,
                OrderDate = orderDate,
                ShippedDate = _shipDate,
                RequiredDate = _requiredDate,
            };
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_memberID != 0)
            {
                MessageBox.Show("You cannot edit order ");
                return;
            }
            var order = GetOrder();
            if (order == null)
            {
                return;
            }
            else if (!_isNew)
            {
                UpdateOrder(order);
            }
            else if (_isNew)
            {
                _orderRepository.InsertOrder(order, dbContext);
                MessageBox.Show("Save successful");
                this.Close();
            }
        }

        private void UpdateOrder(Order order)
        {
            if (_memberID != 0)
            {
                MessageBox.Show("You cannot edit order ");
                    return;
            }
            if (order == null) { return; }
            if (!int.TryParse(txtOrderId.Text, out int OrderId))
            {
                MessageBox.Show("Order ID is not valid");
                return;
            }
            else
            {
                order.OrderId = OrderId;
                _orderRepository.UpdateOrder(order, dbContext);
                MessageBox.Show("Save successful");
                this.Close();
            }
        }

        private void deleteDetailButton_Click(object sender, RoutedEventArgs e)
        {
            if (_memberID != 0)
            {
                MessageBox.Show("You cannot edit order ");
                return;
            }
            var order = GetOrder();
            if (order == null)
            {
                return;
            }
            else
            {
                _orderRepository.DeleteOrder(order, dbContext);
                MessageBox.Show("Delete successful");
                this.Close();
            }
        }

        private void lvOrderDetails_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_memberID != 0)
            {
                MessageBox.Show("You cannot edit order ");
                return;
            }
            if (!int.TryParse(txtOrderId.Text, out int orderId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (!int.TryParse(txtProductId.Text, out int productId))
            {
                return;
            };
            var Order = _orderDetailRepository.GetOderDetailByID(orderId, productId, dbContext);
            if (Order != null)
            {
                OrderDetailMagement detail = new OrderDetailMagement(Order, dbContext, false);
                detail.Closed += MessageBoxWindow_Closed; // Subscribe to the Closed event
                detail.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please enter ID");
            }
        }
    }
    public class MemberModel
    {
        public ObservableCollection<Member> lvMembers { get; set; }
        IMemberRepository _memberRepository = new MemberRepository();
        public int SelectedMemberId { get; set; }
        public MemberModel(MyDbContext myDbContext, Order order)
        {
            var members = _memberRepository.GetMembers(myDbContext);
            // Populate the collection with sample data
            lvMembers = new ObservableCollection<Member>(members);

            SelectedMemberId = members.Where(item => item.MemberId == order.MemberId)
                                      .Select(item => item.MemberId).FirstOrDefault();
        }
    }
}
