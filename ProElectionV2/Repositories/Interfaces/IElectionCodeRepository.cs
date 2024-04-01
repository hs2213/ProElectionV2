using ProElectionV2.Entities;

namespace ProElectionV2.Repositories.Interfaces;

/// <summary>
/// Repository containing methods for interacting with the Election Code table in the database.
/// </summary>
public interface IElectionCodeRepository
{
    /// <summary>
    /// Creates a new Election Code in the Election Code table in the database.
    /// </summary>
    /// <param name="electionCode"><see cref="ElectionCode"/> to add.</param>
    /// <returns><see cref="ElectionCode"/> added to the database.</returns>
    public Task<ElectionCode> Create(ElectionCode electionCode);

    /// <summary>
    /// Gets an Election Code by its ID from the Election Code table in the database.
    /// </summary>
    /// <param name="id"><see cref="Guid"/> Id of election code to get.</param>
    /// <returns><see cref="ElectionCode"/> with the given id or null if not found.</returns>
    public Task<ElectionCode?> GetById(Guid id);

    /// <summary>
    /// Gets an election code by the election and user id from the Election Code table in the database.
    /// </summary>
    /// <param name="electionId">Id of election</param>
    /// <param name="userId">Id of user</param>
    /// <returns>null if it doesnt exist or an <see cref="ElectionCode"/></returns>
    public Task<ElectionCode?> GetByElectionAndUser(Guid electionId, Guid userId);

    /// <summary>
    /// Updates an Election Code in the Election Code table in the database.
    /// </summary>
    /// <param name="electionCode"><see cref="ElectionCode"/> with details to update</param>
    /// <returns></returns>
    public Task Update(ElectionCode electionCode);
}