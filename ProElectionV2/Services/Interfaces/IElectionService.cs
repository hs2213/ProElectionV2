using ProElectionV2.Entities;

namespace ProElectionV2.Services.Interfaces;

public interface IElectionService
{

    /// <summary>
    /// Creates a new election.
    /// </summary>
    /// <param name="election"><see cref="Election"/> to add.</param>
    /// <returns></returns>
    public Task<Election> CreateElection(Election election);

    /// <summary>
    /// Gets an election by its ID.
    /// </summary>
    /// <param name="electionId">Id of the election to get.</param>
    /// <returns><see cref="Election"/> associated with the ID given
    /// or null if not found.</returns>
    public Task<Election?> GetElectionById(Guid electionId);
    
    /// <summary>
    /// Retrieves elections with the given IDs.
    /// </summary>
    /// <param name="electionIds"><see cref="IEnumerable{Guid}"/> of election Ids</param>
    /// <returns><see cref="IEnumerable{Election}"/> associated with Ids given</returns>
    public Task<IEnumerable<Election>> GetElectionsByMultipleIds(IEnumerable<Guid> electionIds);

    /// <summary>
    /// Gets all existing elections.
    /// </summary>
    /// <returns><see cref="IEnumerable{Election}"/> containing all elections.</returns>
    public Task<IEnumerable<Election>> GetAllElections();

    /// <summary>
    /// Gets an <see cref="ElectionCode"/> object by a given ID
    /// </summary>
    /// <param name="electionCodeId">Id of the election code to get</param>
    /// <returns><see cref="ElectionCode"/> containing the given ID or null if it doesnt exist</returns>
    public Task<ElectionCode?> GetElectionCodeById(Guid electionCodeId);

    /// <summary>
    /// Gets an <see cref="ElectionCode"/> associated with the user Id and election Id.
    /// If no election code already exists in the database - a new election code is created.
    /// </summary>
    /// <param name="electionId">Id of election</param>
    /// <param name="userId">Id of user</param>
    /// <returns><see cref="ElectionCode"/> associated with the user ID and election ID given</returns>
    public Task<ElectionCode> GetElectionCodeByUserAndElection(Guid electionId, Guid userId);

    /// <summary>
    /// Records a new vote for a user
    /// </summary>
    /// <param name="vote"><see cref="Vote"/> to record</param>
    /// <returns></returns>
    public Task Vote(Vote vote);

    /// <summary>
    /// Marks an election code as used.
    /// </summary>
    /// <param name="electionCode"><see cref="ElectionCode"/> to mark</param>
    /// <returns></returns>
    public Task MarkElectionCodeAsUsed(ElectionCode electionCode);
    
    /// <summary>
    /// Checks if a user has voted or not for the given election associated with the election ID given.
    /// </summary>
    /// <param name="electionId">Election Id of election to check</param>
    /// <param name="userId">User Id of user to check</param>
    /// <returns>true if a user has voted in the given election</returns>
    public Task<bool> CheckIfUserVoted(Guid electionId, Guid userId);

    /// <summary>
    /// Calculates the results of an election from a given election ID.
    /// </summary>
    /// <param name="electionId">Election Id of election to check</param>
    /// <returns>null if the election is not found
    /// or an ordered dictionary by the candidates vote count
    /// where the key is the candidate and the value is the number of votes they got</returns>
    public Task<Dictionary<User, int>?> CalculateResults(Guid electionId);
}