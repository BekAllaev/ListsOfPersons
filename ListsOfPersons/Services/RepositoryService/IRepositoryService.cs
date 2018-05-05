using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListsOfPersons.Services.RepositoryService
{
    /// <summary>
    /// Consist of editing and read methods
    /// </summary>
    /// <typeparam name="T">
    /// Model that will use as a parameter for methods
    /// </typeparam>
    public interface IRepositoryService<T> where T : class
    {
        #region Read methods
        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(string id);

        Task<List<T>> GetAllFavoriteAsync();
        #endregion

        #region Editing methods
        Task AddAsync(T entity);

        Task DeleteAsync(string id);

        Task UpdateAsync(T entity);
        #endregion
    }
}
