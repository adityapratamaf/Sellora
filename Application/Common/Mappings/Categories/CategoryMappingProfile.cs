using AutoMapper;
using Domain.Entities.Categories;
using Shared.DTO.Categories;
using CategoryEntity = Domain.Entities.Categories.Category;

namespace Application.Common.Mappings.Categories;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CategoryEntity, CategoryResponse>();
        CreateMap<CategoryCreateRequest, Category>();
        CreateMap<CategoryUpdateRequest, Category>();
    }
}
