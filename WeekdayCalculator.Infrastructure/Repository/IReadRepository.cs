using System.Linq;
using WeekdayCalculator.Core.Model;

namespace WeekdayCalculator.Infrastructure.Repository
{
    public interface IReadRepository<TEntity, TIdentifier> where TEntity : IIdentifiable<TIdentifier>
    {
        public IQueryable<TEntity> Query { get; }
        
        // This is an incomplete implementation
    }
}