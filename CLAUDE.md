# Quib

Type-safe fluent SQL query builder for .NET. One NuGet package per DB/version.


## Style

- Be extremely concise. Sacrifice grammar for concision.
- Never commit or push on your own. I'll always do it myself.


## Docs (read before non-trivial changes)

- Coding guidelines: docs/CodingGuidelines.md
- Architecture: docs/Architecture.md
- User guide: docs/UserGuide/ — update when public API changes


## Layout

- src/Quib.Shared: almost all code. Files linked into provider projects, not compiled standalone.
- src/Quib.<Provider><Version>: thin provider libs, each defines constant (MYSQL, POSTGRES, ...).
- Provider differences → `#if` compiler directives in shared code. Never copy code into provider projects.
- test/: one unit test project per provider. Tests live in Quib.Shared.UnitTests (shared project, linked into each).


## Commands

- Build: `dotnet build Quib.slnx`
- Test all: `dotnet test Quib.slnx`
- Test one provider: `dotnet test test/Quib.Postgres18.UnitTests`
- Test affected classes: `dotnet test Quib.slnx --filter "FullyQualifiedName~Quib.UnitTests.QueryTests.LimitTest"`
- Format changed files only: `dotnet format Quib.slnx --include <files>`
- Provider tests need Docker service running; they spin up their DB containers automatically.


## Workflow

- Change in shared code → run affected tests for all providers, not just one. Full suite only when asked.
- Never `dotnet format` the whole solution, only changed files.
- Use the skills in .claude/skills.
