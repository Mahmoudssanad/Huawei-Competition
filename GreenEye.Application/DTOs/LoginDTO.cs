using System.ComponentModel.DataAnnotations;

namespace GreenEye.Application.DTOs
{
    public record LoginDTO
        (
        [Required, EmailAddress] string Email,
        [Required, DataType(DataType.Password)] string Password
        );
}
