using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context) 
        {
            _context = context;
        }
        public async Task<T> GetByIdAsync(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            return result;
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            var resultList = await _context.Set<T>().ToListAsync();
            return resultList;
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            var result = await ApplySpecification(spec).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            var result = await ApplySpecification(spec).ToListAsync();
            return result;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
    }
}
