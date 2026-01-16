using AutoMapper;
using Domain.Interfaces.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Common.Querying;
using Shared.DTO.Users;

namespace Application.Services.User
{
    public interface IUserService
    {
        Task<PaginatedResponse<UserResponseRequest>> GetAllItems(int offset, int limit, string strQueryParam);
        Task<PaginatedResponse<UserResponseRequest>> GetItemDetailById(Guid id);

        Task<UserResponseRequest> CreateAsync(UserCreateRequest request);
        Task<bool> UpdateAsync(Guid id, UserUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _log;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _log = logger;
        }

        public async Task<PaginatedResponse<UserResponseRequest>> GetAllItems(int offset, int limit, string strQueryParam)
        {
            var entityQuery = _userRepository.GetAllAsQueryable();

            if (!string.IsNullOrWhiteSpace(strQueryParam))
            {
                var searchPattern = $"%{strQueryParam}%".ToLower();

                entityQuery = entityQuery.Where(x =>
                    EF.Functions.Like(x.Name.ToLower(), searchPattern) ||
                    EF.Functions.Like(x.Username.ToLower(), searchPattern) ||
                    EF.Functions.Like(x.Email.ToLower(), searchPattern) ||
                    EF.Functions.Like(x.Role.ToLower(), searchPattern) ||
                    EF.Functions.Like(x.Phone.ToLower(), searchPattern)
                );
            }

            var dtoQuery = _mapper.ProjectTo<UserResponseRequest>(entityQuery);

            int totalItems = await dtoQuery.CountAsync();

            var pageNumber = offset > 0 ? offset : 1;
            var pageSize = limit > 0 ? limit : 10;

            var pagedData = await dtoQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            _log.LogInformation(
                "Retrieved {Count} User (Page {Page})",
                pagedData.Count,
                pageNumber);

            return new PaginatedResponse<UserResponseRequest>(pagedData, totalItems, pageNumber, pageSize);
        }

        public async Task<PaginatedResponse<UserResponseRequest>> GetItemDetailById(Guid id)
        {
            var entityQuery = _userRepository.GetAllAsQueryable()
                .Where(x => x.Id == id);

            var dtoQuery = _mapper.ProjectTo<UserResponseRequest>(entityQuery);

            int totalItems = await dtoQuery.CountAsync();

            var pagedData = await dtoQuery
                .Skip(0)
                .Take(1)
                .ToListAsync();

            _log.LogInformation("Get User Detail ID: {Id}", id);

            return new PaginatedResponse<UserResponseRequest>(
                pagedData, 
                totalItems, 
                1, 
                1);
        }

        public async Task<UserResponseRequest> CreateAsync(UserCreateRequest request)
        {
            // HASH PASSWORD HERE
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var entity = new Domain.Entities.Users.User
            {
                Name = request.Name,
                Username = request.Username,
                Email = request.Email,
                Password = hashedPassword,
                Address = request.Address,
                Phone = request.Phone,
                Role = request.Role,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _userRepository.CreateAsync(entity);

            _log.LogInformation("User created: {Id}", created.Id);

            return _mapper.Map<UserResponseRequest>(created);
        }

        public async Task<bool> UpdateAsync(Guid id, UserUpdateRequest request)
        {
            var existing = await _userRepository.GetByIdAsync(id);
            if (existing is null) return false;

            existing.Name = request.Name;
            existing.Username = request.Username;
            existing.Email = request.Email;
            existing.Address = request.Address;
            existing.Phone = request.Phone;
            existing.Role = request.Role;
            existing.IsActive = request.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            // ONLY HASH IF PASSWORD PROVIDED
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                existing.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }

            _log.LogInformation("User updated: {Id}", id);

            return await _userRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _userRepository.DeleteAsync(id);  
            
            _log.LogInformation("User deleted: {Id}", id);

            return result;
        }
    }
}
