namespace BookShop.Api.Infrastructure.Extensions
{
    using BookShop.Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Migrate Database
                serviceScope.ServiceProvider.GetService<BookShopDbContext>()
                    .Database
                    .Migrate();
            }

            return app;
        }
    }
}
