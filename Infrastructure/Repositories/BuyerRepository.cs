using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models.UserEntity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BuyerRepository(AppDbContext _context) : IBuyerRepository
    {
        public async Task<BuyerResponse> AddProductToCartAsync(string productId, string userId)
        {
            int Quantity = 1;

            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                return new BuyerResponse(false, "Product not found");
            
            if (product.Quantity <= 0)
                return new BuyerResponse(false, "Product is out of stock");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new BuyerResponse(false, "User not found");

            var userCart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(u => u.UserId == userId);
            if (userCart == null)
                return new BuyerResponse(false, "Cart not found");

            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == product.Id && ci.CartId == user.CartId);

            if(existingCartItem != null)
            {
                if (existingCartItem.Quantity + Quantity > product.Quantity)
                    return new BuyerResponse(false, "Cannot add more items than available in stock");

                existingCartItem.Quantity += Quantity;
                existingCartItem.Total = existingCartItem.Quantity * existingCartItem.ProductPrice;

                userCart.CartTotal += (decimal)(Quantity * existingCartItem.ProductPrice);

                await _context.SaveChangesAsync();
                return new BuyerResponse(Flag: true, Message: $"{product.Name} quantity increased to {existingCartItem.Quantity}");
            }
            else
            {
                if (Quantity > product.Quantity)
                    return new BuyerResponse(false, "Cannot add more items than available in stock");

                var cartitem = new CartItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    CartId = user.CartId,
                    ProductId = productId,
                    ProductName = product.Name,
                    ProductPrice = (decimal)product.SellingPrice,
                    Quantity = Quantity,
                    Total = (decimal)(Quantity * product.SellingPrice),
                    CoverImageUrl = product.Images.FirstOrDefault()?.Url
                };

                userCart.CartTotal += (decimal)cartitem.Total;

                await _context.CartItems.AddAsync(cartitem);

            }

            await _context.SaveChangesAsync();
            return new BuyerResponse(true, "Product added to the cart!");
        }

        public async Task<BuyerResponse> RemoveProductFromCartAsync(string productId, string userId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return new BuyerResponse(false, "Product not found");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new BuyerResponse(false, "User not found");


            var userCart = await _context.Carts
                .Include(p => p.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (userCart == null)
                return new BuyerResponse(false, "Cart not found");

            var itemToRemove = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == product.Id && ci.CartId == user.CartId);

            if (itemToRemove == null)
                return new BuyerResponse(false, "Product not found");

            if (itemToRemove.Quantity > 1)
            {
                itemToRemove.Quantity--;
                itemToRemove.Total = itemToRemove.Total - itemToRemove.ProductPrice;
                userCart.CartTotal = (decimal)itemToRemove.Total;
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.CartItems.Remove(itemToRemove);
            }

            await _context.SaveChangesAsync();
            return new BuyerResponse(true, "The product have been removed from the cart!");

        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            return await _context.Carts
                .Include(p => p.CartOwner)
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
        {
            var getUser = await _context.Users.FindAsync(userId);
            if (getUser == null)
            {
                Console.WriteLine("User not found");
                return Enumerable.Empty<CartItem>();
            }


            var userCartItems = await _context.CartItems
             .Where(ci => ci.CartId == getUser.CartId)             
             .ToListAsync();

            if (userCartItems == null || !userCartItems.Any())
            {
                Console.WriteLine("User's cart not found");
                return Enumerable.Empty<CartItem>();
            }

            return userCartItems;
        }

        
    }
}
