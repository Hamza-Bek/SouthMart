using Application.DTOs.Request.Account;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Extensions
{
    public class CustomHttpHandler(LocalStorageService localStorageService,
       HttpClientService httpClientService,
       NavigationManager navigationManager, IAccountRepository accountService) : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                bool loginURL = request.RequestUri!.AbsoluteUri.Contains(Constants.LoginRoute);
                bool registerURL = request.RequestUri!.AbsoluteUri.Contains(Constants.RegisterRoute);
                bool refreshTokenURL = request.RequestUri!.AbsoluteUri.Contains(Constants.RefreshTokenRoute);                
                if (loginURL || registerURL || refreshTokenURL )
                    return await base.SendAsync(request, cancellationToken);

                var result = await base.SendAsync(request, cancellationToken);
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //Get Token from localStorage
                    var tokenModel = await localStorageService.GetModelFromToken();
                    if (tokenModel == null) return result;

                    //Call for refresh token
                    var newJwtToken = await GetRefreshToken(tokenModel.Refresh!);
                    if (string.IsNullOrEmpty(newJwtToken)) return result;

                    request.Headers.Authorization = new AuthenticationHeaderValue(Constants.HttpClientHeaderScheme, newJwtToken);
                    return await base.SendAsync(request, cancellationToken);
                }
                return result;
            }
            catch { return null!; }
        }
        private async Task<string> GetRefreshToken(string refreshToken)
        {
            try
            {
                var client = httpClientService.GetPublicClient();
                var response = await accountService.RefreshTokenAsync(new RefreshTokenDTO() { Token = refreshToken });
                if (response == null || response.Token == null)
                {
                    await ClearBrowserStorage();
                    NavigateToLogin();
                    return null!;
                }
                await localStorageService.RemoveTokenFromBrowserLocalStorage();
                await localStorageService.SetBrowserLocalStorage(new LocalStorageDTO() { Refresh = response!.RefreshToken, Token = response.Token });
                return response.Token;
            }
            catch
            {
                return null!;
            }
        }
        private void NavigateToLogin() => navigationManager.NavigateTo(navigationManager.BaseUri, true, true);
        private async Task ClearBrowserStorage() => await localStorageService.RemoveTokenFromBrowserLocalStorage();
    }
}
