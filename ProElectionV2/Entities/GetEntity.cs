using ProElectionV2.Entities.Enums;

namespace ProElectionV2.Entities;

/// <summary>
/// Gets an empty version of an entity for initialisation
/// </summary>
public static class GetEntity
{
    public static User User(UserType userType)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Address = string.Empty,
            Country = string.Empty,
            Email = string.Empty,
            HashedPassword = string.Empty,
            PasswordSalt = string.Empty,
            ParticipatingElections = [],
            Name = string.Empty,
            PhoneNumber = string.Empty,
            Postcode = string.Empty,
            UserType = userType
        }; 
    }
    
    public static Election Election(ElectionType electionType = ElectionType.FirstPastThePost)
    {
        return new Election
        {
            Id = Guid.NewGuid(),
            ElectionType = electionType,
            End = DateTime.Now.AddDays(1),
            Name = string.Empty,
            Start = DateTime.Now,
        };
    }
    
    public static Vote Vote(Guid electionId, Guid userId, Guid candidateId)
    {
        return new Vote
        {
            Id = Guid.NewGuid(),
            ElectionId = electionId,
            UserId = userId,
            CandidateId = candidateId,
            Time = DateTimeOffset.Now
        };
    }
    
    public static ElectionCode ElectionCode(Guid electionId, Guid userId)
    {
        return new ElectionCode
        {
            Id = Guid.NewGuid(),
            ElectionId = electionId,
            UserId = userId,
            Status = CodeStatus.New
        };
    }
}