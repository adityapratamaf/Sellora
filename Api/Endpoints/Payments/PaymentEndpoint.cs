using Application.Services.Payments;
using Shared.DTO.Payments;

namespace Api.Endpoints.Payments
{
    public static class PaymentEndpoints
    {
        public static void MapPaymentEndpoints(this WebApplication app)
        {
            var paymentGroup = app.MapGroup("/api/payments").WithTags("Payments");

            // GET: /api/payments
            paymentGroup.MapGet("/", async (IPaymentService paymentService, int offset = 1, int limit = 10, string strQueryParam = "") =>
            {
                var result = await paymentService.GetAllItems(offset, limit, strQueryParam);
                return Results.Ok(result);
            });

            // GET: /api/payments/{id}
            paymentGroup.MapGet("/{strUUID}", async (IPaymentService paymentService, Guid id) =>
            {
                var result = await paymentService.GetItemDetailById(id);
                return Results.Ok(result);
            });

            // POST: /api/payments
            paymentGroup.MapPost("/", async (PaymentCreateRequest request, IPaymentService service) =>
            {
                var result = await service.CreateAsync(request);
                return Results.Created($"/api/payments/{result.Id}", result);
            });

            // PUT: /api/payments/{id}
            paymentGroup.MapPut("/{id:guid}", async (Guid id, PaymentUpdateRequest request, IPaymentService service) =>
            {
                var updated = await service.UpdateAsync(id, request);
                return Results.Ok(updated);
            });

            // DELETE: /api/payments/{id}
            paymentGroup.MapDelete("/{id:guid}", async (IPaymentService paymentService, Guid id) =>
            {
                var deleted = await paymentService.DeleteAsync(id);
                return Results.Ok(deleted);
            });
        }
    }
}
