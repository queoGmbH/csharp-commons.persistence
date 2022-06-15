using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Queo.Commons.Persistence.EntityFramework
{
    /// <summary>
    /// Startet eine Transaktion und speichert am Ende die Daten in der DB, wenn keine Exception aufgetreten ist bzw.
    /// das Rollback-Flag nicht gesetzt ist.
    /// Siehe auch: https://wiki.queo-group.com/display/QUEOTCSHARP/Rumpf+-+Features
    /// </summary>
    public class TransactionMiddleware
    {
        public static string QueoRollbackTransaction = "queoRollbackTransaction";
        private readonly RequestDelegate _next;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DbContext dbContext)
        {
            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (!context.Items.ContainsKey(QueoRollbackTransaction))
                    {
                        context.Items.Add(QueoRollbackTransaction, false);
                    }
                    await _next(context);
                    if (ShouldRollbackTransaction(context))
                    {
                        transaction.Rollback();
                    }
                    else
                    {
                        dbContext.SaveChanges();
                        transaction.Commit();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private bool ShouldRollbackTransaction(HttpContext context)
        {
            if (context.Items.ContainsKey(QueoRollbackTransaction))
            {
                return (bool)context.Items[QueoRollbackTransaction];
            }
            else
            {
                // Wenn es den Eintrag nicht gibt, ist irgendetwas schief gegangen. Dann nichts speichern.
                return true;
            }
        }
    }

    public static class TransactionMiddlewareExtensions
    {
        /// <summary>
        /// Fügt die Middleware für das Transaktionshandling hinzu.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTransactionMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<TransactionMiddleware>();
        }
    }
}
