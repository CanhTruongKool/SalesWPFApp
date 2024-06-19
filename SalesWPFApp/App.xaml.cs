using BusinessObject.Models;
using DataAccess.Repository.Interface;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace SalesWPFApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);       

            var configuration = builder.Build();
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);


            var serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            // Resolve and show the main window
            try
            {
                mainWindow.Show();
            }
            catch (Exception)
            {
            }
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register repositories and other services
            services.AddSingleton<IMemberRepository, MemberRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IOrderDetailRepository, OrderDetailRepository>();
            // Register the main window
            services.AddSingleton<MemberManagement>();
            services.AddSingleton<ProductManagement>();
            services.AddSingleton<OrderManagement>();
            services.AddSingleton<OrderList>();
            services.AddSingleton<LoginWindow>();
            services.AddSingleton<Report>();
            services.AddSingleton<OrderDetailMagement>();
            services.AddTransient<MainWindow>();
        }
    }
}