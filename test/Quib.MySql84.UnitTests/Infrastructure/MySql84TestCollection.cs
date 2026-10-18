namespace Quib.UnitTests.Infrastructure;

[CollectionDefinition(TestCollections.DATABASE)]
public class MySql84TestCollection : ICollectionFixture<DatabaseFixture>
{
}

public sealed class DatabaseFixture : MySqlFixture
{

    public DatabaseFixture() : base(MySqlContainer.CreateV84())
    {

    }

}
