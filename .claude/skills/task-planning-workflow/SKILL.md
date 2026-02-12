---
name: task-planning-workflow
description: Converts phase files into actionable task structures organized by domain. Use when an approved phase is ready for implementation.
---

# Task Planning Workflow

## Description
Converts a phase file from `external-memory/phases/` into an actionable task structure in `external-memory/tasks/`, organized by domain (backend, uxui, etc.).

## Usage

```
/task-planning-workflow
```

No arguments required. Interactive prompts guide you through every decision.

---

## Workflow

### 1. Select Phase File

**Scan for phase files:**
- List files in `external-memory/phases/` directory
- Find all `.md` files

**If multiple phases found:**
Use AskUserQuestion:
```
Question: "Which phase would you like to process?"
Header: "Phase"
Options: [list each phase with estimated task count from objectives]
  - "phase-1-database-models.md" | "3 objectives, ~4 tasks"
  - "phase-2-authentication-api.md" | "3 objectives, ~5 tasks"
```

**If single phase found:**
- Auto-select it (no prompt needed)
- Inform user: "Found: external-memory/phases/[name].md"

**If no phases found:**
- Error: "No phases found in external-memory/phases/. Run /create-phases first."

### 2. Analyze & Detect Domain

Analyze the phase content to determine which domains are involved:
- **Backend**: API changes, database, services, algorithms, .NET, C#
- **UX/UI**: Components, screens, styling, user flows, React, frontend

**If confident in detection:**
- Auto-select domain
- Inform user: "Detected domain: backend"

**If unclear or mixed:**
Use AskUserQuestion:
```
Question: "Which domain is this phase for?"
Header: "Domain"
Options:
  - "backend" | "API, database, services, algorithms"
  - "frontend" | "React components, UI, styling"
  - "fullstack" | "Both backend and frontend tasks"
```

### 3. Enable Native Task Tracking

Use AskUserQuestion:
```
Question: "Enable real-time task tracking?"
Header: "Tracking"
Options:
  - "Yes (Recommended)" | "Creates native tasks for progress monitoring"
  - "No" | "File-based tracking only"
```

### 4. Preview and Confirm

Before creating any files, use AskUserQuestion:
```
Question: "Ready to create task structure?"
Header: "Confirm"
Options:
  - "Create tasks (Recommended)" | "Create X task files with tracking"
  - "Preview first" | "Show task breakdown without creating"
  - "Cancel" | "Don't create anything"
```

**If "Preview first" selected:**
Show what would be created:
```
Will create:
  - external-memory/tasks/backend/phase-01-database-models/overview.md
  - external-memory/tasks/backend/phase-01-database-models/task-01-user-entity.md
  - external-memory/tasks/backend/phase-01-database-models/task-02-role-entity.md
  - external-memory/tasks/backend/phase-01-database-models/task-03-ef-config.md
  - external-memory/tasks/backend/phase-01-database-models/task-04-migration.md
  + 4 native tasks with dependencies (if tracking enabled)
```

Then prompt again to create or cancel.

### 5. Create Directory Structure

```
external-memory/tasks/[domain]/
├── master-plan.md                    # Create if doesn't exist, update if exists
└── phase-[NN]-[name]/
    ├── overview.md                   # Phase overview from template
    ├── task-01-[name].md             # Individual tasks
    ├── task-02-[name].md
    └── ...
```

#### Phase Numbering
- Use two digits: 00, 01, 02, etc.
- Check existing phases in `external-memory/tasks/[domain]/` to determine next number

### 6. Native Task Creation (if tracking enabled)

1. **First pass - Create tasks:**
   For each task file created, call TaskCreate with:
   - `subject`: Task name from `# Task: [Name]` header
   - `description`: Content of `## Objective` section
   - `activeForm`: Convert subject to present continuous (e.g., "Setup auth" → "Setting up auth")

2. **Second pass - Set dependencies:**
   - Parse each task's `## Dependencies > ### Tasks` section
   - Map task file references (e.g., "task-01") to native task IDs
   - Call TaskUpdate with `addBlockedBy` for each dependency

3. **Update task files:**
   Add `## Native Task` section to each task file with the assigned ID

### 7. Use Templates

Templates are located in this skill's `assets/` directory:
- `master-plan.md` - For creating/updating domain master plan
- `phase-overview.md` - For creating phase overview
- `task.md` - For creating individual task files

### 8. Fill Templates

#### master-plan.md
- Update "Current Status" with new phase
- Add new phase to "Phase Overview" checklist
- Update "Quick Context" with phase description
- Add entry to "Session Log"

#### phase-overview.md
- Fill "Context" with what was completed before
- Fill "Objectives" from phase file goals
- Fill "Tasks" with task file list
- Fill "Dependencies" from phase requirements
- Fill "Acceptance Criteria" from phase criteria
- Fill "Verification" with appropriate commands for this project:
  - Backend: `dotnet build`, `dotnet test`
  - Frontend: `npm run type-check`, `npm run build`

#### task.md (for each task)
- Fill "Context" with task prerequisites
- Fill "Objective" with single clear goal
- Fill "Implementation Notes" minimally (trust Claude's judgment)
- Fill "Files Likely Modified" with expected file paths
- Fill "Acceptance Criteria" - must be verifiable
- Fill "Verification" with specific checks

### 9. Task Granularity Guidelines
- **Too coarse**: "Implement authentication" - requires many assumptions
- **Too granular**: Step-by-step instructions - limits adaptability
- **Just right**: "Implement Supabase auth with sign-up, sign-in, and password reset flows following the service pattern"

### 10. Self-Contained Tasks
Each task file includes enough context to understand WHAT needs to be done:
- What was completed before this task (context)
- Clear single objective
- Files likely to be modified
- Acceptance criteria

**Important**: Self-contained does NOT mean skip the master-plan.md and overview.md. Always read those first for the bigger picture - tasks just ensure you have the specific context for that unit of work.

### 11. Context Required in Tasks

Each task declares what context it needs for orchestration efficiency:

```markdown
## Context Required
- master-plan: yes/no
- phase-overview: yes
- conventions: yes/no
```

Guidelines:

| Task Type | master-plan | conventions |
|-----------|-------------|-------------|
| Infrastructure/setup | yes | no (none yet) |
| New feature area | yes | yes |
| UI component | yes | yes |
| Bug fix | no | yes |
| Tests | no | yes |

### 12. Acceptance / Success Criteria Workflow

#### When Creating Tasks
- Collaborate with user to confirm acceptance/success criteria for each task
- Criteria must be verifiable (runnable commands, observable outcomes)
- A task CANNOT be marked complete until ALL success criteria are met

#### When Success Criteria Met
- Check off all acceptance criteria in the task file
- Update the phase `overview.md`:
  - Mark task as complete in Tasks list
  - Check off related objectives if applicable
- Update `master-plan.md` current status
- Commit changes following `.claude/rules/commits.md` format

#### When Phase is Complete
- Verify ALL tasks in phase have success criteria met
- Run phase-level verification steps
- Update phase overview - all objectives checked
- Update `master-plan.md`:
  - Mark phase as [x] complete
  - Update "Current Status" to next phase
  - Add session log entry


### 13. Update External Memory Status
If adding a new feature/workstream, update `external-memory/status.md` index to include it:
- Add a row with feature name, master-plan.md path, branch, and status

Detailed status tracking goes in the feature's `master-plan.md`, not in `status.md`.

### 14. Report Summary

```
Task structure created from: [phase-file-path]
Domain: [domain]
Phase: [phase-name]

Created files:
  ✓ external-memory/tasks/[domain]/phase-[NN]-[name]/overview.md
  ✓ external-memory/tasks/[domain]/phase-[NN]-[name]/task-01-[name].md (Native task #1)
  ✓ external-memory/tasks/[domain]/phase-[NN]-[name]/task-02-[name].md (Native task #2)
  ✓ external-memory/tasks/[domain]/phase-[NN]-[name]/task-03-[name].md (Native task #3, blocked by #1, #2)
  ✓ external-memory/tasks/[domain]/master-plan.md [created|updated]

Start with: external-memory/tasks/[domain]/phase-[NN]-[name]/task-01-[name].md
```

---

## Output

After running this skill, you will have:
- `external-memory/tasks/[domain]/master-plan.md` - Created or updated
- `external-memory/tasks/[domain]/phase-[NN]-[name]/overview.md` - Phase overview
- `external-memory/tasks/[domain]/phase-[NN]-[name]/task-[NN]-[name].md` - Individual tasks
- Native tasks created (if tracking enabled) with dependencies set

---

## Working on Tasks (After Structure Created)
1. Read `master-plan.md` to understand current status
2. Read the current phase's `overview.md`
3. Read the next task file
4. Complete the task
5. Run verification steps
6. Mark task complete in phase overview
7. Update master plan status

---

## Session Handoff
At the end of each session:
- Mark completed tasks in phase overview
- Update master plan "Current Status" section
- Note any blockers or decisions made in master plan

---

## Phase Transitions
When a phase is complete:
- Verify all tasks have success criteria met
- Run full verification (phase-level)
- Update phase overview - all objectives checked
- Update master plan:
  - Mark phase as [x] complete
  - Update "Current Status" to next phase
  - Add session log entry

---

## Agent Orchestration Mode

When running with agent orchestration:

### Task Assignment
- After creating the task structure, the orchestrator reads `overview.md` for the task list
- Each task file is self-contained and can be assigned to a general-purpose agent

### Dispatching Strategy
- **Sequential tasks** (have dependencies): Dispatch one agent at a time
- **Independent tasks** (no dependencies): Dispatch up to 3-4 agents in parallel
- Check task "Context" section for dependencies between tasks

### Agent Instructions
When dispatching an agent for a task, provide:
1. Path to the master-plan.md for context
2. Path to the phase overview.md
3. Path to the task file (e.g., `external-memory/tasks/backend/phase-01-auth/task-01-service.md`)
4. Native task ID (if using native tasks)
5. Instructions:
   - Read master-plan → overview → task file
   - If native task ID provided: `TaskUpdate(taskId, status: "in_progress")`
   - Implement the task
   - Run verification steps
   - Check off acceptance criteria in task file
   - If native task ID provided: `TaskUpdate(taskId, status: "completed")`

### Progress Tracking (File-based)
- Orchestrator monitors task completion via acceptance criteria
- When agent completes, verify criteria are checked in task file
- Update `overview.md` with completed task
- Update `master-plan.md` current status

### Progress Monitoring (with Native Tasks)
When using native tasks, the orchestrator can:
1. Call `TaskList` to see all tasks with status and blockedBy
2. Identify unblocked pending tasks for dispatch
3. Monitor completion without reading files
4. Verify file state matches before phase completion

### Review Phase
After all tasks in a phase are implemented:
- Dispatch a review agent to verify:
  - All acceptance criteria met
  - Code quality and conventions followed
  - Phase-level verification passes
- Review agent updates phase overview and master plan

### Error Handling
- If an agent fails or gets stuck, assess the blocker
- Create a fix task if needed
- Update the feature's `master-plan.md` with blockers encountered

### Phase Completion
When all tasks are complete, follow the Phase Transitions workflow above.

---

## Edge Cases

| Scenario | Handling |
|----------|----------|
| No phases found | Error with guidance to run /create-phases first |
| Multiple phases found | Prompt user to select which phase |
| No dependencies section | Create native task with no blockedBy |
| Dependency references missing task | Log warning, skip that relationship |
| Cross-domain dependency | Log info, requires manual coordination |
| Circular dependencies | Detect and log error, skip cycle |

---

## Example Session

```
> /task-planning-workflow

Found phases in external-memory/phases/:
[AskUserQuestion: Phase]
> User selects: "phase-1-database-models.md"

Analyzing phase...
  Domain: backend (strong match: .NET, Entity Framework, database)
  Objectives: 3
  Estimated tasks: 4

[AskUserQuestion: Tracking]
> User selects: "Yes"

[AskUserQuestion: Confirm]
Will create:
  - external-memory/tasks/backend/phase-01-database-models/overview.md
  - external-memory/tasks/backend/phase-01-database-models/task-01-user-entity.md
  - external-memory/tasks/backend/phase-01-database-models/task-02-role-entity.md
  - external-memory/tasks/backend/phase-01-database-models/task-03-ef-config.md
  - external-memory/tasks/backend/phase-01-database-models/task-04-migration.md
  + 4 native tasks with dependencies

> User selects: "Create tasks"

Creating task structure...
  ✓ overview.md
  ✓ task-01-user-entity.md (Native task #1)
  ✓ task-02-role-entity.md (Native task #2)
  ✓ task-03-ef-config.md (Native task #3, blocked by #1, #2)
  ✓ task-04-migration.md (Native task #4, blocked by #3)

Done! Start with: external-memory/tasks/backend/phase-01-database-models/task-01-user-entity.md
```

