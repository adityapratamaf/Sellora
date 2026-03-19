using AutoMapper;
using Domain.Entities.Orders;
using Shared.DTO.Orders;

namespace Application.Common.Mappings.Orders;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderResponse>();
        CreateMap<OrderItem, OrderItemResponse>();
    }
}
