namespace ProElection.Tests.Unit;

public static class Fakers
{
    public static User FakeUser(UserType userType)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            HashedPassword = "9f5930755fd506140d5af6332d2d839a2c8ee29242919cd010324b3212c064c9",
            PasswordSalt = "Hello",
            UserType = userType,
            Address = "24 Test Street",
            Country = "Test",
            Name = "Test",
            ParticipatingElections = [],
            PhoneNumber = "01229301923",
            Postcode = "TE1 1ST",
        };
    }

    public static Election FakeElection()
    {
        return new Election
        {
            Id = Guid.NewGuid(),
            Name = "Fake Election",
            Start = DateTime.Now,
            End = DateTime.Now.AddDays(1),
            ElectionType = ElectionType.FirstPastThePost
        };
    }

    public static Vote FakeVote()
    {
        return new Vote
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CandidateId = Guid.NewGuid(),
            ElectionId = Guid.NewGuid(),
            Time = DateTimeOffset.Now,
        };
    }

    public static ElectionCode FakeElectionCode()
    {
        return new ElectionCode
        {
            Id = Guid.NewGuid(),
            ElectionId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Status = CodeStatus.New
        };
    }
}