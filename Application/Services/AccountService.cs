﻿using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.Extensions;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AccountService(HttpClientService httpClientService, HttpClient _httpClient) : IAccountRepository
    {
        public async Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleDTO model)
        {
            try
            {
                var publicClient = await httpClientService.GetPrivateClient();
                var response = await publicClient.PostAsJsonAsync(Constants.ChangeUserRoleRoute, model);
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    return new GeneralResponse(false, error);

                var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return result!;
            }
            catch (Exception ex) { return new GeneralResponse(false, ex.Message); }
        }

        public async Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constants.RegisterRoute, model);
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    return new GeneralResponse(Flag: false, Message: error);

                var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return result!;
            }
            catch (Exception ex) { return new GeneralResponse(Flag: false, Message: ex.Message); }
        }

        public Task<GeneralResponse> CreateRoleAsync(CreateRoleDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constants.LoginRoute, model);
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    return new LoginResponse(Flag: false, Message: error);

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!;
            }
            catch (Exception ex) { return new LoginResponse(Flag: false, Message: ex.Message); }
        }        

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constants.RefreshTokenRoute, model);
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    return new LoginResponse(false, error);

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!;
            }
            catch (Exception ex) { return new LoginResponse(false, ex.Message); }
        }

        private static string CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return $"sorry unkown error occured.{Environment.NewLine} Error Description : {Environment.NewLine} Status Code : {response.StatusCode}{Environment.NewLine} Reason Phrase : {response.ReasonPhrase}";
            else
                return null;
        }

        public async Task<IEnumerable<GetUserDTO>> GetUserAsync(string userId)
        {
            try
            {
                var privateClient = await httpClientService.GetPrivateClient();
                var response = await privateClient.GetAsync($"api/account/get-user/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    // Log the status code or error for debugging purposes
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP Error: {response.StatusCode}, Details: {error}");
                }

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetUserDTO>>();
                return result ?? Enumerable.Empty<GetUserDTO>();
            }
            catch (Exception ex)
            {
                // Log the exception (if you have a logging mechanism)
                throw new Exception($"Error fetching user details: {ex.Message}");
            }
        }

        public async Task<LoginResponse> LoginSellerAccountAsync(LoginDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constants.LoginSellerRoute, model);
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    return new LoginResponse(Flag: false, Message: error);

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!;
            }
            catch (Exception ex) { return new LoginResponse(Flag: false, Message: ex.Message); }
        }

        public async Task<LoginResponse> LoginAdminAccountAsync(LoginDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constants.LoginAdminRoute, model);
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    return new LoginResponse(Flag: false, Message: error);

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!;
            }
            catch (Exception ex) { return new LoginResponse(Flag: false, Message: ex.Message); }
        }
    }
}
