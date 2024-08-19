using ClanChat.Shared.Database.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace ClanChat.Shared.Database
{
    public class ClanChatDbContext : DbContext
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Clan> Clans { get; set; }

        public ClanChatDbContext(DbContextOptions<ClanChatDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
