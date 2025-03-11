using System.Reflection.Metadata.Ecma335;

namespace ChatBotDal.Entities
{
    public class Users
    {
        public long BotUserId { get; set; }
        public long TelegramUserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsBlocked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}