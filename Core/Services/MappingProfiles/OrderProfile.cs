using AutoMapper;
using Domain.Models.OrderModels;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Address,AddressDto>().ReverseMap();

            CreateMap<OrderItem,OrderItemDto>()
                .ForMember(d => d.ProductId, d => d.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, d => d.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, d => d.MapFrom(s => s.Product.PictureUrl));

            CreateMap<Order, OrderResultDto>()
                .ForMember(d => d.PaymentStatus, d => d.MapFrom(s => s.PaymentStatus.ToString()))
                .ForMember(d => d.DeliveryMethod, d => d.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.Total, d => d.MapFrom(s => s.SubTotal + s.DeliveryMethod.Cost));

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
    }
}
