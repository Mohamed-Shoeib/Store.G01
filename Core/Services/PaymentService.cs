using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.OrderModels;
using Microsoft.Extensions.Configuration;
using Services.Abstractions;
using Shared;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProduct = Domain.Models.Product;

namespace Services
{
    public class PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,IMapper mapper,IConfiguration configuration) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
           var basket = await basketRepository.GetBasketAsync(basketId);
            if (basket == null)
                throw new BasketNotFoundException(basketId);

            foreach (var item in basket.Items)
            {
               var product = await unitOfWork.GetRepository<OrderProduct, int>().GetAsync(item.Id);
               if(product == null) 
                    throw new ProductNotFoundException(item.Id);

               item.Price = product.Price;
            }

            if(!basket.DeliveryMethodId.HasValue)
                throw new Exception("Invaild Delivery Method Id ");

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod,int>().GetAsync(basket.DeliveryMethodId.Value);
            if(deliveryMethod == null)
                throw new DeliveryNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingPrice = deliveryMethod.Cost;

            // Amount 
            var amount = (long) (basket.Items.Sum(s => s.Price * s.Quantity) + basket.ShippingPrice) * 100;

            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create
                var CreateOptions = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                var paymentIntent = await service.CreateAsync(CreateOptions);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                // Update
                var UpdateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = amount,
                };
                await service.UpdateAsync(basket.PaymentIntentId, UpdateOptions);
                
            }
            await basketRepository.UpdateBasketAsync(basket);
            var result = mapper.Map<BasketDto>(basket);
            return result;
        }
    }
}
