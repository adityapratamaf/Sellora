using AutoMapper;
using Shared.DTO.Products;
using ProductEntity = Domain.Entities.Products.Product;

namespace Application.Common.Mappings.Products;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<ProductEntity, ProductResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        
        CreateMap<ProductCreateRequest, ProductEntity>();
        
        CreateMap<ProductUpdateRequest, ProductEntity>();
    }
}
