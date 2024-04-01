using System.ComponentModel.DataAnnotations;
using ProElectionV2.Entities.Enums;

namespace ProElectionV2.Entities;

/// <summary>
/// Election entity containing info about an election
/// </summary>
public class Election
{
    /// <summary>
    /// Unique election ID.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Name of Election.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Start DateTime of the election.
    /// </summary>
    public DateTime Start { get; set; }
    
    /// <summary>
    /// End DateTime of the election
    /// </summary>
    public DateTime End { get; set; }
    
    public ElectionType ElectionType { get; set; }
}