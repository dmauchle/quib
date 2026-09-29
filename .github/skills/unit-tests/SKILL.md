---
name: unit-tests
description: "Use when writing or extending unit tests for this project/library."
---

# Unit Test Skill

## Scope

Use this skill whenever tests are added or changed in this repository.

This includes:
- new unit test files
- extending coverage for existing features
- tests for new DB expression types and operators
- tests for query/update/schema/installer behavior

## Test Placement Rules

Follow the shared-test architecture:

- Put most test code in `test/Quib.Shared.UnitTests/`.
- Organize tests by feature folder:
  - `ExpressionTests/`
  - `QueryTests/`
  - `UpdateTests/`
  - `SchemaTests/`
  - `InstallerTests/`

When placing a new test file:
- First try to use an existing feature folder.
- If no existing folder fits the test topic, ask the user whether to:
  - place it in a specific existing folder, or
  - create a new feature folder.

## Important Build Inclusion Rule

`test/Quib.Shared.UnitTests/Quib.Shared.UnitTests.projitems` contains explicit `<Compile Include=...>` entries.

After adding a shared test file, always add it to:
- `test/Quib.Shared.UnitTests/Quib.Shared.UnitTests.projitems`

If you skip this step, the test will not be compiled in shared-project consumers.

## Structure Conventions (match existing tests)

- Framework: xUnit
- Typical class shape:
  - `[Collection(TestCollections.DATABASE)]`
  - constructor `(DatabaseFixture env, ITestOutputHelper log)`
  - base class `DbTestBase`
- Async test methods with `[Fact]` and `Task`.
- Reuse `DB` from `DbTestBase` for all DB interactions.
- Use existing namespace style from nearby tests (currently `Quib.UnitTests.SchemaTests` across many test folders).

## Naming Conventions in This Repository

Match existing file and method naming patterns in the same folder:
- Use test class names like:
  - `DbTextExpressionTest`
  - `InsertTest`
  - `JoinTest`
- Methods use descriptive scenario names, e.g.:
  - `Equal_WithValue_True`
  - `PlusNullableNull`
  - `FetchOne_WithoutRow`

Prefer consistency with neighboring tests over introducing new naming styles.

## General Test Quality Rules

- Keep tests deterministic.
- Assert both positive and null/empty/edge cases.
- Use Arrange/Act/Assert flow (can be compact).
- Keep tests small and focused on behavior, not implementation details.
- Follow `.editorconfig` and `docs/CodingGuidelines.md`.

## Verification Workflow

After test changes:

1. Build once.
2. Run the full test suite.
3. If failures occur, fix root cause in one pass; otherwise ask for clarification.

Do not skip verification.
