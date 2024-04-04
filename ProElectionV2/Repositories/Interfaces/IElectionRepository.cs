using ProElectionV2.Entities;

namespace ProElectionV2.Repositories.Interfaces;

/// <summary>
/// Repository containing methods for interacting with the elections table in the database.
/// </summary>
public interface IElectionRepository : IBaseRepository<Election>
{
    /// <summary>
    /// Gets all existing elections in the elections table in the database.
    /// </summary>
    /// <returns>The newly created election</returns>
    public Task<IEnumerable<Election>> GetElections();

    /// <summary>
    /// Gets multiple elections by their IDs from the elections table in the database.
    /// </summary>
    /// <param name="ids"><see cref="IEnumerable{Guid}"/> of election Ids</param>
    /// <returns>List of Elections associated with the user</returns>
    public Task<IEnumerable<Election>> GetMultipleElectionsByIds(IEnumerable<Guid> ids);
}