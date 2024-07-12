using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILocationRepository
    {
        Task<GeneralResponse> AddLocationAsync(LocationDTO model);
        Task<GeneralResponse> UpdateLocationAsync(LocationDTO model);
    }
}
