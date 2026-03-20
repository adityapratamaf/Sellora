using Domain.Entities.Orders;

namespace Domain.Interfaces.Orders;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order> CreateAsync(Order order);
    Task<bool> UpdateAsync(Order order);
    Task<Order?> GetByIdWithItemsAsync(Guid id);
    Task<Order?> GetByIdWithItemsAndPaymentAsNoTrackingAsync(Guid id);
}
