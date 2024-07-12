using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.SellerEntity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SellerRepository(AppDbContext _context, IMapper _mapper) : ISellerRepository
    {
        public async Task<SellerResponse> CreateSellerAccountAsync(SellerAccountDTO model)
        {
            var map = _mapper.Map<SellerAccount>(model);

            var userId = model.SellerId;
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                return new SellerResponse(false, "User not found");

            if (model == null)
                return new SellerResponse(false, "Can not inster null values");

            var acc = new SellerAccount()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Description = model.Description,
                DateCreated = DateTime.Now,
                GrossSales = 0,
                Revenue = 0,
                SellerId = userId,                
            };

            _context.SellerAccounts.Add(acc);

            await _context.SaveChangesAsync();

            return new SellerResponse(true, "Seller account request sent!");
        }
        public async Task<SellerAccountDTO> GetSellerAccountAsync(string userId)
        {            
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                Console.WriteLine("User not found");

            var getSellerAccount = await _context.SellerAccounts
                .FirstOrDefaultAsync(o => o.SellerId == userId);

            var sellerAccountDTO = _mapper.Map<SellerAccountDTO>(getSellerAccount);

            return sellerAccountDTO;
        }


        private async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
