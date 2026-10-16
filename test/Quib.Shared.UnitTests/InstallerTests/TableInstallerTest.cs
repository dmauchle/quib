using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.InstallerTests;

[Collection(TestCollections.DATABASE)]
public class TableInstallerTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
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
    public async Task Exists_Works()
    {
        var name = "ti_exists";
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table(name).CreateWithPrimaryKey("id", ColumnType.VarChar(32));
            Assert.True(install.Table(name).Exists());

            Assert.False(install.Table("ti_does_not_exist").Exists());
        }
    }

    [Fact]
    public async Task CreateTable_Works()
    {
        var name = "ti_cpk_str";
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table(name).CreateWithPrimaryKey("id", ColumnType.VarChar(32));

            var colCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '{name}' AND column_name = 'id'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = current_schema() AND table_name = '{name}' AND column_name = 'id'"
#endif
            );
            Assert.Equal(1L, colCnt);

            var pkCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = DATABASE() AND table_name = '{name}' AND constraint_type = 'PRIMARY KEY'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = current_schema() AND table_name = '{name}' AND constraint_type = 'PRIMARY KEY'"
#endif
            );
            Assert.Equal(1L, pkCnt);
        }
    }

    [Fact]
    public async Task CreateTable_WithMultiPrimaryKeyColumns_Works()
    {
        var name = "ti_cpk_pk";
        using (var install = new DbInstaller(DB.Connection))
        {
            var pk = new PrimaryKey()
                .AddColumn("id", ColumnType.VarChar(32))
                .AddColumn("id2", ColumnType.VarChar(32));

            install.Table(name).CreateWithPrimaryKey(pk);

            var colCnt1 = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '{name}' AND column_name = 'id'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = current_schema() AND table_name = '{name}' AND column_name = 'id'"
#endif
            );
            Assert.Equal(1L, colCnt1);

            var colCnt2 = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '{name}' AND column_name = 'id2'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = current_schema() AND table_name = '{name}' AND column_name = 'id2'"
#endif
            );
            Assert.Equal(1L, colCnt2);

            var pkCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = DATABASE() AND table_name = '{name}' AND constraint_type = 'PRIMARY KEY'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = current_schema() AND table_name = '{name}' AND constraint_type = 'PRIMARY KEY'"
#endif
            );
            Assert.Equal(1L, pkCnt);

            var pkColumns = new List<string>();
            using (var cmd = DbConnection.CreateCommand())
            {
#if MYSQL
                cmd.CommandText = $"SELECT column_name FROM information_schema.key_column_usage WHERE table_schema = DATABASE() AND table_name = '{name}' AND constraint_name = 'PRIMARY' ORDER BY ordinal_position";
#elif POSTGRES
                cmd.CommandText = $"SELECT k.column_name FROM information_schema.table_constraints t JOIN information_schema.key_column_usage k ON t.constraint_name = k.constraint_name AND t.table_schema = k.constraint_schema WHERE t.table_schema = current_schema() AND t.table_name = '{name}' AND t.constraint_type = 'PRIMARY KEY' ORDER BY k.ordinal_position";
#endif
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        pkColumns.Add(reader.GetString(0));
                    }
                }
            }

            Assert.Equal(2, pkColumns.Count);
            Assert.Contains("id", pkColumns);
            Assert.Contains("id2", pkColumns);
        }
    }

    [Fact]
    public async Task CreateTable_WithNullablePrimaryKey_Works()
    {
        var name = "ti_cpk_str";
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table(name).CreateWithPrimaryKey("id", ColumnType.VarChar(32));
        }
    }

    [Fact]
    public async Task Drop_Works()
    {
        var name = "ti_drop";
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table(name).CreateWithPrimaryKey("id", ColumnType.VarChar(32));
            var before = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = '{name}'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = current_schema() AND table_name = '{name}'"
#endif
            );
            Assert.Equal(1L, before);

            install.Table(name).Drop();

            var after = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = '{name}'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = current_schema() AND table_name = '{name}'"
#endif
            );
            Assert.Equal(0L, after);
        }
    }

    [Fact]
    public async Task Rename_Works()
    {
        var from = "ti_rename_from";
        var to = "ti_rename_to";
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table(from).CreateWithPrimaryKey("id", ColumnType.VarChar(32));

            install.Table(from).RenameTo(to);

            var oldCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = '{from}'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = current_schema() AND table_name = '{from}'"
#endif
            );
            var newCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = '{to}'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = current_schema() AND table_name = '{to}'"
#endif
            );

            Assert.Equal(0L, oldCnt);
            Assert.Equal(1L, newCnt);
        }
    }



    [Fact]
    public async Task AddForeignKey_InvalidArguments_Throws()
    {
        var parent = "ti_parent";
        var child = "ti_child";

        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table(parent)
                .CreateWithPrimaryKey("pid", ColumnType.VarChar(32));
            install.Table(child)
                .CreateWithPrimaryKey("cid", ColumnType.VarChar(32))
                .CreateColumn("pid", ColumnType.VarChar(32).NotNull());

            // empty local columns
            Assert.Throws<ArgumentException>(() => install.Table(child).CreateForeignKey("bad", [], parent, []));
            // empty local columns but foreign provided 
            Assert.Throws<ArgumentException>(() => install.Table(child).CreateForeignKey("bad", [], parent, ["pid"]));
            // mismatched lengths
            Assert.Throws<ArgumentException>(() => install.Table(child).CreateForeignKey("bad2", ["a"], parent, []));
        }
    }

    [Fact]
    public async Task AddForeignKey_Works()
    {
        var parent = "ti_parent";
        var child = "ti_child";
        var fkname = "fk_ti_child_parent";

        using (var install = new DbInstaller(DB.Connection))
        {
            // create parent with PK pid
            install.Table(parent)
                .CreateWithPrimaryKey("pid", ColumnType.VarChar(32));

            // create child with PK cid and a pid column
            install.Table(child)
                .CreateWithPrimaryKey("cid", ColumnType.VarChar(32))
                .CreateColumn("pid", ColumnType.VarChar(32).NotNull());
            // create fk
            install.Table(child).CreateForeignKey(fkname, ["pid"], parent, ["pid"]);

            var fkCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = DATABASE() AND table_name = '{child}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = current_schema() AND table_name = '{child}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#endif
            );
            Assert.Equal(1L, fkCnt);
        }
    }

    [Fact]
    public async Task AddForeignKey_WithMultiColumns_Works()
    {
        var parent = "ti_parent2";
        var child = "ti_child2";
        var fkname = "fk_ti_child2_parent2";

        using (var install = new DbInstaller(DB.Connection))
        {
            var pk = new PrimaryKey()
                .AddColumn("pid1", ColumnType.VarChar(32))
                .AddColumn("pid2", ColumnType.VarChar(32));

            install.Table(parent).CreateWithPrimaryKey(pk);

            // create child with its own PK and the two fk columns
            install.Table(child)
                .CreateWithPrimaryKey("cid", ColumnType.VarChar(32))
                .CreateColumn("pid1", ColumnType.VarChar(32).NotNull())
                .CreateColumn("pid2", ColumnType.VarChar(32).NotNull());

            // create fk spanning two columns
            install.Table(child).CreateForeignKey(fkname, ["pid1", "pid2"], parent, ["pid1", "pid2"]);

            var fkCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = DATABASE() AND table_name = '{child}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = current_schema() AND table_name = '{child}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#endif
            );
            Assert.Equal(1L, fkCnt);

            // verify parent PK has two columns
            var pkCols = new List<string>();
            using (var cmd = DbConnection.CreateCommand())
            {
#if MYSQL
                cmd.CommandText = $"SELECT column_name FROM information_schema.key_column_usage WHERE table_schema = DATABASE() AND table_name = '{parent}' AND constraint_name = 'PRIMARY' ORDER BY ordinal_position";
#elif POSTGRES
                cmd.CommandText = $"SELECT k.column_name FROM information_schema.table_constraints t JOIN information_schema.key_column_usage k ON t.constraint_name = k.constraint_name AND t.table_schema = k.constraint_schema WHERE t.table_schema = current_schema() AND t.table_name = '{parent}' AND t.constraint_type = 'PRIMARY KEY' ORDER BY k.ordinal_position";
#endif
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        pkCols.Add(reader.GetString(0));
                }
            }
            Assert.Equal(2, pkCols.Count);
            Assert.Contains("pid1", pkCols);
            Assert.Contains("pid2", pkCols);

            // verify fk columns on child
            var fkCols = new List<string>();
            using (var cmd = DbConnection.CreateCommand())
            {
#if MYSQL
                cmd.CommandText = $"SELECT column_name FROM information_schema.key_column_usage WHERE table_schema = DATABASE() AND table_name = '{child}' AND constraint_name = '{fkname}' ORDER BY ordinal_position";
#elif POSTGRES
                cmd.CommandText = $"SELECT column_name FROM information_schema.key_column_usage WHERE table_schema = current_schema() AND table_name = '{child}' AND constraint_name = '{fkname}' ORDER BY ordinal_position";
#endif
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        fkCols.Add(reader.GetString(0));
                }
            }
            Assert.Equal(2, fkCols.Count);
            Assert.Equal("pid1", fkCols[0]);
            Assert.Equal("pid2", fkCols[1]);
        }
    }

    [Fact]
    public async Task DropForeignKey_Works()
    {
        var parent = "ti_parent";
        var child = "ti_child";
        var fkname = "fk_ti_child_parent";

        using (var install = new DbInstaller(DB.Connection))
        {
            // create parent with PK pid
            install.Table(parent).CreateWithPrimaryKey("pid", ColumnType.VarChar(32));

            // create child with PK cid and a pid column
            install.Table(child)
                .CreateWithPrimaryKey("cid", ColumnType.VarChar(32))
                .CreateColumn("pid", ColumnType.VarChar(32).NotNull());

            // create fk
            install.Table(child).CreateForeignKey(fkname, ["pid"], parent, ["pid"]);

            var fkCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = DATABASE() AND table_name = '{child}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = current_schema() AND table_name = '{child}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#endif
            );
            Assert.Equal(1L, fkCnt);

            // drop fk
            install.Table(child).DropForeignKey(fkname);

            var fkCntAfter = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = DATABASE() AND table_name = '{child}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = current_schema() AND table_name = '{child}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#endif
            );
            Assert.Equal(0L, fkCntAfter);
        }
    }

    [Fact]
    public async Task CreateIndex_Works()
    {
        var name = "ti_index";
        var idx = "idx_ti_demo";

        using (var install = new DbInstaller(DB.Connection))
        {
            var table = install.Table(name)
                .CreateWithPrimaryKey("id", ColumnType.VarChar(32))
                .CreateColumn("c1", ColumnType.VarChar(32).NotNull())
                .CreateColumn("c2", ColumnType.VarChar(32).NotNull());

            Assert.Throws<ArgumentException>(() => install.Table(name).CreateIndex("bad", []));

            install.Table(name).CreateIndex(idx, "c1", "c2");

            var idxCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.statistics WHERE table_schema = DATABASE() AND table_name = '{name}' AND index_name = '{idx}'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM pg_catalog.pg_indexes WHERE schemaname = current_schema() AND tablename = '{name}' AND indexname = '{idx}'"
#endif
            );
            Assert.True(idxCnt >= 1L);
        }
    }

    [Fact]
    public async Task DropIndex_Works()
    {
        var name = "ti_index";
        var idx = "idx_ti_demo";

        using (var install = new DbInstaller(DB.Connection))
        {
            var table = install.Table(name)
                .CreateWithPrimaryKey("id", ColumnType.VarChar(32))
                .CreateColumn("c1", ColumnType.VarChar(32).NotNull())
                .CreateColumn("c2", ColumnType.VarChar(32).NotNull());

            install.Table(name).CreateIndex(idx, "c1", "c2");

            var idxCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.statistics WHERE table_schema = DATABASE() AND table_name = '{name}' AND index_name = '{idx}'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM pg_catalog.pg_indexes WHERE schemaname = current_schema() AND tablename = '{name}' AND indexname = '{idx}'"
#endif
            );
            Assert.True(idxCnt >= 1L);

            install.Table(name).DropIndex(idx);

            var idxCntAfter = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.statistics WHERE table_schema = DATABASE() AND table_name = '{name}' AND index_name = '{idx}'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM pg_catalog.pg_indexes WHERE schemaname = current_schema() AND tablename = '{name}' AND indexname = '{idx}'"
#endif
            );
            Assert.Equal(0L, idxCntAfter);
        }
    }

    [Fact]
    public async Task Rename_OfReferencedTable_Works()
    {
        var companiesTN = "ti_companies";
        var companiesNewTN = "ti_companies_new";
        var usersTN = "ti_users";
        var fkname = "fk_ti_users_companies";

        using (var install = new DbInstaller(DB.Connection))
        {
            // create parent (companies) with PK id
            var companies = install.Table(companiesTN)
                .CreateWithPrimaryKey("id", ColumnType.VarChar(32));

            // create child (users) with PK uid and company_id column
            var users = install.Table(usersTN)
                .CreateWithPrimaryKey("uid", ColumnType.VarChar(32))
                .CreateColumn("company_id", ColumnType.VarChar(32).NotNull())
                .CreateForeignKey(fkname, ["company_id"], companiesTN, ["id"]);  // create fk from users.company_id -> companies.id

            // verify fk exists before rename
            var fkCntBefore = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = DATABASE() AND table_name = '{usersTN}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = current_schema() AND table_name = '{usersTN}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#endif
            );
            Assert.Equal(1L, fkCntBefore);

            // rename companies
            companies.RenameTo(companiesNewTN);

            // verify old name gone, new name present
            var oldCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = '{companiesTN}'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = current_schema() AND table_name = '{companiesTN}'"
#endif
            );
            var newCnt = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = '{companiesNewTN}'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = current_schema() AND table_name = '{companiesNewTN}'"
#endif
            );

            Assert.Equal(0L, oldCnt);
            Assert.Equal(1L, newCnt);

            // ensure foreign key on users still exists after rename
            var fkCntAfter = await ScalarLong(
#if MYSQL
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = DATABASE() AND table_name = '{usersTN}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#elif POSTGRES
                $"SELECT COUNT(*) FROM information_schema.table_constraints WHERE table_schema = current_schema() AND table_name = '{usersTN}' AND constraint_name = '{fkname}' AND constraint_type = 'FOREIGN KEY'"
#endif
            );
            Assert.Equal(1L, fkCntAfter);
        }
    }

}
