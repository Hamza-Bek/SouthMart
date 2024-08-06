using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountRepository _account , IHttpContextAccessor _httpContextAccessor) : Controller
    {

        [HttpPost("identity/login")]
        public async Task<ActionResult<GeneralResponse>> LoginAccount(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model user cannot be null!");

            return Ok(await _account.LoginAccountAsync(model));
        }

        [HttpPost("identity/login/seller")]
        public async Task<ActionResult<GeneralResponse>> LoginSellerAccount(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model user cannot be null!");

            return Ok(await _account.LoginSellerAccountAsync(model));
        }

        [HttpPost("identity/create")]
        public async Task<ActionResult<GeneralResponse>> CreateAccount(CreateAccountDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model cannot be null!");

            return Ok(await _account.CreateAccountAsync(model));
        }

        [HttpPost("identity/role/create")]
        public async Task<ActionResult<GeneralResponse>> CreateRole(CreateRoleDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model role cannot be null!");

            return Ok(await _account.CreateRoleAsync(model));
        }

        [HttpPost("identity/refresh-token")]
        public async Task<ActionResult<GeneralResponse>> RefreshToken(RefreshTokenDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model cannot be null!");

            return Ok(await _account.RefreshTokenAsync(model));
        }

        [HttpGet("get-user/{userId}")]
        public async Task<ActionResult<GetUserDTO>> GetUser(string userId)
        {
            var data = await _account.GetUserAsync(userId);
            return Ok(data);
        }

        [HttpPost("identity/change-role")]
        public async Task<ActionResult<GeneralResponse>> ChangeUserRole(ChangeUserRoleDTO model)
            => Ok(await _account.ChangeUserRoleAsync(model));
    }
}
