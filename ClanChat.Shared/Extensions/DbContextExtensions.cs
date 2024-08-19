using ClanChat.Shared.Database;
using ClanChat.Shared.Database.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClanChat.Shared.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ClanChatDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("ClanChat.Shared")
                )
            );
        }

        public static async Task SeedDataDb(this IServiceProvider serviceProvider)
        {
            using (var context = new ClanChatDbContext(serviceProvider.GetRequiredService<DbContextOptions<ClanChatDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;
                }

                context.Users.AddRange(
                    new User
                    {
                        Name = "user",
                        Password = "user"
                    },
                    new User
                    {
                        Name = "user2",
                        Password = "user2"
                    }
                );

                context.Clans.AddRange(
                    new Clan
                    {
                        Name = "clan"
                    },
                    new Clan
                    {
                        Name = "clan2"
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
