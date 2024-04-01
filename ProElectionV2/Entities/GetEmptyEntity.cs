using ProElectionV2.Entities.Enums;

namespace ProElectionV2.Entities;

/// <summary>
/// Gets an empty version of an entity for initialisation
/// </summary>
public static class GetEmptyEntity
{
    public static User User()
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
            Postcode = string.Empty
        }; 
    }
    
    public static Election Election()
    {
        return new Election
        {
            Id = Guid.NewGuid(),
            ElectionType = ElectionType.FirstPastThePost,
            End = DateTime.Now.AddDays(1),
            Name = string.Empty,
            Start = DateTime.Now
        };
    }
}