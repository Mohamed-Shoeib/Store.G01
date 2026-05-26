using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.OrderModels;
using Services.Abstractions;
using Services.Specifications;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService(IMapper mapper,IBasketRepository basketRepository, IUnitOfWork unitOfWork) : IOrderService
    {
        public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequestDto,string userEmail)
        {
            // Address
            var address = mapper.Map<Address>(orderRequestDto.ShippingAddress);

            //OrderItems => Basket
            var basket = await basketRepository.GetBasketAsync(orderRequestDto.BasketId);
            if (basket == null) 
                throw new BasketNotFoundException(orderRequestDto.BasketId);

            var orderItems = new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>().GetAsync(item.Id);
                if (product == null)
                    throw new ProductNotFoundException(item.Id);
                var orderItem = new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl), item.Quantity, (int) product.Price);
                orderItems.Add(orderItem);
            }

            // Get Delivery Method
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequestDto.DeliveryMethodId);
            if (deliveryMethod == null)
                throw new DeliveryNotFoundException(orderRequestDto.DeliveryMethodId);

            // Get SubTotal 
            var subtotal = orderItems.Sum(i => i.Price *  i.Quantity);

            // ToDo : PaymentIntentId

            // Check Order Exists

            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);
            var ExistsOrder = await unitOfWork.GetRepository<Order, Guid>().GetAsync(spec);
            if(ExistsOrder is not null)
                unitOfWork.GetRepository<Order,Guid>().Delete(ExistsOrder);


            // Create Order
            var order = new Order(userEmail, address,orderItems,deliveryMethod,subtotal,basket.PaymentIntentId);

            await unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            var count = await unitOfWork.SaveChanges();
            if (count == 0)
                throw new OrderCreateBadRequestException();

            var result = mapper.Map<OrderResultDto>(order);
            return result;
        }
        public async Task<OrderResultDto> GetOrderByIdAsync(Guid orderId)
        {
            var spec = new OrderSpecifications(orderId); 
            var order = await unitOfWork.GetRepository<Order,Guid>().GetAsync(spec);

            if (order == null)
                throw new OrderNotFoundException(orderId);

            var result = mapper.Map<OrderResultDto>(order);
            return result;
        }
        public async Task<IEnumerable<OrderResultDto>> GetOrdersByUserEmailAsync(string userEmail)
        {
            var spec = new OrderSpecifications(userEmail);
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);

            var result = mapper.Map<IEnumerable<OrderResultDto>>(orders);
            return result;
        }
        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
            return result;
        }
    }
}
