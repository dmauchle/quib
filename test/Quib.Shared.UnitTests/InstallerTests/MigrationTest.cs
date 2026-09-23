using System.Text;
using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.InstallerTests;

[Collection(TestCollections.DATABASE)]
public class MigrationTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static StringBuilder _record = new StringBuilder();

    private class Products : IModuleMigration
    {
        public static string ModuleName => "products";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("products:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("products:2"));

            module.AddVersion(3)
                .With((_, _) => _record.AppendLine("products:3"));
        }
    }

    private class Orders : IModuleMigration
    {
        public static string ModuleName => "orders";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("orders:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("orders:2"));
        }
    }

    [Fact]
    public async Task Migration_RegistersVersionsCorrectly()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<Products>();
            migration.Add<Orders>();
            await install.RunMigration(migration);

            var output = _record.ToString();
            log.WriteLine(output);

            // Verify all versions executed
            Assert.Contains("products:1", output);
            Assert.Contains("products:2", output);
            Assert.Contains("products:3", output);
            Assert.Contains("orders:1", output);
            Assert.Contains("orders:2", output);

            // Verify only the latest version of each module is registered in VersionsTable
            var registeredVersions = await DB.SelectFromTable("dbversions")
                .Project((rs, table) => new
                {
                    ModuleName = rs.Get(table.Column<DbText>("module")),
                    Version = rs.Get(table.Column<DbInt>("version"))
                })
                .FetchArray((rs, table) => new
                {
                    ModuleName = rs.Get(table.ModuleName),
                    Version = rs.Get(table.Version)
                });

            Assert.Equal(2, registeredVersions.Length);

            var productsVersion = registeredVersions.FirstOrDefault(v => v.ModuleName == "products");
            Assert.NotNull(productsVersion);
            Assert.Equal(3, productsVersion.Version);

            var ordersVersion = registeredVersions.FirstOrDefault(v => v.ModuleName == "orders");
            Assert.NotNull(ordersVersion);
            Assert.Equal(2, ordersVersion.Version);
        }
    }

    private class Inventory : IModuleMigration
    {
        public static string ModuleName => "inventory";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("inventory:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("inventory:2"));

            module.AddVersion(3)
                .With((_, _) => _record.AppendLine("inventory:3"));

            module.AddVersion(4)
                .With((_, _) => _record.AppendLine("inventory:4"));

            module.AddVersion(5)
                .With((_, _) => _record.AppendLine("inventory:5"));
        }
    }

    private class Shipping : IModuleMigration
    {
        public static string ModuleName => "shipping";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("shipping:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("shipping:2"));

            module.AddVersion(3)
                .With((_, _) => _record.AppendLine("shipping:3"));

            module.AddVersion(4)
                .With((_, _) => _record.AppendLine("shipping:4"));
        }
    }

    [Fact]
    public async Task Migration_OnlyAppliesNewVersions()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            // Create initial state: inventory at version 2, shipping at version 3
            install.Table("dbversions").CreateWithPrimaryKey("module", ColumnType.VarChar(36))
                .CreateColumn("version", ColumnType.Int().NotNull());

            await DB.InsertIntoTable("dbversions")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbText>("module"), "inventory");
                    query.Set(table.Column<DbInt>("version"), 2);
                })
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbText>("module"), "shipping");
                    query.Set(table.Column<DbInt>("version"), 3);
                })
                .Execute();

            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<Inventory>();
            migration.Add<Shipping>();
            await install.RunMigration(migration);

            var output = _record.ToString();
            log.WriteLine(output);

            // Verify only versions after initial state were applied
            // Inventory: had version 2, so versions 3, 4, 5 should be applied
            // Shipping: had version 3, so versions 4 should be applied
            Assert.DoesNotContain("inventory:1", output);
            Assert.DoesNotContain("inventory:2", output);
            Assert.Contains("inventory:3", output);
            Assert.Contains("inventory:4", output);
            Assert.Contains("inventory:5", output);

            Assert.DoesNotContain("shipping:1", output);
            Assert.DoesNotContain("shipping:2", output);
            Assert.DoesNotContain("shipping:3", output);
            Assert.Contains("shipping:4", output);

            // Verify final versions in VersionsTable
            var registeredVersions = await DB.SelectFromTable("dbversions")
                .Project((rs, table) => new
                {
                    ModuleName = rs.Get(table.Column<DbText>("module")),
                    Version = rs.Get(table.Column<DbInt>("version"))
                })
                .FetchArray((rs, table) => new
                {
                    ModuleName = rs.Get(table.ModuleName),
                    Version = rs.Get(table.Version)
                });

            Assert.Equal(2, registeredVersions.Length);

            var inventoryVersion = registeredVersions.FirstOrDefault(v => v.ModuleName == "inventory");
            Assert.NotNull(inventoryVersion);
            Assert.Equal(5, inventoryVersion.Version);

            var shippingVersion = registeredVersions.FirstOrDefault(v => v.ModuleName == "shipping");
            Assert.NotNull(shippingVersion);
            Assert.Equal(4, shippingVersion.Version);
        }
    }

    private class ModuleA : IModuleMigration
    {
        public static string ModuleName => "a";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("a:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("a:2"));

            module.AddVersion(3)
                .With((_, _) => _record.AppendLine("a:3"));
        }
    }

    private class ModuleB : IModuleMigration
    {
        public static string ModuleName => "b";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleA>(version: 2)
                .With((_, _) => _record.AppendLine("b:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("b:2"));
        }
    }

    [Fact]
    public async Task Migration_DependencyOnAlreadyInstalledModule_Works()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            // Create initial state: module A at version 2
            install.Table("dbversions").CreateWithPrimaryKey("module", ColumnType.VarChar(36))
                .CreateColumn("version", ColumnType.Int().NotNull());

            await DB.InsertIntoTable("dbversions")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbText>("module"), "a");
                    query.Set(table.Column<DbInt>("version"), 2);
                })
                .Execute();

            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleA>();
            migration.Add<ModuleB>();
            await install.RunMigration(migration);

            var output = _record.ToString();
            log.WriteLine(output);

            // Verify module A only had version 3 applied (started at 2)
            Assert.DoesNotContain("a:1", output);
            Assert.DoesNotContain("a:2", output);
            Assert.Contains("a:3", output);

            // Verify module B had all versions applied (new module)
            Assert.Contains("b:1", output);
            Assert.Contains("b:2", output);

            // Verify B:1 executes before A:3 (B:1 depends on A:2 which is already installed)
            var b1Index = output.IndexOf("b:1");
            var a3Index = output.IndexOf("a:3");
            Assert.True(b1Index < a3Index, "B:1 should execute before A:3 due to dependency on A:2");

            // Verify final versions in VersionsTable
            var registeredVersions = await DB.SelectFromTable("dbversions")
                .Project((rs, table) => new
                {
                    ModuleName = rs.Get(table.Column<DbText>("module")),
                    Version = rs.Get(table.Column<DbInt>("version"))
                })
                .FetchArray((rs, table) => new
                {
                    ModuleName = rs.Get(table.ModuleName),
                    Version = rs.Get(table.Version)
                });

            Assert.Equal(2, registeredVersions.Length);

            var moduleAVersion = registeredVersions.FirstOrDefault(v => v.ModuleName == "a");
            Assert.NotNull(moduleAVersion);
            Assert.Equal(3, moduleAVersion.Version);

            var moduleBVersion = registeredVersions.FirstOrDefault(v => v.ModuleName == "b");
            Assert.NotNull(moduleBVersion);
            Assert.Equal(2, moduleBVersion.Version);
        }
    }


    private class ModuleC : IModuleMigration
    {
        public static string ModuleName => "c";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleD>(version: 2)
                .With((_, _) => _record.AppendLine("c:1"));
        }
    }

    private class ModuleD : IModuleMigration
    {
        public static string ModuleName => "d";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("d:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("d:2"));

            module.AddVersion(3)
                .With((_, _) => _record.AppendLine("d:3"));
        }
    }

    [Fact]
    public async Task Migration_WithModuleInstalledBefore_Throws()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            // Create initial state: module D at version 3
            install.Table("dbversions").CreateWithPrimaryKey("module", ColumnType.VarChar(36))
                .CreateColumn("version", ColumnType.Int().NotNull());

            await DB.InsertIntoTable("dbversions")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbText>("module"), "d");
                    query.Set(table.Column<DbInt>("version"), 3);
                })
                .Execute();

            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleC>();
            migration.Add<ModuleD>();

            // Module C:1 requires D:2, but D:3 is already installed
            // This should throw an exception due to version conflict
            var ex = await Assert.ThrowsAsync<MigrationException>(() => install.RunMigration(migration));
            Assert.Equal("Migration 'd:2' required by 'c:1' has already been installed before.", ex.Message);
        }
    }


  private class ModuleE : IModuleMigration
    {
        public static string ModuleName => "e";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(3)
                .With((_, _) => _record.AppendLine("e:3"));
        }
    }

    private class ModuleF : IModuleMigration
    {
        public static string ModuleName => "f";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleE>(version: 2)
                .With((_, _) => _record.AppendLine("f:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("f:2"));
        }
    }

    [Fact]
    public async Task Migration_DependencyOnInstalledModuleVersion_Works()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            // Create initial state: module E at version 2
            install.Table("dbversions").CreateWithPrimaryKey("module", ColumnType.VarChar(36))
                .CreateColumn("version", ColumnType.Int().NotNull());

            await DB.InsertIntoTable("dbversions")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbText>("module"), "e");
                    query.Set(table.Column<DbInt>("version"), 2);
                })
                .Execute();

            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleE>();
            migration.Add<ModuleF>();
            await install.RunMigration(migration);

            var output = _record.ToString();
            log.WriteLine(output);

            // Verify module E only had version 3 applied (started at 2)
            Assert.DoesNotContain("e:1", output);
            Assert.DoesNotContain("e:2", output);
            Assert.Contains("e:3", output);

            // Verify module F had all versions applied (new module depending on E:2)
            Assert.Contains("f:1", output);
            Assert.Contains("f:2", output);

            // Verify F:1 executes before E:3 (F:1 depends on E:2 which is already installed)
            var f1Index = output.IndexOf("f:1");
            var e3Index = output.IndexOf("e:3");
            Assert.True(f1Index < e3Index, "F:1 should execute before E:3 due to dependency on E:2");

            // Verify final versions in VersionsTable
            var registeredVersions = await DB.SelectFromTable("dbversions")
                .Project((rs, table) => new
                {
                    ModuleName = rs.Get(table.Column<DbText>("module")),
                    Version = rs.Get(table.Column<DbInt>("version"))
                })
                .FetchArray((rs, table) => new
                {
                    ModuleName = rs.Get(table.ModuleName),
                    Version = rs.Get(table.Version)
                });

            Assert.Equal(2, registeredVersions.Length);

            var moduleEVersion = registeredVersions.FirstOrDefault(v => v.ModuleName == "e");
            Assert.NotNull(moduleEVersion);
            Assert.Equal(3, moduleEVersion.Version);

            var moduleFVersion = registeredVersions.FirstOrDefault(v => v.ModuleName == "f");
            Assert.NotNull(moduleFVersion);
            Assert.Equal(2, moduleFVersion.Version);
        }
    }


}
