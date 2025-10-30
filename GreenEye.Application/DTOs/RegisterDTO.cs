using GreenEye.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GreenEye.Application.DTOs
{
    public record RegisterDTO
        (
        [Required, MaxLength(250), MinLength(3)]string Name,
        [Required, EmailAddress] string Email,
        [Required, MaxLength(500), MinLength(3)] string Address,
        [Required, MaxLength(11), MinLength(11)] string TelephoneNumber,
        [Required, DataType(DataType.Password)] string Password,
        [Required] Roles Role
        );
}
