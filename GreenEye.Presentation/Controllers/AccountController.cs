using GreenEye.Application.DTOs;
using GreenEye.Application.IRepository;
using GreenEye.Application.Responses;
using GreenEye.Infrasturcture.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenEye.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountRepository _accountRepo) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Response>> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(Request);

            var response = await _accountRepo.RegisterAsync(registerDTO);

            return response.Flag ? Ok(response) : BadRequest(Request);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(Request);

            var response = await _accountRepo.Login(loginDTO);

            return response.Flag ? Ok(response) : BadRequest(Request);
        }
    }
}
