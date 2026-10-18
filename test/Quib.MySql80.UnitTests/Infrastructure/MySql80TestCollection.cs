namespace Quib.UnitTests.Infrastructure;

[CollectionDefinition(TestCollections.DATABASE)]
public class MySql80TestCollection : ICollectionFixture<DatabaseFixture>
{
}

public sealed class DatabaseFixture : MySqlFixture
{

    public DatabaseFixture() : base(MySqlContainer.CreateV80())
    {

    }

}
