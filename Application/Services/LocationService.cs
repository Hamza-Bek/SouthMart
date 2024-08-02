using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services
{
    public class LocationService(HttpClient _httpClient) : ILocationRepository
    {
        public async Task<GeneralResponse> AddLocationAsync(LocationDTO model)
        {
            try
            {
                var data = await _httpClient.PostAsJsonAsync("api/location/add-location", model);
                string error = CheckResponseStatus(data);
                var response = await data.Content.ReadFromJsonAsync<GeneralResponse>();
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                if (response.Flag)
                {
                    return new GeneralResponse(Flag: true, Message: "Location added successfully.");
                }
                else
                {
                    return new GeneralResponse(Flag: false, Message: "Something went wrong while trying to add user's location");
                }
            }
            catch
            {
                return new GeneralResponse(Flag: false, Message: "Something went wrong!");
            }
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

        public async Task<GeneralResponse> UpdateLocationAsync(LocationDTO model)
        {
            try
            {
                var data = await _httpClient.PutAsJsonAsync("api/locations/edit-location", model);
                if (!data.IsSuccessStatusCode)
                {
                    string responseContent = await data.Content.ReadAsStringAsync();
                    return new GeneralResponse(Flag: false, Message: $"Failed to update location. StatusCode: {data.StatusCode}, Response: {responseContent}");
                }

                var response = await data.Content.ReadFromJsonAsync<GeneralResponse>();
                if (response == null)
                {
                    return new GeneralResponse(Flag: false, Message: "Invalid response received from server.");
                }

                if (response.Flag)
                {
                    return new GeneralResponse(Flag: true, Message: "Location updated successfully.");
                }
                else
                {
                    return new GeneralResponse(Flag: false, Message: "Something went wrong while trying to update the location.");
                }
            }
            catch (Exception ex)
            {
                return new GeneralResponse(Flag: false, Message: $"Exception: {ex.Message}");
            }
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
