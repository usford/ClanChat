namespace ClanChat.Shared.Database.EntityModels
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public List<Chat>? Chats { get; set; }
    }
}
