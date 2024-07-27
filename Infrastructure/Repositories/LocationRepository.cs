using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.UserEntity;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LocationRepository(AppDbContext _context , IMapper _mapper) : ILocationRepository
    {
        public async Task<GeneralResponse> AddLocationAsync(LocationDTO model)
        {
            var map = _mapper.Map<LocationDTO>(model);
            
            var userId = model.ApplicationUserId;
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new GeneralResponse(false, "User not found");

            if (model == null)
                return new GeneralResponse(false, "Can not inster null values");

            var location = new Location()
            {
                LocationId = Guid.NewGuid().ToString(),
                PhoneNumber = model.PhoneNumber,
                Country = model.Country,
                Street  = model.Street,
                Building = model.Building,
                Floor = model.Floor,
                ApplicationUserId = model.ApplicationUserId,
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return new GeneralResponse(true, "Location added successfully!");

        }
        public async Task<GeneralResponse> UpdateLocationAsync(LocationDTO model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.ApplicationUserId))
                    return new GeneralResponse(false, "Can not insert null values");

                var location = await _context.Locations.FindAsync(model.LocationId);
                if(location == null)
                    return new GeneralResponse(false , "Location not found");

                _mapper.Map(model, location);

                _context.Locations.Update(location);
                await _context.SaveChangesAsync();
                return new GeneralResponse(true, "Location changed successfully");
            }
            catch (Exception ex)
            {
                return new GeneralResponse(false, $"Error : {ex.Message} ");
            }
        }
    }
}
