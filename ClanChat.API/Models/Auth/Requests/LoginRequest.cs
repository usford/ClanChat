using System.ComponentModel.DataAnnotations;

namespace ClanChat.API.Models.Auth.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = $"Поле {nameof(Username)} не может быть пустым")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = $"Поле {nameof(Username)} не может быть пустым")]
        public string Password { get; set; } = null!;
    }
}
