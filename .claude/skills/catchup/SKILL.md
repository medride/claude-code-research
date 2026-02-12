---
name: catchup
description: Start-of-session skill — loads context, shows progress, recommends next action. Run this when resuming work.
---

# Catchup

Loads context at session start — reads tracking files, shows progress, and recommends the next action. Read-only except optional native task hydration.

## Usage

```
/catchup
```

No arguments required. Everything is auto-detected and guided through prompts.

---

## Workflow

### 1. Read Status Index
- Read `external-memory/status.md`
- Parse the workstream table
- Categorize each feature as **Active** (not marked COMPLETE) or **Completed**
- If `status.md` is missing: error with guidance ("Create external-memory/status.md with your active workstreams")

### 2. Select Feature
- If no active workstreams: report completed features, suggest starting new work. Stop here.
- If single active workstream: auto-select it
- If multiple active workstreams, use AskUserQuestion:
  ```
  Question: "Which feature are you working on?"
  Header: "Feature"
  Options: [list active features with their current status]
    - "React CRUD Admin Panel" | "Phase 6 — Search Toolbar"
    - "Dark Mode + Filters" | "Planning complete"
  ```

### 3. Load Master Plan
- Read the selected feature's `master-plan.md` (path from `status.md`)
- Extract and display:
  - **Current status** — phase and task in progress
  - **Phase overview** — checklist with completion markers and `<-- YOU ARE HERE` indicator on active phase
  - **Last 3 session log entries** — most recent sessions
  - **Open blockers** — from Blockers section (if any)

### 4. Check Task Progress
- Identify the active phase directory from master-plan's "Current Status"
- If the phase directory exists in `external-memory/tasks/[domain]/`:
  - List all `task-*.md` files
  - For each task, parse `## Acceptance Criteria`:
    - All `[x]` → complete
    - Mixed → in progress (show X/Y)
    - All `[ ]` → pending
  - Display per-task progress with indicators:
    - `[x]` for complete
    - `[~]` for in progress (with criteria count)
    - `[ ]` for pending
  - Mark the first in-progress or pending task with `<-- IN PROGRESS` or `<-- NEXT`
- If no task directory exists: note this for recommendations

### 5. Check Git State
- Run `git branch --show-current` to get current branch
- Compare with expected branch from `status.md`
- If mismatch: warn but do NOT switch branches
  ```
  ⚠ Branch mismatch: on 'main' but feature expects 'claude-preparation'
  ```
- Run `git status --short` to check for uncommitted changes
- If uncommitted changes exist: note them, suggest reviewing
  ```
  ⚠ Uncommitted changes detected (X files). Consider reviewing before starting.
  ```

### 6. Offer Native Task Hydration

Scan all task directories across all domains (`external-memory/tasks/*/phase-*/`) for task files. If task files exist, use AskUserQuestion:

**If active phase has task files:**
```
Question: "Recreate native task tracking from file state?"
Header: "Task Tracking"
Options:
  - "Hydrate active phase (Recommended)" | "Hydrate [active-phase-name] (N tasks)"
  - "Hydrate different phase" | "Choose from all available phase directories"
  - "Skip" | "File-based tracking only"
```

**If active phase has no task files but other phases do:**
```
Question: "Recreate native task tracking from file state?"
Header: "Task Tracking"
Options:
  - "Hydrate a phase" | "Choose from available phase directories"
  - "Skip" | "File-based tracking only"
```

**If "Hydrate different phase" or "Hydrate a phase" selected:**
Show a follow-up AskUserQuestion listing all available phase directories:
```
Question: "Which phase to hydrate?"
Header: "Phase"
Options: [list each directory with task counts]
  - "backend/phase-01-setup" | "5 tasks (2 completed, 1 in_progress, 2 pending)"
  - "frontend/phase-01-components" | "3 tasks (0 completed)"
```

**Hydration logic (same for any selected phase):**
1. Read `overview.md` in the selected directory to get task list
2. For each task file, parse `## Acceptance Criteria` checkboxes to determine status:
   - All `[x]` → status: `completed`
   - Some `[x]` → status: `in_progress`
   - All `[ ]` → status: `pending`
3. Call TaskCreate for each task (subject from `# Task:` header, description from `## Objective`, activeForm in present continuous)
4. Parse `## Dependencies > ### Tasks` and set `blockedBy` relationships via TaskUpdate
5. Update task files with `## Native Task` section containing the new ID
6. If existing `## Native Task` section found, overwrite with fresh ID
7. Report: "Hydrated N tasks (X completed, Y in_progress, Z pending)"

**If "Skip":** proceed without hydration

### 7. Recommend Next Action
Based on the current state, recommend one of:

| State | Recommendation |
|-------|---------------|
| Task in progress (mixed criteria) | "Continue: [task file path]" |
| Task complete, next task pending | "Next task: [task file path]" |
| All tasks in phase complete | "Run `/complete phase` to finalize" |
| Phase file exists, no task directory | "Run `/task-planning-workflow` to create tasks" |
| No phase files in `external-memory/phases/` | "Run `/create-phases` to generate phases from plan" |
| All phases complete | "All phases done. Mark feature complete with `/complete all`" |
| No active work at all | "Start new work: create a plan in plan mode" |

### 8. Report
Display a concise summary formatted like:

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
[Feature Name]
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Status: Phase [N] — [Phase Name]
Branch: [current branch] [✓ matches | ⚠ expected: X]

Phase Progress:
  [x] Phase 1-N (complete)
  [ ] Phase N+1 - [Name]    <-- YOU ARE HERE
  [ ] Phase N+2 - [Name]

Phase [N+1] Tasks:
  [x] task-01-name               (5/5)
  [~] task-02-name               (2/4)  <-- IN PROGRESS
  [ ] task-03-name               (0/6)

Recent Sessions:
  [date] | [summary]
  [date] | [summary]
  [date] | [summary]

Recommended: [action]
```

---

## What `/catchup` Reads vs Modifies

| Action | Files |
|--------|-------|
| **Always reads** | `status.md`, selected `master-plan.md`, task file acceptance criteria |
| **Conditionally reads** | Phase `overview.md`, phase file in `phases/` |
| **Does NOT read** | Completed phases, full task content, code files |
| **Only modifies (with permission)** | Task files (Native Task section during hydration) |

---

## Edge Cases

| Scenario | Handling |
|----------|----------|
| `status.md` missing | Error: "Create external-memory/status.md with your active workstreams" |
| No active workstreams | Report completed features, suggest starting new work |
| Master plan path in status.md doesn't exist | Error: "Master plan not found at [path]" |
| Branch mismatch | Warn but don't switch |
| Uncommitted changes | Note them, suggest reviewing |
| Phase file exists but no task directory | Recommend `/task-planning-workflow` |
| No phase files | Recommend `/create-phases` |
| All phases complete | Suggest marking feature complete |
| Multiple features share same master-plan | Show both features, let user pick |
| Session Log section missing in master-plan | Note "No session history" in report |
| Task files have no acceptance criteria | Show as `(?/?)` with warning |
| User selects non-active phase for hydration | Hydrate that phase's tasks normally |
| Hydrate finds existing Native Task section | Overwrite with fresh ID |
| No task directories exist anywhere | Skip hydration step entirely |

---

## Examples

### Single active feature
```
> /catchup

Reading status.md...
  Active: React CRUD Admin Panel (Phase 6)
  Completed: Data Model Entities, Consistency Backlog

Loading master plan...

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
React CRUD Admin Panel
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Status: Phase 6 — Search Toolbar with Text Search
Branch: claude-preparation ✓

Phase Progress:
  [x] Phase 1-5 (complete)
  [ ] Phase 6 - Search Toolbar    <-- YOU ARE HERE
  [ ] Phase 7 - Faceted Filters

Phase 6 Tasks:
  [x] task-01-useDebounce-hook       (5/5)
  [~] task-02-data-table-toolbar     (2/4)  <-- IN PROGRESS
  [ ] task-03-wire-search-list-pages (0/6)

Recent Sessions:
  2026-02-12 | Phase 5 COMPLETE. ThemeProvider + ModeToggle. build passes.

[AskUserQuestion: Task Tracking]
> User selects: "Hydrate"

Hydrated 3 tasks (1 completed, 1 in_progress, 1 pending)

Recommended: Continue task-02-data-table-toolbar.md
```

### Multiple active features
```
> /catchup

Reading status.md...
  Active: React CRUD Admin Panel, Dark Mode + Filters

[AskUserQuestion: Feature]
> User selects: "Dark Mode + Filters"

Loading master plan...
[displays status report]

Recommended: Run /task-planning-workflow to create tasks for Phase 6
```

### Hydrating a different phase
```
> /catchup

[... status report ...]

[AskUserQuestion: Task Tracking]
> User selects: "Hydrate different phase"

[AskUserQuestion: Phase]
  - "backend/phase-01-setup" | "5 tasks (2 completed, 1 in_progress, 2 pending)"
  - "backend/phase-02-auth" | "3 tasks (0 completed)"
> User selects: "backend/phase-02-auth"

Hydrated 3 tasks (0 completed, 0 in_progress, 3 pending)

Recommended: Continue task-02-data-table-toolbar.md
```

### No active work
```
> /catchup

Reading status.md...
  No active workstreams found.
  Completed: Data Model Entities, Consistency Backlog, React CRUD Admin Panel

Recommended: Start new work — create a plan in plan mode.
```
