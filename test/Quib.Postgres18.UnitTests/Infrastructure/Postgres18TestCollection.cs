namespace Quib.UnitTests.Infrastructure;

[CollectionDefinition(TestCollections.DATABASE)]
public class Postgres18TestCollection : ICollectionFixture<DatabaseFixture>
{
}

public class DatabaseFixture : PostgresFixture
{
    public DatabaseFixture() : base(PostgresContainer.CreateV18())
    {
    }
}