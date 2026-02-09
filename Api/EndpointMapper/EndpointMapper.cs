using Api.Endpoints.Users;
using Api.Endpoints.Categories;
using Api.Endpoints.Products;
using Api.Endpoints.Payments;
using Api.Endpoints.Carts;

namespace Api.EndpointMapper
{
    public static class EndpointMapper
    {
        public static WebApplication MapAllEndpoints(this WebApplication app)
        {
            // Master Data
            app.MapUserEndpoints();
            app.MapCategoryEndpoints();
            app.MapProductEndpoints();
            app.MapPaymentEndpoints();
            app.MapCartEndpoints();

            return app;
        }
    }
}
