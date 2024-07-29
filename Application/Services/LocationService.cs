using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LocationService(HttpClient _httpClient) : ILocationRepository
    {
        public Task<GeneralResponse> AddLocationAsync(LocationDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LocationDTO>> GetLocation(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/locations/get-location/{userId}");
                string error = CheckResponseStatus(response);
                if(!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<LocationDTO>>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<GeneralResponse> UpdateLocationAsync(LocationDTO model)
        {
            throw new NotImplementedException();
        }

        private static string CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return $"sorry unkown error occured.{Environment.NewLine} Error Description : {Environment.NewLine} Status Code : {response.StatusCode}{Environment.NewLine} Reason Phrase : {response.ReasonPhrase}";
            else
                return null!;
        }
    }
}
