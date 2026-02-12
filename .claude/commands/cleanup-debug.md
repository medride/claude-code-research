---
name: cleanup-debug
description: Scans for and removes debug code (console.logs, debugger statements) with human approval. Use before shipping or merging to clean up development artifacts.
disable-model-invocation: true
argument-hint: [--frontend | --backend]
allowed-tools: Grep, Glob, Read, Edit, Bash, AskUserQuestion
---

# Debug Code Cleanup

Scan the codebase for debug code and remove it with user approval.

## Arguments
- No arguments: Scan entire codebase
- `--frontend`: Scan only `src/frontend/`
- `--backend`: Scan only `src/backend/`

## Workflow

### Step 1: Scan for Debug Patterns

**Frontend (TypeScript/React)** - search in `src/frontend/`:
- `console.log`, `console.debug`, `console.info` statements
- `debugger` statements

**Backend (.NET)** - search in `src/backend/`:
- `Debug.WriteLine` statements
- Skip `src/backend/src/API/ConsoleSimulation/` directory (intentional console output)

**Skip these (likely intentional)**:
- Test files (`*.test.ts`, `*.spec.ts`, `*Tests.cs`)
- `console.error` and `console.warn` (error handling)
- Proper logging via `ILogger`

### Step 2: Categorize Findings

**Safe to remove** (high confidence):
- `console.log` with string literals containing: "debug", "test", "here", "xxx", "TODO"
- `debugger` statements
- `Debug.WriteLine` calls

**Needs review** (ask user):
- `console.log` with variable output
- `console.log` in error handlers or catch blocks
- Any other ambiguous cases

### Step 3: Present Findings to User

Display findings grouped by category:

```
## Safe to Remove (X items)
1. [file:line] console.log("debug message")
2. [file:line] debugger

## Needs Review (Y items)
3. [file:line] console.log("Connection state:", state)
```

### Step 4: Get User Approval

Use AskUserQuestion to let user select which items to remove.
Present options:
- Remove all "Safe to Remove" items
- Review each "Needs Review" item individually
- Skip cleanup

### Step 5: Remove Approved Items

For each approved item:
1. Read the file
2. Remove the line (or statement if multi-line)
3. Save the file

### Step 6: Run Verification

After all deletions:

**Frontend**:
```bash
cd src/frontend && npm run type-check && npm run build
```

**Backend**:
```bash
dotnet build src/backend/NEMT.sln && dotnet test src/backend/NEMT.sln
```

If verification fails:
1. Report which file/change likely caused the failure
2. Suggest reverting that specific change
3. Do NOT automatically revert (let user decide)

### Step 7: Report Results

Summarize:
- Items removed
- Items skipped
- Verification status
- Any warnings or issues
