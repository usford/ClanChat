namespace ClanChat.Shared.Database.EntityModels
{
    public class Chat
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid ClanId { get; set; }
        public Clan Clan { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime TimeDispatchDate { get; set; }
    }
}
