using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.InstallerTests;

[Collection(TestCollections.DATABASE)]
public class ColumnInstallerTest(DatabaseFixture env) : DbTestBase(env)
{
    private async Task<long> ScalarLong(string sql)
    {
        using (var cmd = DbConnection.CreateCommand())
        {
            cmd.CommandText = sql;
            var rs = await cmd.ExecuteScalarAsync();
            return (long)rs!;
        }
    }

    [Fact]
    public async Task CreateColumn_Works()
    {
        var name = "ci_create";
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table(name)
                .CreateWithPrimaryKey("id", ColumnType.VarChar(32))
                .AddColumn("age", ColumnType.VarChar(32).NotNull());

            var colCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '{name}' AND column_name = 'age'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = current_schema() AND table_name = '{name}' AND column_name = 'age'"
#endif
            );
            Assert.Equal(1L, colCnt);
        }
    }

    [Fact]
    public async Task Drop_Works()
    {
        var name = "ci_drop";
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table(name)
                .CreateWithPrimaryKey("id", ColumnType.VarChar(32))
                .AddColumn("age", ColumnType.VarChar(32).NotNull());

            var before = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '{name}' AND column_name = 'age'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = current_schema() AND table_name = '{name}' AND column_name = 'age'"
#endif
            );
            Assert.Equal(1L, before);

            install.Table(name).Column("age").Drop();

            var after = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '{name}' AND column_name = 'age'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = current_schema() AND table_name = '{name}' AND column_name = 'age'"
#endif
            );
            Assert.Equal(0L, after);
        }
    }

    [Fact(Skip = "Need more types first.")]
    public async Task ChangeType_Works()
    {
        throw new NotImplementedException("Need more types first.");
    }

#if MYSQL56 || MYSQL57
    [Fact(Skip = "MySQL 5.6 and 5.7 do not support renaming columns. Use a different method.")]
    public async Task Rename_Works()
    {
    }
#else
    [Fact]
    public async Task Rename_Works()
    {
        var name = "ci_rename";
        using (var install = new DbInstaller(DB.Connection))
        {
            var table = install.Table(name)
                .CreateWithPrimaryKey("id", ColumnType.VarChar(32))
                .AddColumn("c1", ColumnType.VarChar(32).NotNull());

            table.Column("c1").RenameTo("c2");

            var oldCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '{name}' AND column_name = 'c1'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = current_schema() AND table_name = '{name}' AND column_name = 'c1'"
#endif
            );
            var newCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '{name}' AND column_name = 'c2'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = current_schema() AND table_name = '{name}' AND column_name = 'c2'"
#endif
            );

            Assert.Equal(0L, oldCnt);
            Assert.Equal(1L, newCnt);
        }
    }
#endif

#if MYSQL56 || MYSQL57
    [Fact(Skip = "MySQL 5.6 and 5.7 do not support renaming columns. Use a different method.")]
    public async Task Rename_ForeignKeyColumn_Works()
    {
    }
#else
    [Fact]
    public async Task Rename_ForeignKeyColumn_Works()
    {
        var companiesTableName = "ci_companies";
        var usersTableName = "ci_users";
        var fkname = "fk_ci_users_companies";

        using (var install = new DbInstaller(DB.Connection))
        {
            // create parent table
            var companies = install.Table(companiesTableName);
            companies.CreateWithPrimaryKey("id", ColumnType.VarChar(32));

            // create child table with company column
            var users = install.Table(usersTableName)
                .CreateWithPrimaryKey("id", ColumnType.VarChar(32))
                .AddColumn("company", ColumnType.VarChar(32).NotNull())
                .CreateForeignKey(fkname, ["company"], companiesTableName, ["id"]);  // create foreign key from users.company -> companies.id

            // rename the company column
            users.Column("company").RenameTo("company_id");

            // verify old column is gone and new column exists
            var oldCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '{usersTableName}' AND column_name = 'company'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = current_schema() AND table_name = '{usersTableName}' AND column_name = 'company'"
#endif
            );
            var newCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '{usersTableName}' AND column_name = 'company_id'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = current_schema() AND table_name = '{usersTableName}' AND column_name = 'company_id'"
#endif
            );

            Assert.Equal(0L, oldCnt);
            Assert.Equal(1L, newCnt);

            // verify foreign key constraint still exists and references the renamed column
            var fkCols = new List<string>();
            using (var cmd = DbConnection.CreateCommand())
            {
#if MYSQL
                cmd.CommandText = $"SELECT column_name FROM information_schema.key_column_usage WHERE table_schema = DATABASE() AND table_name = '{usersTableName}' AND constraint_name = '{fkname}' ORDER BY ordinal_position";
#elif POSTGRES
                cmd.CommandText = $"SELECT column_name FROM information_schema.key_column_usage WHERE table_schema = current_schema() AND table_name = '{usersTableName}' AND constraint_name = '{fkname}' ORDER BY ordinal_position";
#endif
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        fkCols.Add(reader.GetString(0));
                }
            }

            Assert.Single(fkCols);
            Assert.Equal("company_id", fkCols[0]);
        }
    }
#endif

}
