---
name: create-phases
description: Bridges plan mode output to external-memory structure. Extracts phases from approved plans and creates individual phase files for task-planning-workflow.
---

# Create Phases

Bridges plan mode output to the external-memory structure. Takes an approved plan containing multiple phases and creates individual phase files that `/task-planning-workflow` can process.

## Usage

```
/create-phases
```

No arguments required. Interactive prompts guide you through every decision.

---

## Workflow

### 1. Locate Plan File

**Scan for plan files:**
- List files in `~/.claude/plans/` directory
- Find all `.md` files

**If multiple plans found:**
Use AskUserQuestion:
```
Question: "Which plan would you like to process?"
Header: "Plan"
Options: [list each plan with modification time]
```

**If single plan found:**
- Auto-select it (no prompt needed)
- Inform user: "Found: ~/.claude/plans/[name].md"

**If no plans found:**
- Error: "No plan files found in ~/.claude/plans/. Create a plan first using plan mode."

### 2. Parse Plan Structure

Read the plan file and extract:

#### Feature Name
Look for patterns (in order of preference):
- `# Plan: [Feature Name]`
- `# [Feature Name]`
- First heading that isn't a section header

#### Domain Detection
Analyze plan content for keywords:

| Keywords | Domain |
|----------|--------|
| backend, API, .NET, C#, Entity Framework, controller, service, SignalR | `backend` |
| frontend, React, UI, component, TypeScript, Vite, Zustand | `frontend` |
| UX, UI, design, layout, styling, CSS, Tailwind | `uxui` |

**If confident in detection (strong keyword match):**
- Auto-select domain
- Inform user: "Detected domain: backend"

**If unclear or mixed signals:**
Use AskUserQuestion:
```
Question: "Which domain is this feature for?"
Header: "Domain"
Options:
  - "backend" | ".NET, API, database, services"
  - "frontend" | "React, UI components, styling"
  - "fullstack" | "Both backend and frontend phases"
```

#### Phases
Find all phase sections using these patterns:
```
## Phase 1: Name
## Phase 1 - Name
## Phase 1. Name
### Phase 1: Name
```

Regex pattern: `^#{2,3}\s*Phase\s*(\d+)[\s:\-\.]+(.+)$`

For each phase, extract:
- Phase number
- Phase name
- All content until next phase header or end of file

**If no phases found:**
- Error: "No phases found. Ensure plan has `## Phase N: Name` sections"

**If single phase found:**
Use AskUserQuestion:
```
Question: "Only 1 phase found. Single-phase plans don't usually need this workflow. Continue anyway?"
Header: "Confirm"
Options:
  - "Continue" | "Create the single phase file"
  - "Cancel" | "Stop and restructure plan into multiple phases"
```

### 3. Extract Phase Content

For each phase section, parse:

#### Objectives
Look for:
- `### Objectives` or `### Goals` section
- Bullet points starting with action verbs (Implement, Create, Add, Build, etc.)
- Numbered lists with deliverables

#### Tasks (High-Level)
Look for:
- `### Tasks` section
- Numbered or bulleted task lists
- Work items described in the phase

#### Dependencies
Look for:
- `### Dependencies` or `### Prerequisites` section
- References to other phases or external requirements

#### Acceptance Criteria
Look for:
- `### Acceptance Criteria` or `### Success Criteria` section
- Checklist items (`- [ ]`)
- If not found, mark as "TBD - define before starting"

### 4. Preview and Confirm

Before creating any files, use AskUserQuestion:
```
Question: "Ready to create phase files?"
Header: "Confirm"
Options:
  - "Create files (Recommended)" | "Create X phase files and update master-plan"
  - "Preview first" | "Show what will be created without writing"
  - "Cancel" | "Don't create anything"
```

**If "Preview first" selected:**
Show what would be created:
```
Will create:
  - external-memory/phases/phase-1-database-setup.md
  - external-memory/phases/phase-2-api-implementation.md
  - external-memory/tasks/backend/master-plan.md

Will update:
  - external-memory/status.md (add feature row)

Phase 1 - Database Setup:
  Objectives: 3 items
  Tasks: 4 items
  Acceptance Criteria: 2 items

Phase 2 - API Implementation:
  Objectives: 2 items
  Tasks: 5 items
  Acceptance Criteria: TBD
```

Then prompt again to create or cancel.

### 5. Handle Existing Files

For each phase file that already exists, use AskUserQuestion:
```
Question: "Phase file already exists: phase-1-setup.md"
Header: "Conflict"
Options:
  - "Overwrite" | "Replace with new content"
  - "Skip this phase" | "Keep existing, continue with others"
  - "Cancel" | "Stop and review manually"
```

### 6. Create Phase Files

For each phase found, create: `external-memory/phases/phase-[N]-[slug].md`

**Slug generation:**
- Lowercase the phase name
- Replace spaces and special characters with hyphens
- Remove consecutive hyphens
- Example: "Database Setup & Migrations" → "database-setup-migrations"

**Phase file format:**
```markdown
# Phase [N]: [Name]

## Context
[From plan's context/background section if present, otherwise generate brief summary]

## Objectives
- [ ] [Extracted objective 1]
- [ ] [Extracted objective 2]

## Tasks (High-Level)
[Extracted from phase content - these will become task files via /task-planning-workflow]
- [Task 1 description]
- [Task 2 description]

## Dependencies
- [Prerequisites from plan]
- [Cross-phase dependencies]

## Acceptance Criteria
- [ ] [Extracted or "TBD - define before starting"]
```

### 7. Create/Update Master Plan

**Path:** `external-memory/tasks/[domain]/master-plan.md`

#### If master-plan.md doesn't exist:
Create from template at `.claude/skills/task-planning-workflow/assets/master-plan.md`:
- Set `[Project Name]` to feature name
- Populate Phase Overview with all extracted phases
- Set Current Status to "Phase 1 - [Name]"
- Add Quick Context summary
- Add session log entry with creation date

#### If master-plan.md exists:
- Add new phases to Phase Overview (as unchecked items)
- Update Quick Context with new feature scope
- Add session log entry noting phases added

### 8. Update status.md

**Path:** `external-memory/status.md`

Add new row to the Active Workstreams table:
```markdown
| [Feature Name] | external-memory/tasks/[domain]/master-plan.md | main | Planning complete |
```

### 9. Report Summary

Output the following:
```
Phases created from: [plan-file-path]
Domain: [domain]
Feature: [feature-name]

Created files:
  ✓ external-memory/phases/phase-1-[slug].md
  ✓ external-memory/phases/phase-2-[slug].md
  ✓ external-memory/phases/phase-3-[slug].md
  ✓ external-memory/tasks/[domain]/master-plan.md [created|updated]
  ✓ external-memory/status.md [updated]

Next step: /task-planning-workflow
```

---

## Edge Cases

| Scenario | Handling |
|----------|----------|
| No plans found | Error with guidance to use plan mode first |
| Multiple plans found | Prompt user to select which plan |
| No phases found in plan | Error: "No phases found. Ensure plan has `## Phase N: Name` sections" |
| Single phase detected | Confirm with user before proceeding |
| Phase file already exists | Prompt: Overwrite, Skip, or Cancel |
| Domain unclear from content | Prompt user to select domain |
| master-plan.md exists with different feature | Add phases to existing, update Quick Context |
| No acceptance criteria in phase | Set to "TBD - define before starting" |
| Empty plan file | Error: "Plan file is empty or unreadable" |

---

## Example Session

```
> /create-phases

Found: ~/.claude/plans/auth-system.md (only plan)

Parsing plan...
  Feature: User Authentication System
  Phases found: 3

Detected domain: fullstack (mixed backend + frontend keywords)

[AskUserQuestion: Domain - prompted because mixed signals]
> User selects: "fullstack"

[AskUserQuestion: Confirm]
Will create:
  - external-memory/phases/phase-1-database-models.md
  - external-memory/phases/phase-2-authentication-api.md
  - external-memory/phases/phase-3-frontend-integration.md
  - external-memory/tasks/backend/master-plan.md (create)
  - external-memory/tasks/frontend/master-plan.md (create)
  - external-memory/status.md (update)

> User selects: "Create files"

Creating phase files...
  ✓ phase-1-database-models.md
  ✓ phase-2-authentication-api.md
  ✓ phase-3-frontend-integration.md
  ✓ external-memory/tasks/backend/master-plan.md
  ✓ external-memory/tasks/frontend/master-plan.md
  ✓ status.md updated

Done! Next step: /task-planning-workflow
```

---

## Integration with Workflow

**Full workflow after plan approval:**

| Step | Tool/Skill | Input | Output |
|------|------------|-------|--------|
| 1 | Plan mode | User request | `~/.claude/plans/[name].md` |
| 2 | `/create-phases` | (interactive) | `external-memory/phases/*.md` + master-plan |
| 3 | `/task-planning-workflow` | (interactive) | `external-memory/tasks/[domain]/phase-XX/` with task files |
| 4 | Implementation | Task files | Code changes |
| 5 | `/complete` | Task/phase path | Updated tracking, commits |

---

## Directory Structure After Running

```
external-memory/
├── status.md                           # Updated with new feature
├── phases/
│   ├── phase-1-database-setup.md       # Created
│   ├── phase-2-api-implementation.md   # Created
│   └── phase-3-frontend-integration.md # Created
└── completed/                          # (unchanged)

└── tasks/
    └── backend/
        └── master-plan.md              # Created or updated
```

---

## Verification

After creating phases, verify:

1. **Files created correctly:**
   - Phase files exist in `external-memory/phases/`
   - Each phase file has proper structure (Context, Objectives, Tasks, etc.)

2. **Master plan updated:**
   - `external-memory/tasks/[domain]/master-plan.md` exists
   - Phase Overview lists all phases with checkboxes
   - Current Status points to Phase 1

3. **Status.md updated:**
   - New feature row in Active Workstreams table

4. **Ready for next step:**
   - `/task-planning-workflow` should work
