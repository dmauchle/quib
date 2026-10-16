# Coding Styles and Guidelines

## Purpose

This document describes the coding style, conventions, and practical guidelines to be followed by developers and AI 
agents working on this repository. The goal is to keep all code consistent, readable, secure, and maintainable.

## Scope

Applies to all C# projects and related tests in this repository. These guidelines assume .NET 10 target frameworks and 
nullable reference types enabled where appropriate. This guideline also applies to markdown files.

## Project Structure

For each supported database/provider (like MySql, Postgres), there is a separate library project and unit test project.
Most of the code lives in a shared project (source files are linked). Whenever we need to distinguish between providers
(like SQL query strings), we use a compiler directive. So each library project defines a constant for its database
provider (like MYSQL or POSTGRES).

## Formatting & Tooling

- Follow the configurations in the .editorconfig at repository root.
- Use Roslyn analyzers and code style rules (Microsoft.CodeAnalysis, StyleCop.Analyzers) where configured.
- Run dotnet format and the test suite before submitting changes.

## Naming

- PascalCase for types, properties, methods, events, and public members.
- camelCase for private fields and parameters. Backing fields: _camelCase (single underscore).
- Interfaces start with `I` (e.g., `IRepository`).
- Async methods dont have to end with `Async` (e.g., `GetAsync`), especially if there is no synchronous counterpart. If both sync and async exist, use `Async` suffix for the async version.
- Boolean properties/methods use affirmative names (e.g., `IsEnabled`, `HasItems`).

## Files & Organization

- One type per file when possible. File name should match the primary type name.
- Organize code into clear folders by feature or layer (e.g., Controllers, Services, Infrastructure, Domain, Tests).
- Keep classes small and focused. Prefer composition over large God-classes.

## Usings & Namespaces

- Place `using` directives at the top of the file (file-scoped usings allowed if consistent across repo).
- Remove unused `using`s.
- Use single namespace per file. Avoid nested namespaces unless necessary for clarity.
- Use single line namespace declarations.

## Access Modifiers

- Explicitly specify accessibility (avoid default internal for top-level types unless intended).
- Keep types as restrictive as possible (internal > public).
- Keep members as restrictive as possible (private > internal > protected > public).

## Inheritance

- Make classes `sealed` unless inheritance is explicitely required.

## Fields, Properties & Constructors

- Prefer immutable types where practical. Use `readonly` for fields that do not change.
- Prefer properties over public fields.
- Validate constructor arguments and throw ArgumentNullException / ArgumentException for invalid inputs.

## Methods & Style

- Keep methods short and single-responsibility.
- Prefer `var` when the type is obvious from the right-hand side.

## Async & Concurrency

- Always use async/await for I/O-bound operations. Avoid blocking calls (.Result, .Wait()).
- Propagate CancellationToken for public async operations when cancellation is meaningful.

## Nullability & Guards

- Prefer nullable reference annotations and check for null where appropriate.
- Use `ArgumentNullException.ThrowIfNull(argument);` for argument guards when targeting modern frameworks.
- Nullable annotations are compile-time only: consuming projects may have them disabled or ignore the warnings. So 
  every public member (incl. operators, extension members and their receiver, and public members of internal types 
  implementing public interfaces) guards all non-nullable reference type arguments with `ThrowIfNull` at the start of 
  the method, even if the argument is only passed on. Internal members don't need guards.

## Exceptions & Error Handling

- Use exceptions for exceptional conditions, not control flow.
- Catch and rethrow with additional context where necessary, avoid swallowing exceptions silently.
- For library code, prefer returning Result/Outcome patterns when errors are expected and part of the domain.

## Tests

- Write unit tests for behavior, not implementation details.
- Use clear Arrange/Act/Assert structure and descriptive test names (MethodUnderTest_StateUnderTest_ExpectedBehavior).
- For the "good" case, just use the "_Works" suffix (e.g., `GetUserById_Works`).
- Prefer deterministic tests (avoid reliance on system time, network, random without seed).

## Documentation & Comments

- Use XML doc comments for public APIs. Keep comments concise and meaningful.
- Prefer expressive code over comments. Use comments to explain why, not what.
- For markdown files, use a maximum line width of 120 characters in paragraphs.

## Code Reviews & PRs

- Make small, focused pull requests. Explain the purpose and scope in the PR description.
- Include test changes and run the test suite locally before submitting.

## Security & Secrets

- Never hard-code secrets, keys, or passwords in the source tree. Use secrets management (user secrets, environment variables, vaults).
- Validate inputs and avoid injection vulnerabilities.

## Behavior for AI Agents

- Make minimal, well-explained changes. Prefer edits that follow repository conventions and existing patterns.
- When uncertain, add TODO comments and ask for human review. Do not guess project-specific conventions that are not discoverable.
- Always run build and tests locally (or via CI) before proposing changes.
- Do not remove or expose secrets or credentials. If a secret is found, record it as an observation and notify maintainers — do not commit it.

## Commit Messages

- Use imperative, present-tense summaries (e.g., "Add caching for user service").
- Include issue or ticket references when available.

## Enforcement

- Prefer automated enforcement (editorconfig, analyzers, pre-commit hooks, CI checks) over manual enforcement.
