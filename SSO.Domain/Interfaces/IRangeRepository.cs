namespace SSO.Domain.Interfaces
{
    /// <summary>
    /// Bulk process
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRangeRepository<T>
    {
        /// <summary>
        /// Adds collection of entities at once
        /// </summary>
        /// <param name="param"></param>
        /// <param name="saveChanges"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task AddRange(IEnumerable<T> param, bool? saveChanges = true, object ? args = null);

        /// <summary>
        /// Removes collection of entities at once
        /// </summary>
        /// <param name="param"></param>
        /// <param name="saveChanges"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task RemoveRange(IEnumerable<T> param, bool? saveChanges = true, object? args = null);
    }
}
