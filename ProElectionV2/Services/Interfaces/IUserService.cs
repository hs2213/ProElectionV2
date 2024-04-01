using ProElectionV2.Entities;
using ProElectionV2.Entities.Enums;

namespace ProElectionV2.Services.Interfaces;

/// <summary>
/// Service to handle user related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Used to authenticate a user by their email and password.
    /// </summary>
    /// <param name="email">input user email</param>
    /// <param name="password">raw input user password</param>
    /// <returns>null or <see cref="User"/> if email exists in the db and the password is correct.</returns>
    public Task<User?> Authenticate(string email, string password);

    /// <summary>
    /// Gets a user by its Id.
    /// </summary>
    /// <param name="id"><see cref="Guid"/> Id of user to get.</param>
    /// <returns><see cref="User"/> with the given ID or null if not found.</returns>
    public Task<User?> GetUserById(Guid id);

    /// <summary>
    /// Gets a list of <see cref="User"/> that are candidates for the given election.
    /// </summary>
    /// <param name="election">election to get candidates of</param>
    /// <returns>List of <see cref="User"/> with the candidate user type for the given election.</returns>
    public Task<IEnumerable<User>> GetCandidatesForElection(Election election);
    
    /// <summary>
    /// Gets all the users with the type of <see cref="UserType.Candidate"/>
    /// </summary>
    /// <returns>List of <see cref="User"/> of type <see cref="UserType.Candidate"/></returns>
    public Task<IEnumerable<User>> GetCandidates();

    /// <summary>
    /// Creates a new user, hashing its password.
    /// </summary>
    /// <param name="user"><see cref="User"/> to add to the database.</param>
    /// <returns>null if the email of the user is not unique or
    /// <see cref="User"/> added to the database</returns>
    public Task<User?> CreateUser(User user);

    /// <summary>
    /// Gets a list of <see cref="Election"/> the user can vote in.
    /// </summary>
    /// <param name="userId"><see cref="Guid"/> containing user ID.</param>
    /// <returns>List of elections that the user can vote in.</returns>
    public Task<IEnumerable<Election>?> GetUserElections(Guid userId);

    /// <summary>
    /// Adds an election to a <see cref="User"/>.
    /// </summary>
    /// <param name="user">user to add the election to.</param>
    /// <param name="election">Election to add</param>
    /// <returns></returns>
    public Task AddElectionToUser(User user, Election election);

    /// <summary>
    /// Gets all the users that are not already part of the given election,
    /// where their email contains the search query filtering by the <see cref="UserType"/>.
    /// </summary>
    /// <param name="searchQuery">email query to search for</param>
    /// <param name="userType"><see cref="UserType"/> of user being searched for</param>
    /// <param name="electionId">Id of the election to filter out users that arent already a part of</param>
    /// <returns>A list of users that can be added to an election</returns>
    public Task<IEnumerable<User>> GetUsersByEmailSearch(string searchQuery, UserType userType, Guid electionId);
}