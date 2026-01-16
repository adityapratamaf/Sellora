using Domain.Entities.Payments;

namespace Domain.Interfaces.Payments;

public interface IPaymentRepository
{
    IQueryable<Payment> GetAllAsQueryable();
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<Payment?> GetByIdAsync(Guid id);
    Task<Payment> CreateAsync(Payment payment);
    Task<bool> UpdateAsync(Payment payment);
    Task<bool> DeleteAsync(Guid id);
}
