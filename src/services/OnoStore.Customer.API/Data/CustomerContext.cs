using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnoStore.Core.Data;
using OnoStore.Core.DomainObjects;
using OnoStore.Customer.API.Models;

namespace OnoStore.Customer.API.Data
{
    public sealed class CustomerContext : DbContext, IUnitOfWork
    {
        //private readonly IMediatorHandler _mediatorHandler;

        public CustomerContext(DbContextOptions<CustomerContext> options)//, IMediatorHandler mediatorHandler)
            : base(options)
        {
            //_mediatorHandler = mediatorHandler;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Models.Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            //modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull; // delete in cascade disabled

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;
            //if (sucesso) await _mediatorHandler.PublicarEventos(this);

            return success;
        }
    }

    public static class MediatorExtension
    {
        //public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        //{
        //    var domainEntities = ctx.ChangeTracker
        //        .Entries<Entity>()
        //        .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

        //    var domainEvents = domainEntities
        //        .SelectMany(x => x.Entity.Notificacoes)
        //        .ToList();

        //    domainEntities.ToList()
        //        .ForEach(entity => entity.Entity.LimparEventos());

        //    var tasks = domainEvents
        //        .Select(async (domainEvent) => {
        //            await mediator.PublishEvent(domainEvent);
        //        });

        //    await Task.WhenAll(tasks);
        //}
    }
}