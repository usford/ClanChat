using ClanChat.Shared.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ClanChat.Desktop
{

    public partial class App : Application
    {

        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();

            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<ClanChatDbContext>(options =>
                options.UseNpgsql("Server=localhost;Port=5432;User ID=postgres;Password=postgres;Database=ClanChat;Include Error Detail=true"));

            services.AddTransient<MainWindow>();
        }
    }

}
