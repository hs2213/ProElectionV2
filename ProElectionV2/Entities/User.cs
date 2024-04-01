using ProElectionV2.Entities.Enums;

namespace ProElectionV2.Entities;

/// <summary>
/// User entity containing information about a user. 
/// </summary>
public class User
{
    /// <summary>
    /// Unique user ID.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Name of user
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Phone number of user
    /// </summary>
    public required string PhoneNumber { get; set; }
    
    /// <summary>
    /// Email of user
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Address of user's home
    /// </summary>
    public required string Address { get; set; }
    
    /// <summary>
    /// Users postcode
    /// </summary>
    public required string Postcode { get; set; }
    
    /// <summary>
    /// Country the user resides in
    /// </summary>
    public required string Country { get; set; }
    
    /// <summary>
    /// Hashed version of the users password - used for authentication
    /// </summary>
    public required string HashedPassword { get; set; }
    
    /// <summary>
    /// Each user has a unique salt to hash their password with.
    /// Used to make rainbow table attacks more difficult.
    /// </summary>
    public required string PasswordSalt { get; set; }
    
    /// <summary>
    /// Type of user - candidate, voter or admin.
    /// </summary>
    public UserType UserType { get; set; }
    
    /// <summary>
    /// List of election Id's corresponding to the elections a user can participate in.
    /// </summary>
    public required List<Guid> ParticipatingElections { get; set; }
}