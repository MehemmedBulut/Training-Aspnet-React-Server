﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Drawing;
using System.Net;
using TrainPro.Data;
using TrainPro.Models;

namespace TrainPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;

        public PaymentController(IConfiguration configuration, ApplicationDbContext db)
        {
            _response = new();
            _configuration = configuration;
            _db = db;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment(string userId)
        {
            ShoppingCart shoppingCart = _db.ShoppingCarts
                .Include(u => u.CartItems).ThenInclude(u => u.MenuItem)
                .FirstOrDefault(u => u.UserId == userId);
            if(shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count() == 0) 
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSsuccess= false;
                return BadRequest(_response);
            }

            #region Create Payment Intent
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);
            PaymentIntentCreateOptions options = new()
            {
                Amount = (int)(shoppingCart.CartTotal * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string>
                {
                    "card",
                }
            };
            PaymentIntentService service = new ();
            PaymentIntent response = service.Create(options);
            shoppingCart.StripePaymentIntentId = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;
            #endregion

            _response.Result = shoppingCart;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

    }
}
