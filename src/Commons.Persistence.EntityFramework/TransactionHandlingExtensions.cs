using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Queo.Commons.Persistence.EntityFramework.TransactionManager;
using Queo.Commons.Persistence.Filter;
using Queo.Commons.Persistence.TransactionManager;


namespace Queo.Commons.Persistence.EntityFramework
{
    public static class TransactionHandlingExtensions
    {
        public static void UseTransactionHandling<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped<ITransactionManager, GenericTransactionManager<TDbContext>>();

            // Add TransactionCommitFilter
            services.AddMvc(options =>
            {
                options.Filters.Add<TransactionCommitFilter>();
            });

            // Add TransactionFilter
            services.AddMvc(options =>
            {
                options.Filters.Add<TransactionFilter>();
            });

            // Add TransactionRollbackOnExceptionFilter
            services.AddMvc(options =>
            {
                options.Filters.Add<TransactionRollbackOnExceptionFilter>();
            });
        }
    }
}
