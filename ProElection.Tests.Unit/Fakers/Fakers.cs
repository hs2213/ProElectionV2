namespace ProElection.Tests.Unit.Fakers;

public static class Fakers
{
    public static User FakeUser(UserType userType)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            HashedPassword = "Test123",
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