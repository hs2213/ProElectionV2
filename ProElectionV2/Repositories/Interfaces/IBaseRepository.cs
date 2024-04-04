namespace ProElectionV2.Repositories.Interfaces;

/// <summary>
/// Base repository interface for common actions between repositories such as creating, adding, updating etc..
/// </summary>
public interface IBaseRepository<T> : IAsyncDisposable
{
    /// <summary>
    /// Gets an object from the database by its Id.
    /// </summary>
    /// <param name="id"><see cref="Guid"/> Id of object to get.</param>
    /// <returns>Object with the given ID or null if not found.</returns>
    public abstract Task<T?> GetById(Guid id);

    /// <summary>
    /// Creates a new object (row) in the database.
    /// </summary>
    /// <param name="obj">object to add to the database.</param>
    /// <returns>object added to the database</returns>
    public abstract Task<T> Create(T obj);

    /// <summary>
    /// Updates a object in the database.
    /// </summary>
    /// <param name="obj">object to update in the database.</param>
    /// <returns></returns>
    public Task Update(T obj);
}