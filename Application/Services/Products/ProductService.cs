using AutoMapper;
using Domain.Interfaces.Categories;
using Domain.Interfaces.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Common.Querying;
using Shared.DTO.Products;

namespace Application.Services.Products
{
    public interface IProductService
    {
        Task<PaginatedResponse<ProductResponse>> GetAllItems(
            int offset,
            int limit,
            string strQueryParam);
        Task<PaginatedResponse<ProductResponse>> GetItemDetailById(Guid id);
        Task<ProductResponse> CreateAsync(ProductCreateRequest request);
        Task<bool> UpdateAsync(Guid id, ProductUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<ProductService> _log;
        
        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _log = logger;
        }

        /// <summary>
        /// Retrieves paginated product list with optional search by name.
        /// </summary>
        public async Task<PaginatedResponse<ProductResponse>> GetAllItems(
            int offset, 
            int limit, 
            string strQueryParam)
        {
            // 1. Ambil IQueryable<Entity>
            var entityQuery = _productRepository.GetAllAsQueryable();

            // 2. Filtering
            if (!string.IsNullOrWhiteSpace(strQueryParam))
            {
                var searchPattern = $"%{strQueryParam}%".ToLower();
                entityQuery = entityQuery.Where(x =>
                    EF.Functions.Like(x.Name.ToLower(), searchPattern) ||
                    EF.Functions.Like(x.Category.Name.ToLower(), searchPattern)
                );
            }

            // 3. Project ke DTO
            var dtoQuery = _mapper.ProjectTo<ProductResponse>(entityQuery);

            // 4. Total item
            int totalItems = await dtoQuery.CountAsync();

            // 5. Pagination item
            var pageNumber = offset > 0 ? offset : 1;
            var pageSize = limit > 0 ? limit : 10;

            // 6. Ambil data
            var pagedData = await dtoQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            _log.LogInformation(
                "Retrieved {Count} product (Page {Page})",
                pagedData.Count,
                pageNumber);

            // 7. Return response
            return new PaginatedResponse<ProductResponse>(pagedData, totalItems, pageNumber, pageSize);
        }

        /// <summary>
        /// Retrieves product detail by ID.
        /// </summary>
        public async Task<PaginatedResponse<ProductResponse>> GetItemDetailById(Guid id)
        {
            // 1. Ambil IQueryable<Entity>
            var entityQuery = _productRepository.GetAllAsQueryable()
                .Where(x => x.Id == id);

            // 2. Project ke DTO
            var dtoQuery = _mapper.ProjectTo<ProductResponse>(entityQuery);

            // 3. Totam item
            int totalItems = await dtoQuery.CountAsync();

            var pagedData = await dtoQuery
                .Skip(0)
                .Take(1)
                .ToListAsync();
            
            _log.LogInformation("Get Product Detail Id: {Id}", id);

            return new PaginatedResponse<ProductResponse>(pagedData, totalItems, 1, 1);
        }

        public async Task<ProductResponse> CreateAsync(ProductCreateRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category is null)
                throw new Exception("Category Not Found");

            var entity = new Domain.Entities.Products.Product
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description,
                ImageProduct = request.ImageProduct,
                Price = request.Price,
                Stock = request.Stock,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _productRepository.CreateAsync(entity);

            _log.LogInformation("Product created: {Id}", created.Id);

            // Reload with category to ensure CategoryName mapping works
            var createdWithCategory = await _productRepository.GetByIdAsync(created.Id);

            return _mapper.Map<ProductResponse>(createdWithCategory!);
        }

        public async Task<bool> UpdateAsync(Guid id, ProductUpdateRequest request)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing is null) return false;

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category is null)
                throw new Exception("Category not found");

            existing.CategoryId = request.CategoryId;
            existing.Name = request.Name;
            existing.Description = request.Description;
            existing.ImageProduct = request.ImageProduct;
            existing.Price = request.Price;
            existing.Stock = request.Stock;
            existing.IsActive = request.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _productRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _productRepository.DeleteAsync(id);

            _log.LogInformation("Product Delete : {Id}", id);

            return result;
        }
    }
}
