using Domain.Entities.Orders;
using Domain.Interfaces.Orders;
using Infrastructure.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Orders;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetReservedQtyAsync(Guid productId, DateTime nowUtc)
    {
        return await _context.StockReservations
            .AsNoTracking()
            .Where(x => x.ProductId == productId && !x.IsReleased && x.ReservedUntil > nowUtc)
            .SumAsync(x => x.Quantity);
    }

    public async Task CreateManyAsync(IEnumerable<StockReservation> reservations)
    {
        _context.StockReservations.AddRange(reservations);
        await _context.SaveChangesAsync();
    }

    public async Task<List<StockReservation>> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.StockReservations
            .Where(x => x.OrderId == orderId && !x.IsReleased)
            .ToListAsync();
    }

    public async Task ReleaseByOrderIdAsync(Guid orderId)
    {
        var rows = await _context.StockReservations
            .Where(x => x.OrderId == orderId && !x.IsReleased)
            .ToListAsync();

        foreach (var r in rows) r.IsReleased = true;

        await _context.SaveChangesAsync();
    }

    public async Task CleanupExpiredAsync(DateTime nowUtc)
    {
        var expired = await _context.StockReservations
            .Where(x => !x.IsReleased && x.ReservedUntil <= nowUtc)
            .ToListAsync();

        foreach (var r in expired) r.IsReleased = true;

        await _context.SaveChangesAsync();
    }
}
