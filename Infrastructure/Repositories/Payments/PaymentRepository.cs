using Domain.Entities.Payments;
using Domain.Interfaces.Payments;
using Infrastructure.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Payments
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(
            AppDbContext context,
            ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<Payment> GetAllAsQueryable()
        {
            return _context.Payments.AsNoTracking();
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await _context.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> UpdateAsync(Payment payment)
        {
            var existing = await _context.Payments
                .FirstOrDefaultAsync(x => x.Id == payment.Id);

            if (existing is null) return false;
        
            existing.Name = payment.Name;
            existing.ImageLogo = payment.ImageLogo;
            existing.IsActive = payment.IsActive;
            existing.UpdatedAt = payment.UpdatedAt;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Payments
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing is null) return false;

            _context.Payments.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}