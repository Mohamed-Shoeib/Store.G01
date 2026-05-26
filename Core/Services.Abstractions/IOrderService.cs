using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        // Get Order by Id
        Task<OrderResultDto> GetOrderByIdAsync(Guid orderId);

        // Get All Orders for a User
        Task<IEnumerable<OrderResultDto>> GetOrdersByUserEmailAsync(string userEmail);

        // Create Order
        Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequestDto, string UserEmail);

        // Get Delivery Methods
        Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodsAsync();

    }
}
