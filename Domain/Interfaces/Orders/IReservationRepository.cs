using Domain.Entities.Orders;

namespace Domain.Interfaces.Orders;

public interface IReservationRepository
{
    Task<int> GetReservedQtyAsync(Guid productId, DateTime nowUtc);
    Task CreateManyAsync(IEnumerable<StockReservation> reservations);
    Task<List<StockReservation>> GetByOrderIdAsync(Guid orderId);
    Task ReleaseByOrderIdAsync(Guid orderId);
    Task CleanupExpiredAsync(DateTime nowUtc);
}
