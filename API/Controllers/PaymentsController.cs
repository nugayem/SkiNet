using System.IO;
using System.Threading.Tasks;
using API.Error;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Order= Core.Entities.OrderAggregate.Order;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentServive;
        private readonly ILogger<PaymentsController> _logger;
        private readonly string _whSecret;
        public PaymentsController(IPaymentService paymentServive, ILogger<PaymentsController> logger, IConfiguration config)
        {
            this._paymentServive = paymentServive;
            this._logger = logger;
            this._whSecret=config.GetSection("WhSecret").Value;
        }
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket= await _paymentServive.CreateOrUpdatePaymentIntent(basketId);

            if(basket ==null) return BadRequest(new ApiResponse(400,"Problem with your basket" ));
            return basket;
        }
        
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json= await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent= EventUtility.ConstructEvent(json, Request.Headers["stripe-Signature"], _whSecret);

            PaymentIntent intent;
            Order order;
            
            switch(stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent) stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded: ", intent.Id);
                    order =  await _paymentServive.UpdateOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Order Updated to payment Received: ", order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent) stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed: ", intent.Id);                    
                    order =  await _paymentServive.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Payment Failed: ", order.Id);
                    break;
            }

            return new EmptyResult();

        }
    }
}