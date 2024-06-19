using BusinessObject.Models;
using DataAccess.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
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
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : UserControl
    {
        IOrderRepository OrderRepository;
        MyDbContext dbContext;
        int _memberId;
        public Report(IOrderRepository repository, MyDbContext myDbContext, int memberId)
        {
            InitializeComponent();
            OrderRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            dbContext = myDbContext;
            _memberId = memberId;
        }
        public void LoadOrderList(MyDbContext db)
        {
            if (_memberId == 0)
            {
                lvOrders.ItemsSource = OrderRepository.GetOrders(db).OrderBy(item => item.OrderDate);
            }
        }
        public void SearchOrderList(MyDbContext db)
        {
            var orders = OrderRepository.GetOrders(db);

            if (!txtFromDate.Text.IsNullOrEmpty()) {
                bool v = DateTime.TryParse(txtFromDate.Text, out  DateTime fromDate); ;
                orders = orders.Where(item => item.OrderDate >= fromDate );
            } 
            if (!txtOrderDate.Text.IsNullOrEmpty()) {
                bool v = DateTime.TryParse(txtOrderDate.Text, out  DateTime toDate); ;
                orders = orders.Where(item => item.OrderDate <= toDate );
            }
            lvOrders.ItemsSource = orders.OrderBy(item =>item.OrderDate);
        }
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadOrderList(dbContext);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchOrderList(dbContext); 
        }
    }
}
