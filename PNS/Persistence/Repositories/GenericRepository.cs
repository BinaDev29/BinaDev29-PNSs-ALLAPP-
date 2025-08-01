// File Path: Persistence/Repositories/GenericRepository.cs
// Namespace: Persistence.Repositories (Namespace ን ቀይረናል)
using Application.Contracts.IRepository; // ለ IGenericRepository<T>
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // ለ .Where() እና .ToListAsync()

namespace Persistence.Repositories // Namespace ን ቀይረናል
{
    // GenericRepository አሁን BaseDomainEntity ን inherit የሚያደርጉ class ዎችን ብቻ ነው የሚቀበለው
    public class GenericRepository<T> : IGenericRepository<T> where T : Domain.Common.BaseDomainEntity
    {
        private readonly PnsDbContext _context;

        public GenericRepository(PnsDbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity != null;
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(Guid id) // Nullable return type አድርገነዋል
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}