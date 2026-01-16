using AutoMapper;
using Domain.Interfaces.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Common.Querying;
using Shared.DTO.Categories;

namespace Application.Services.Categories
{
    // INTERFACE DI FILE YANG SAMA
    public interface ICategoryService
    {
        Task<PaginatedResponse<CategoryResponse>> GetAllItems(
            int offset,
            int limit,
            string strQueryParam);
        Task<PaginatedResponse<CategoryResponse>> GetItemDetailById(Guid id);
        Task<CategoryResponse> CreateAsync(CategoryCreateRequest request);
        Task<bool> UpdateAsync(Guid id, CategoryUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    // IMPLEMENTATION
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _log;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _log = logger;
        }

        /// <summary>
        /// Retrieves paginated category list with optional search by name.
        /// </summary>
        public async Task<PaginatedResponse<CategoryResponse>> GetAllItems(
            int offset,
            int limit,
            string strQueryParam)
        {
            // 1. Ambil IQueryable<Entity>
            var entityQuery = _categoryRepository.GetAllAsQueryable();

            // 2. Filtering
            if (!string.IsNullOrWhiteSpace(strQueryParam))
            {
                entityQuery = entityQuery
                    .Where(x =>
                            (x.Name != null && x.Name.ToUpper().Contains(strQueryParam.ToUpper())) ||
                            (x.Id != Guid.Empty && x.Id.ToString().ToUpper().Contains(strQueryParam.ToUpper()))
                        );
            }

            // 3. Project ke DTO
            var dtoQuery = _mapper.ProjectTo<CategoryResponse>(entityQuery);

            // 4. Total item
            int totalItems = await dtoQuery.CountAsync();

            // 5. Pagination default
            int pageNumber = offset > 0 ? offset : 1;
            int pageSize = limit > 0 ? limit : 10;

            // 6. Ambil data
            var pagedData = await dtoQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _log.LogInformation(
                "Retrieved {Count} categories (Page {Page})",
                pagedData.Count,
                pageNumber);

            // 7. Return response
            return new PaginatedResponse<CategoryResponse>(
                pagedData,
                totalItems,
                pageNumber,
                pageSize);
        }

        /// <summary>
        /// Retrieves category detail by ID.
        /// </summary>
        public async Task<PaginatedResponse<CategoryResponse>> GetItemDetailById(Guid id)
        {
            // 1. Ambil IQueryable<Entity>
            var entityQuery = _categoryRepository.GetAllAsQueryable()
                .Where(x => x.Id == id);

            // 2. Project ke DTO
            var dtoQuery = _mapper.ProjectTo<CategoryResponse>(entityQuery);

            int totalItems = await dtoQuery.CountAsync();

            var pagedData = await dtoQuery
                .Take(1)
                .ToListAsync();

            _log.LogInformation("Get Category Detail ID: {Id}", id);

            return new PaginatedResponse<CategoryResponse>(
                pagedData,
                totalItems,
                1,
                1);
        }

        public async Task<CategoryResponse> CreateAsync(CategoryCreateRequest request)
        {
            var entity = new Domain.Entities.Categories.Category
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _categoryRepository.CreateAsync(entity);

            _log.LogInformation("Category created: {Id}", created.Id);

            return _mapper.Map<CategoryResponse>(created);
        }

        public async Task<bool> UpdateAsync(Guid id, CategoryUpdateRequest request)
        {
            var entity = new Domain.Entities.Categories.Category
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _categoryRepository.UpdateAsync(entity);

            _log.LogInformation("Category updated: {Id}", id);

            return result;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _categoryRepository.DeleteAsync(id);

            _log.LogInformation("Category deleted: {Id}", id);

            return result;
        }
    }
}
