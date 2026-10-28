# Umbraco.Commerce.DemoStore

Blendid demo site for Umbraco Commerce. Not a shipped package. Shared rules: orchestrator root `CLAUDE.md` (Umbraco.Commerce workspace), when present.

## Branches
- `main` = latest major. `support/{major}.x` = older majors.
- An update for an older major goes to its `support/{major}.x` branch, never `main`. Close PRs that target the wrong branch and redo the work on the right one.
- Work on `topic/{major}.x/{name}`, PR into the target branch.

## Updating the demo to newer packages
Do every step. Skipping the database steps leaves the shipped databases behind the packages.

1. Bump every `Umbraco.Cms*` and `Umbraco.Commerce*` `PackageReference` in both `src/*/*.csproj` to the latest stable version **within the branch's major**. Check `https://api.nuget.org/v3-flatcontainer/{id}/index.json`. Don't add new package references.
2. `dotnet build`. Commit the regenerated `src/Umbraco.Commerce.DemoStore.Web/appsettings-schema.json`. Diffs that only escape characters (`'` → `'`) are expected. Other `appsettings-schema.*.json` files are gitignored.
3. **SQLite** (`src/Umbraco.Commerce.DemoStore.Web/umbraco/Data/Umbraco.sqlite.db`):
   - From `src/Umbraco.Commerce.DemoStore.Web`: `ASPNETCORE_URLS=https://localhost:<port> dotnet run --no-build --no-launch-profile`. Unattended upgrade runs on boot.
   - Confirm in `umbraco/Logs/*.json`: `Unattended upgrade completed successfully` and `Umbraco Commerce migrations executed succesfully`, no `Error`/`Fatal`.
   - Stop the site before committing (flushes the `-wal` file into the db). Commit `Umbraco.sqlite.db`. Leave `Umbraco.sqlite.db.bak` alone.
4. **SQL Server bacpac** (`db/UmbracoCommerceDemoStore_v{version}.bacpac`, `{version}` = the `Umbraco.Cms` version):
   - Start a throwaway SQL Server 2022 container (`mcr.microsoft.com/mssql/server:2022-latest`) on a free port.
   - `sqlpackage /a:Import` the current bacpac.
   - Run the site against it via env overrides `ConnectionStrings__umbracoDbDSN=<sql conn>` and `ConnectionStrings__umbracoDbDSN_ProviderName=Microsoft.Data.SqlClient`. Confirm the same log lines as step 3. Stop the site.
   - `sqlpackage /a:Export` to a file named for the new version, `git rm` the old bacpac, remove the container.
5. Update every bacpac file name and database name in `README.md`.
6. PR description: list version changes, migrations applied, and the bacpac rename.

## Gotchas
- `sqlpackage` pins an exact .NET patch in its `runtimeconfig.json`, so it fails on an older local runtime. Don't touch the global tool: `dotnet tool install microsoft.sqlpackage --tool-path <scratch>`, then lower `version` in its `tools/*/any/sqlpackage.runtimeconfig.json` to `10.0.0`.
- Backoffice login needs HTTPS. Login: `admin@example.com` / `password1234`.
- Log warning `Could not find an editor with alias Umbraco.MediaPicker` comes from the demo's old content. Ignore it.
- Verify database state directly: `umbracoKeyValue` key `Umbraco.Core.Upgrader.State+Umbraco.Core` (CMS) and row count of `umbracoCommerceMigrations` (Commerce). The SQLite and SQL Server Commerce counts differ by design.
