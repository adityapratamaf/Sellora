using AutoMapper;
using Domain.Entities.Payments;
using Domain.Interfaces.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Common.Querying;
using Shared.DTO.Payments;

namespace Application.Services.Payments
{
    // INTERFACE DI FILE YANG SAMA
    public interface IPaymentService
    {
        Task<PaginatedResponse<PaymentResponse>> GetAllItems(
            int offset,
            int limit,
            string strQueryParam);
        Task<PaginatedResponse<PaymentResponse>> GetItemDetailById(Guid id);
        Task<PaymentResponse> CreateAsync(PaymentCreateRequest request);
        Task<bool> UpdateAsync(Guid id, PaymentUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _log;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IMapper mapper,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _log = logger;
        }

        /// <summary>
        /// Retrieves paginated payment list with optional search by name.
        /// </summary>
        public async Task<PaginatedResponse<PaymentResponse>> GetAllItems(
            int offset,
            int limit,
            string strQueryParam)
        {
            // 1. Ambil IQueryable<Entity>
            var entityQuery = _paymentRepository.GetAllAsQueryable();

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
            var dtoQuery = _mapper.ProjectTo<PaymentResponse>(entityQuery);

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
                "Retrieved {Count} payment (Page {Page})",
                pagedData.Count,
                pageNumber);

            // 7. Return response
            return new PaginatedResponse<PaymentResponse>(
                pagedData,
                totalItems,
                pageNumber,
                pageSize);
        }

        /// <summary>
        /// Retrieves payment detail by ID.
        /// </summary>
        public async Task<PaginatedResponse<PaymentResponse>> GetItemDetailById(Guid id)
        {
            // 1. Ambil IQueryable<Entity>
            var entityQuery = _paymentRepository.GetAllAsQueryable()
                .Where(x => x.Id == id);

            // 2. Project ke DTO
            var dtoQuery = _mapper.ProjectTo<PaymentResponse>(entityQuery);

            int totalItems = await dtoQuery.CountAsync();

            var pagedData = await dtoQuery
                .Take(1)
                .ToListAsync();

            _log.LogInformation("Get Payment Detail ID: {Id}", id);

            return new PaginatedResponse<PaymentResponse>(
                pagedData,
                totalItems,
                1,
                1);
        }

        public async Task<PaymentResponse> CreateAsync(PaymentCreateRequest request)
        {
            var entity = new Domain.Entities.Payments.Payment
            {
                Name = request.Name,
                ImageLogo = request.ImageLogo,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _paymentRepository.CreateAsync(entity);

            _log.LogInformation("Payment created: {Id}", created.Id);

            return _mapper.Map<PaymentResponse>(created);
        }

        public async Task<bool> UpdateAsync(Guid id, PaymentUpdateRequest request)
        {
            var entity = new Domain.Entities.Payments.Payment
            {
                Id = id,
                Name = request.Name,
                ImageLogo = request.ImageLogo,
                IsActive = request.IsActive,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _paymentRepository.UpdateAsync(entity);

            _log.LogInformation("Payment updated: {Id}", id);

            return result;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _paymentRepository.DeleteAsync(id);

            _log.LogInformation("Payment deleted: {Id}", id);

            return result;
        }

    }

}