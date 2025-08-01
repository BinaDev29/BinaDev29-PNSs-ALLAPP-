// File Path: Application/Contracts/IRepository/IGenericRepository.cs
// Namespace: Application.Contracts.IRepository
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Common; // ለ BaseDomainEntity

namespace Application.Contracts.IRepository
{
    public interface IGenericRepository<T> where T : BaseDomainEntity // T BaseDomainEntity መሆን አለበት
    {
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<T?> GetById(Guid id); // T? ማለት T null ሊሆን ይችላል
        Task<IReadOnlyList<T>> GetAll();
        Task<bool> Exists(Guid id);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);
    }
}