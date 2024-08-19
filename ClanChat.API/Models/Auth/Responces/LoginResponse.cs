using System.ComponentModel.DataAnnotations;

namespace ClanChat.API.Models.Auth.Responces
{
    public class LoginResponse
    {
        [Required(ErrorMessage = $"Поле {nameof(Token)} не может быть пустым")]
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = $"Поле {nameof(UserId)} не может быть пустым")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = $"Поле {nameof(Username)} не может быть пустым")]
        public string Username { get; set; } = null!;
    }
}
