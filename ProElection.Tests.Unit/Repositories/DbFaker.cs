using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProElection.Tests.Unit.Repositories;

public abstract class DbFaker
{
    protected Database Database { get; private set; }

    protected DbFaker()
    {
        var builder = new DbContextOptionsBuilder<Database>();
        
        Database = database;
    }
}