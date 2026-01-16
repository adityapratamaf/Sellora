using AutoMapper;
using Domain.Entities.Payments;
using Shared.DTO.Payments;
using PaymentEntity = Domain.Entities.Payments.Payment;

namespace Application.Common.Mappings.Payments;

public class PaymentMappingProile : Profile
{
    public PaymentMappingProile()
    {
        CreateMap<PaymentEntity, PaymentResponse>();
        CreateMap<PaymentCreateRequest, PaymentEntity>();
        CreateMap<PaymentUpdateRequest, PaymentEntity>();
    }
}