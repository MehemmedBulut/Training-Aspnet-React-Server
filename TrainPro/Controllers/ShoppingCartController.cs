using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TrainPro.Data;
using TrainPro.Models;

namespace TrainPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ApiResponse _response;
        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            _response = new();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, int menuItemId, int updateQuantityBy)
        {
            ShoppingCart shoppingCart = _db.ShoppingCarts.Include(u=>u.CartItems).FirstOrDefault(u => u.UserId == userId);
            MenuItem menuItem = _db.MenuItems.FirstOrDefault(u => u.Id == menuItemId);
            if(menuItem== null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSsuccess = false;
                return BadRequest(_response);
            }
            if (shoppingCart == null && updateQuantityBy > 0)
            {
                ShoppingCart newCart = new() { UserId = userId };
                _db.ShoppingCarts.Add(newCart);
                _db.SaveChanges();

                CartItem newCartItem = new()
                {
                    MenuItemId = menuItemId,
                    Quantity = updateQuantityBy,
                    ShoppingCartId = newCart.Id,
                    MenuItem = null
                };
                _db.CartItems.Add(newCartItem);
                _db.SaveChanges();
            }
            else
            {
                CartItem cartItemCart = shoppingCart.CartItems.FirstOrDefault(u => u.MenuItemId == menuItemId);
                if(cartItemCart == null)
                {
                    CartItem newCartItem = new()
                    {
                        MenuItemId = menuItemId,
                        Quantity = updateQuantityBy,
                        ShoppingCartId = shoppingCart.Id
                    };
                    _db.CartItems.Add(newCartItem);
                    _db.SaveChanges();
                }
                else
                {
                    int newQuantity = cartItemCart.Quantity + updateQuantityBy;
                    if(updateQuantityBy == 0 || newQuantity <= 0)
                    {
                        _db.CartItems.Remove(cartItemCart);
                        if(shoppingCart.CartItems.Count() == 1)
                        {
                            _db.ShoppingCarts.Remove(shoppingCart);
                        }
                        _db.SaveChanges();
                    }
                    else
                    {
                        cartItemCart.Quantity = newQuantity;
                        _db.SaveChanges();
                    }
                }
            }
            return _response;
        }
    }
}
