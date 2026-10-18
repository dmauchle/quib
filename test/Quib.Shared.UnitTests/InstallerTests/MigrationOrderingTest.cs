using System.Text;
using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.InstallerTests;

[Collection(TestCollections.DATABASE)]
public class MigrationOrderingTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static StringBuilder _record = new StringBuilder();

    private class Users : IModuleMigration
    {
        public static string ModuleName => "users";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("users:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("users:2"));

            module.AddVersion(3)
                .With((_, _) => _record.AppendLine("users:3"));
        }
    }

    private class Catalog : IModuleMigration
    {
        public static string ModuleName => "catalog";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("catalog:1"));

            module.AddVersion(2)
                .Requires<Users>(version: 1)
                .With((_, _) => _record.AppendLine("catalog:2"));

            module.AddVersion(3)
                .With((_, _) => _record.AppendLine("catalog:3"));
        }
    }

    private class Orders : IModuleMigration
    {
        public static string ModuleName => "orders";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<Catalog>(version: 1)
                .Requires<Users>(version: 1)
                .With((_, _) => _record.AppendLine("orders:1"));

            module.AddVersion(2).Requires<Catalog>(version: 3)
                .With((_, _) => _record.AppendLine("orders:2"));
        }
    }

    [Fact]
    public async Task Migration_IsOrderedCorrectly()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<Users>();
            migration.Add<Catalog>();
            migration.Add<Orders>();
            await install.RunMigration(migration);

            var output = _record.ToString();
            log.WriteLine(output);

            // Verify all migrations executed
            Assert.Contains("users:1", output);
            Assert.Contains("users:2", output);
            Assert.Contains("users:3", output);
            Assert.Contains("catalog:1", output);
            Assert.Contains("catalog:2", output);
            Assert.Contains("catalog:3", output);
            Assert.Contains("orders:1", output);
            Assert.Contains("orders:2", output);

            // Verify dependency ordering: dependencies must precede dependents
            var lines = output.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
            int indexOf(string s) => System.Array.IndexOf(lines, s);

            // catalog:2 requires users:1
            Assert.True(indexOf("users:1") < indexOf("catalog:2"), "users:1 must come before catalog:2");

            // orders:1 requires users:1 and catalog:1
            Assert.True(indexOf("users:1") < indexOf("orders:1"), "users:1 must come before orders:1");
            Assert.True(indexOf("catalog:1") < indexOf("orders:1"), "catalog:1 must come before orders:1");

            // orders:2 requires catalog:3
            Assert.True(indexOf("catalog:3") < indexOf("orders:2"), "catalog:3 must come before orders:2");

            // Verify module version ordering (versions within a module must be sequential)
            Assert.True(indexOf("users:1") < indexOf("users:2"), "users:1 must come before users:2");
            Assert.True(indexOf("users:2") < indexOf("users:3"), "users:2 must come before users:3");
            Assert.True(indexOf("catalog:1") < indexOf("catalog:2"), "catalog:1 must come before catalog:2");
            Assert.True(indexOf("catalog:2") < indexOf("catalog:3"), "catalog:2 must come before catalog:3");

            // Verify "version locking" constraint: catalog:2 requires users:1, so catalog:2 must execute
            // between users:1 and users:2 (i.e., while users is at version 1)
            Assert.True(indexOf("users:1") < indexOf("catalog:2") && indexOf("catalog:2") < indexOf("users:2"),
                "catalog:2 must execute exactly when users is at version 1 (between users:1 and users:2)");

            // Similarly, orders:1 requires users:1 and catalog:1, so it must execute
            // between users:1 and users:2, and between catalog:1 and catalog:2
            Assert.True(indexOf("users:1") < indexOf("orders:1") && indexOf("orders:1") < indexOf("users:2"),
                "orders:1 must execute while users is at version 1");
            Assert.True(indexOf("catalog:1") < indexOf("orders:1") && indexOf("orders:1") < indexOf("catalog:2"),
                "orders:1 must execute while catalog is at version 1");
        }
    }

    [Fact]
    public async Task Migration_WithCycles_Throws()
    {
        // Test that circular dependencies are detected and throw an exception
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleA>();
            migration.Add<ModuleB>();

            var ex = await Assert.ThrowsAsync<MigrationException>(() => install.RunMigration(migration));
            Assert.Contains("Cycle detected", ex.Message);
        }
    }

    private class ModuleA : IModuleMigration
    {
        public static string ModuleName => "moduleA";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleB>(version: 1)
                .With((_, _) => _record.AppendLine("moduleA:1"));
        }
    }

    private class ModuleB : IModuleMigration
    {
        public static string ModuleName => "moduleB";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleA>(version: 1)
                .With((_, _) => _record.AppendLine("moduleB:1"));
        }
    }

    [Fact]
    public async Task Migration_WithMultiEdgeCycles_Throws()
    {
        // Test that circular dependencies with 4 edges are detected and throw an exception
        // Forms cycle using version numbers: ModuleX:1 -> ModuleY:1 -> ModuleX:2 -> ModuleY:2 -> ModuleX:1
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleX>();
            migration.Add<ModuleY>();

            var ex = await Assert.ThrowsAsync<MigrationException>(() => install.RunMigration(migration));
            Assert.Contains("Cycle detected", ex.Message);
        }
    }

    private class ModuleX : IModuleMigration
    {
        public static string ModuleName => "moduleX";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleY>(version: 1)
                .With((_, _) => _record.AppendLine("moduleX:1"));

            module.AddVersion(2)
                .Requires<ModuleY>(version: 2)
                .With((_, _) => _record.AppendLine("moduleX:2"));
        }
    }

    private class ModuleY : IModuleMigration
    {
        public static string ModuleName => "moduleY";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleX>(version: 2)
                .With((_, _) => _record.AppendLine("moduleY:1"));

            module.AddVersion(2)
                .Requires<ModuleX>(version: 1)
                .With((_, _) => _record.AppendLine("moduleY:2"));
        }
    }

    [Fact]
    public async Task Migration_WithMissingModuleReference_Throws()
    {
        // Test that referencing a module that doesn't exist throws an exception
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleG>();
            // Note: ModuleI is NOT added, but ModuleG requires it

            var ex = await Assert.ThrowsAsync<MigrationException>(() => install.RunMigration(migration));
            Assert.Contains("Missing migration", ex.Message);
            Assert.Contains("moduleI:1", ex.Message);
        }
    }

    private class ModuleG : IModuleMigration
    {
        public static string ModuleName => "moduleG";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleI>(version: 1)
                .With((_, _) => _record.AppendLine("moduleG:1"));
        }
    }

    private class ModuleI : IModuleMigration
    {
        public static string ModuleName => "moduleI";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("moduleI:1"));
        }
    }

    [Fact]
    public async Task Migration_WithMissingVersionReference_Throws()
    {
        // Test that referencing a version that doesn't exist throws an exception
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleJ>();
            migration.Add<ModuleK>();
            // ModuleJ:1 requires ModuleK:5, but ModuleK only has version 1

            var ex = await Assert.ThrowsAsync<MigrationException>(() => install.RunMigration(migration));
            Assert.Contains("Missing migration", ex.Message);
            Assert.Contains("moduleK:5", ex.Message);
        }
    }

    private class ModuleJ : IModuleMigration
    {
        public static string ModuleName => "moduleJ";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleK>(version: 5)
                .With((_, _) => _record.AppendLine("moduleJ:1"));
        }
    }

    private class ModuleK : IModuleMigration
    {
        public static string ModuleName => "moduleK";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("moduleK:1"));
        }
    }

    [Fact]
    public async Task Migration_VersionsAddedInReverseOrder_ExecuteInCorrectOrder()
    {
        // Test that versions added in reversed order are still executed in correct ascending order
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ReversedModule>();
            await install.RunMigration(migration);

            var output = _record.ToString();
            log.WriteLine(output);

            // Verify all versions executed
            Assert.Contains("reversed:1", output);
            Assert.Contains("reversed:2", output);
            Assert.Contains("reversed:3", output);

            // Verify correct execution order (ascending, not reversed)
            var lines = output.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
            int indexOf(string s) => System.Array.IndexOf(lines, s);

            Assert.True(indexOf("reversed:1") < indexOf("reversed:2"), "reversed:1 must come before reversed:2");
            Assert.True(indexOf("reversed:2") < indexOf("reversed:3"), "reversed:2 must come before reversed:3");
        }
    }

    private class ReversedModule : IModuleMigration
    {
        public static string ModuleName => "reversed";

        public static void Configure(ModuleMigration module)
        {
            // Add versions in REVERSE order (3, 2, 1)
            // They should still execute in correct order (1, 2, 3)
            module.AddVersion(3)
                .With((_, _) => _record.AppendLine("reversed:3"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("reversed:2"));

            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("reversed:1"));
        }
    }

    [Fact]
    public async Task Migration_ModulesAddedInReverseOrder_ExecuteInCorrectOrder()
    {
        // Test that modules added in reverse dependency order still execute in correct order
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            // Add modules in REVERSE dependency order: Dependent first, dependency last
            migration.Add<ModuleN>();  // Depends on ModuleO
            migration.Add<ModuleO>();  // No dependencies
            await install.RunMigration(migration);

            var output = _record.ToString();
            log.WriteLine(output);

            // Verify all migrations executed
            Assert.Contains("moduleO:1", output);
            Assert.Contains("moduleN:1", output);

            // Verify correct execution order: dependency must execute before dependent,
            // regardless of the order modules were added
            var lines = output.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
            int indexOf(string s) => System.Array.IndexOf(lines, s);

            Assert.True(indexOf("moduleO:1") < indexOf("moduleN:1"),
                "moduleO:1 (dependency) must execute before moduleN:1 (dependent), even though ModuleN was added first");
        }
    }

    private class ModuleN : IModuleMigration
    {
        public static string ModuleName => "moduleN";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleO>(version: 1)
                .With((_, _) => _record.AppendLine("moduleN:1"));
        }
    }

    private class ModuleO : IModuleMigration
    {
        public static string ModuleName => "moduleO";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("moduleO:1"));
        }
    }

    [Fact]
    public async Task Migration_WithConflictingVersion_Throws()
    {
        // Test that requiring two different versions of the SAME module throws an exception
        // This is impossible to satisfy: can't be at version 1 and version 3 of the same module simultaneously
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleP>();
            migration.Add<ModuleR>();

            var ex = await Assert.ThrowsAsync<MigrationException>(() => install.RunMigration(migration));
            Assert.Contains("Cycle detected", ex.Message);
        }
    }

    private class ModuleP : IModuleMigration
    {
        public static string ModuleName => "moduleP";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                // Requires TWO different versions of the same module (moduleR)
                // This creates an impossible constraint: moduleR:1 and moduleR:3 need to execute "at the same time"
                .Requires<ModuleR>(version: 1)
                .Requires<ModuleR>(version: 3)
                .With((_, _) => _record.AppendLine("moduleP:1"));
        }
    }

    private class ModuleR : IModuleMigration
    {
        public static string ModuleName => "moduleR";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("moduleR:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("moduleR:2"));

            module.AddVersion(3)
                .With((_, _) => _record.AppendLine("moduleR:3"));
        }
    }

    [Fact]
    public async Task Migration_WithDuplicateVersionNumber_Throws()
    {
        // Test that adding the same version number twice for the same module throws an exception
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleS>();

            var ex = await Assert.ThrowsAsync<MigrationException>(() => install.RunMigration(migration));
            Assert.Contains("Duplicate migration key detected", ex.Message);
        }
    }

    private class ModuleS : IModuleMigration
    {
        public static string ModuleName => "moduleS";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("moduleS:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("moduleS:2"));

            // Try to add version 1 again - this should throw
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("moduleS:1-duplicate"));
        }
    }

    [Fact]
    public async Task Migration_WithDuplicateModule_Throws()
    {
        // Test that adding the same module twice (duplicate module names) throws an exception
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleT>();
            migration.Add<ModuleT>();  // Add the same module again - duplicate module name

            var ex = await Assert.ThrowsAsync<MigrationException>(() => install.RunMigration(migration));
            Assert.Contains("Duplicate migration key detected", ex.Message);
        }
    }

    [Fact]
    public async Task Migration_WithDuplicateModuleName_Throws()
    {
        // Test that adding the same module twice (duplicate module names) throws an exception
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleT>();
            migration.Add<ModuleT1>();  // Add a module with the same name

            var ex = await Assert.ThrowsAsync<MigrationException>(() => install.RunMigration(migration));
            Assert.Contains("Duplicate migration key detected", ex.Message);
        }
    }

    private class ModuleT : IModuleMigration
    {
        public static string ModuleName => "moduleT";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("moduleT:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("moduleT:2"));
        }
    }

    private class ModuleT1 : IModuleMigration
    {
        public static string ModuleName => "moduleT";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("moduleT:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("moduleT:2"));
        }
    }

    [Fact]
    public async Task Migration_WithBackwardVersionDependency_Throws()
    {
        // Test that backward version dependencies are detected and throw an exception
        // Module V:1 requires U:2, but Module V:2 requires U:1
        // This is impossible because you can't go from U:2 backwards to U:1
        using (var install = new DbInstaller(DB.Connection))
        {
            _record = new StringBuilder();
            var migration = new DbMigration("dbversions");
            migration.Add<ModuleU>();
            migration.Add<ModuleV>();

            var ex = await Assert.ThrowsAsync<MigrationException>(() => install.RunMigration(migration));
            Assert.Contains("Cycle detected", ex.Message);
        }
    }

    private class ModuleU : IModuleMigration
    {
        public static string ModuleName => "moduleU";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .With((_, _) => _record.AppendLine("moduleU:1"));

            module.AddVersion(2)
                .With((_, _) => _record.AppendLine("moduleU:2"));
        }
    }

    private class ModuleV : IModuleMigration
    {
        public static string ModuleName => "moduleV";

        public static void Configure(ModuleMigration module)
        {
            module.AddVersion(1)
                .Requires<ModuleU>(version: 2)
                .With((_, _) => _record.AppendLine("moduleV:1"));

            module.AddVersion(2)
                .Requires<ModuleU>(version: 1)
                .With((_, _) => _record.AppendLine("moduleV:2"));
        }
    }

}
