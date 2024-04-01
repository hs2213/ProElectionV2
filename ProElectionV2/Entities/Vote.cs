namespace ProElectionV2.Entities;

/// <summary>
/// Entity used to keep track of votes for candidates for a given election.
/// </summary>
public class Vote : UserElectionAssociation
{
    /// <summary>
    /// ID of candidate user voted for.
    /// </summary>
    public Guid CandidateId { get; set; }
    
    /// <summary>
    /// Date and time of which the user voted at
    /// DateTimeOffset used due to it using UTC so it isn't influenced by different time zones
    /// </summary>
    public DateTimeOffset Time { get; set; }
}