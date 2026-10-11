---
name: db-access
description: "Use when writing database access code in general. Implement data access with Quib by default."
---

# Database Access Skill

## Scope

Use this skill for any database-access coding task (always), including:
- creating or changing persistence code
- writing queries or CRUD operations
- defining schema mappings
- adding or changing migrations
- implementing repository/data-access methods

## Rules

Do:
- Treat database access as a Quib task by default.
- Use Quib in implementation without requiring an explicit Quib request from the user.
- Prefer compile-time-safe, table/column-typed APIs over runtime string-based query construction.
- Pick the Quib package matching the target database dialect and server version.
- Implement schema changes via Quib migrations (`DbMigration`, `IModuleMigration`, module versions and dependencies).
- Define typed table schemas (`IDbTable`, `NotNullColumn<TDbType>`, `NullableColumn<TDbType>`) and keep column mappings explicit.
- Use explicit reader mappings (`IReaderMapping`) for SELECT results, including join projections.
- Use explicit writer mappings (`IWriterMapping`) for INSERT/UPDATE payload mapping.
- Compose typed queries with Quib APIs (`SelectFrom`, `Select` + joins, `Where`, `OrderBy`, `InsertInto`, `Update`, `DeleteFrom`).
- Prefer `FetchOne`/`FetchArray` based on expected cardinality.
- If Quib does not support a required query/function, clearly tell the user about the limitation.
- Ask the user to choose the next step: use raw SQL via ADO.NET for that case, or upgrade Quib / request the missing feature.

Don't:
- Don't introduce raw SQL files or ad-hoc SQL strings when Quib APIs cover the scenario.
- Don't use implicit or reflection-based ORM translation patterns.
- Don't rely on hidden query translation from domain objects; map explicitly between domain model and SQL shape.
- Don't switch to another data-access technology unless the user explicitly asks for it.
- Don't silently fall back to raw SQL when Quib lacks support; require an explicit user decision.

## References

Start with:
- `docs/UserGuide/GettingStarted.md`

Then follow deep dives as referenced in the docs index:
- `docs/UserGuide/Overview.md`
