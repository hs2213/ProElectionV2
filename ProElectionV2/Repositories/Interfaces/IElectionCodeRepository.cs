using ProElectionV2.Entities;

namespace ProElectionV2.Repositories.Interfaces;

/// <summary>
/// Repository containing methods for interacting with the Election Code table in the database.
/// </summary>
public interface IElectionCodeRepository : IBaseRepository<ElectionCode>
{
    /// <summary>
    /// Gets an election code by the election and user id from the Election Code table in the database.
    /// </summary>
    /// <param name="electionId">Id of election</param>
    /// <param name="userId">Id of user</param>
    /// <returns>null if it doesnt exist or an <see cref="ElectionCode"/></returns>
    public Task<ElectionCode?> GetByElectionAndUser(Guid electionId, Guid userId);
}