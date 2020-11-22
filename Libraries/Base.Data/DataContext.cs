using Base.Core.DataContext;
using Base.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Base.Data
{
    public class DataContext : IdentityDbContext<User, Role, Guid>, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public override DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces()
           .Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))).ToList();

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                builder.ApplyConfiguration(configurationInstance);
            }
            base.OnModelCreating(builder);
        }
        public override int SaveChanges()
        {
            var changes = base.SaveChanges();
            return changes;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            return changesAsync;
        }

        internal Task<int> SaveChangesAsync()
        {
            var changes = base.SaveChangesAsync();
            return changes;
        }
    }
}
