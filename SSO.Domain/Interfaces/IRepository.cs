namespace SSO.Domain.Interfaces
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Adds entity
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> Add(T param);

        /// <summary>
        /// Updates entity
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> Update(T param);

        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="param"></param>
        Task Delete(T param);

        /// <summary>
        /// Filters entities
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> Find(Func<T, bool>? predicate);

        /// <summary>
        /// Gets one entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> FindOne(Func<T, bool> predicate);
    }
}
