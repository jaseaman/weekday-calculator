using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeekdayCalculator.Core.Model;

namespace WeekdayCalculator.Infrastructure.Repository.EntityFramework
{
    public class EntityFrameworkRepository<TEntity, TIdentifier> : IReadRepository<TEntity, TIdentifier> where TEntity : class, IIdentifiable<TIdentifier>
    {
        private readonly DbContext _context;
        
        public EntityFrameworkRepository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Query => _context.Set<TEntity>();
        
        // Incomplete repository definition
    }
}