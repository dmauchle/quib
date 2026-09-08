# Supported Databases

Quib ships database- and version-specific packages. Pick the package that matches your database
server and SQL dialect.

Current packages (examples):

- Quib.MySql56 — MySQL 5.6
- Quib.MySql57 — MySQL 5.7
- Quib.MySql80 — MySQL 8.0
- Quib.MySql84 — MySQL 8.4
- Quib.Postgres18 — Postgres 18

Each package contains dialect-specific features and function mappings. You can sometimes use a package built for a nearby 
version if your SQL uses only features common to both dialects, but runtime exceptions may occur for dialect-specific SQL.
