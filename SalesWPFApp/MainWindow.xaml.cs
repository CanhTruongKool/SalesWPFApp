using BusinessObject.Models;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IMemberRepository _memberRepository;
        IProductRepository _productRepository;
        IOrderRepository _orderRepository;
        MyDbContext context;
        List<Member> members;
        public int LoggedInMemberId { get; private set; }

        public MainWindow(IMemberRepository memberRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            MyDbContext myDbContext)
        {
            InitializeComponent();
            context = myDbContext;
            ShowLoginWindow();
            _memberRepository = memberRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MembersTab.IsSelected)
            {
                MainContentControl.Content = new MemberManagement(_memberRepository, context, LoggedInMemberId);
            }
            else if (ProductsTab.IsSelected)
            {
                MainContentControl.Content = new ProductManagement(_productRepository, context, LoggedInMemberId);
            }
            else if (OrdersTab.IsSelected)
            {
                MainContentControl.Content = new OrderManagement(_orderRepository, context, LoggedInMemberId);
            }
            else if (ReportTab.IsSelected)
            {
                MainContentControl.Content = new Report(_orderRepository, context, LoggedInMemberId);
            }
            else
            {
                MainContentControl.Content = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var admin = GetAdmin();
            members = _memberRepository.GetMembers(context).ToList();
            if(admin != null)
            {
                LoginWindow login = new(admin.Email, admin.Password, members);
                bool? result = login.ShowDialog();
                if (result == true)
                {
                    LoggedInMemberId = login.LoggedInMemberId.GetValueOrDefault();
                    var member = context.Members.FirstOrDefault(m => m.MemberId == LoggedInMemberId);
                    if (member != null)
                        txtHello.Text = "Hello " + member.Fullname;
                    if (LoggedInMemberId != 0)
                    {
                        ReportTab.IsEnabled = false;
                    }
                }
            }
        }

        private void ShowLoginWindow()
        {
            AdminCredentials? adminCredentials = GetAdmin();

            members = context.Members.ToList();
            if(adminCredentials == null) {  return; }

            var loginWindow = new LoginWindow(adminCredentials.Email, adminCredentials.Password, members);
            bool? result = loginWindow.ShowDialog();

            if (result == true)
            {
                LoggedInMemberId = loginWindow.LoggedInMemberId.GetValueOrDefault();
                var member = context.Members.FirstOrDefault(m => m.MemberId == LoggedInMemberId);
                if (member != null)
                txtHello.Text = "Hello " + member.Fullname;
                if (LoggedInMemberId != 0)
                {
                    ReportTab.IsEnabled = false;
                }
            }
            else
            {
                // Handle login failure (e.g., close the application)
                MessageBox.Show("Login required. Closing application.");
                this.Close();
            }
        }
        private AdminCredentials? GetAdmin()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();
            var adminCredentials = configuration.GetSection("AdminCredentials")
            .Get<AdminCredentials>();

            return adminCredentials;
        }
        public class AdminCredentials
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
