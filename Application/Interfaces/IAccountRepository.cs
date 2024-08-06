using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<LoginResponse> LoginSellerAccountAsync(LoginDTO model);
        Task<LoginResponse> LoginAccountAsync(LoginDTO model);
        Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model);


        Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model);
        Task<GeneralResponse> CreateRoleAsync(CreateRoleDTO model);
        Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleDTO model);

        Task<IEnumerable<GetUserDTO>> GetUserAsync(string userId);
    }
}
