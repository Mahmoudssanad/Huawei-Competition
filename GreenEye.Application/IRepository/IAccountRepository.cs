using GreenEye.Application.DTOs;
using GreenEye.Application.Responses;

namespace GreenEye.Application.IRepository
{
    public interface IAccountRepository
    {
        Task<Response> RegisterAsync(RegisterDTO registerDTO);

        Task<Response> Login(LoginDTO loginDTO);
    }
}
