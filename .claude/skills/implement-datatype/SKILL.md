---
name: implement-datatype
description: "Use when I want you to implement a new datatype in the Quib query builder."
---

# Datatype Implementation Workflow

This skill extends the `implement` skill (`.claude/skills/implement/SKILL.md`). Read that skill first and follow all of 
its phases and rules (Preparation, Planning, Implementation, Verification). Everything stated there also applies here. 
The sections below only add datatype-specific steps to the corresponding phases.

The rules for datatypes are documented in /docs/Architecture.md, in the sections "Column types vs. DB types", 
"Column types" and "DB types for expressions". They apply to all steps below.


## 1. Preparation

In addition to the preparation steps of the `implement` skill:

- Read the datatype sections of /docs/Architecture.md listed above.
- Check `src/Quib.Shared/Types/DbTypes.cs` and `src/Quib.Shared/Installer/ColumnTypeFactory.cs` to see which DB types and 
  column types already exist.
- Pick the existing datatype(s) most similar to the requested one as reference for implementation and tests.


## 2. Planning

In addition to the planning steps of the `implement` skill:

- Always consider both sides: the column type(s) and the DB type(s). For a requested column type, check whether an 
  existing DB type fits. For a requested DB type, check which column types it corresponds to.
- In the plan, list the affected column types and DB types, and for each whether it already exists or must be created.
- List the operators and functions you intend to add for a new DB type. If you are not sure which ones are suitable, ask 
  me before implementing them.
- Plan the unit tests for all added functionality.


## 3. Implementation

In addition to the implementation steps of the `implement` skill:

- **New DB type**: Add the not-null type and its nullable counterpart, and for both of them all parts described in 
  "DB types for expressions" (expression extensions, factory methods, fetch, mapping and update methods).
- **New column type**: Add the factory method as described in "Column types".
- **Unit tests**: Add tests for all added functionality, as described in the "Unit tests" subsections of both sections. 
  Follow the `unit-tests` skill (`.claude/skills/unit-tests/SKILL.md`).
- Use the existing implementations and tests as a reference.
- Add all new source and test files to the corresponding `.projitems` files.


## 4. Verification

Follow the verification steps of the `implement` skill.
