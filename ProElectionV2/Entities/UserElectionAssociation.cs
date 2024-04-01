namespace ProElectionV2.Entities;

/// <summary>
/// Used as a base class for entities that require both an election ID and a UserId.
/// </summary>
public abstract class UserElectionAssociation
{
    /// <summary>
    /// Unique Identifier for association
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Election ID associated with the entity.
    /// </summary>
    public Guid ElectionId { get; set; }
    
    /// <summary>
    /// User ID associated with the entity.
    /// </summary>
    public Guid UserId { get; set; }
}