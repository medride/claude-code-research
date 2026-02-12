# Claude Code Project Workflow

A structured workflow for Claude Code that takes projects from planning through implementation using persistent memory, phase-based development, and task tracking.

## How It Works

```
Plan  -->  Review  -->  Create Phases  -->  Task Planning  -->  Implement  -->  Complete
```

All project state lives in markdown files under `external-memory/`, so context carries across sessions automatically.

---

## Workflow

### 1. Start a Plan

Enter plan mode in Claude Code to define what you want to build. Claude collaborates with you to create a plan file.

```
> Enter plan mode (or use the plan mode shortcut)
> Describe what you want to build
> Iterate until the plan is solid
> Approve the plan
```

The plan is saved to `~/.claude/plans/`.

### 2. Review the Plan

Run the `/review-plan` skill. This spawns a staff engineer agent that critically reviews your plan against six criteria:

- Convention alignment
- Completeness
- Risks and edge cases
- Conflicts with other workstreams
- Complexity assessment
- Assumptions

If there are concerns, you can iterate on the plan and re-review until it's approved.

### 3. Create Phases

Run the `/create-phases` skill. This reads your approved plan and breaks it into individual phase files.

**What it creates:**
- Phase files in `external-memory/phases/` (one per phase)
- A `master-plan.md` for tracking the feature
- An entry in `external-memory/status.md`

### 4. Task Planning

Run the `/task-planning-workflow` skill. Select which phase you want to work on, and it converts that phase into actionable task files.

**What it creates:**
- A phase overview file with objectives and task table
- Individual task files, each with:
  - Dependencies
  - Objective
  - Acceptance criteria (checkboxes)
  - Verification steps

Tasks are scoped to be small and focused - each one is completable in a session.

### 5. Implement

Work through the tasks one by one. Each task file tells you exactly what needs to be done and how to verify it.

1. Open the task file for context
2. Implement the changes
3. Run verification (tests, build)
4. Check off acceptance criteria as you go
5. Update `master-plan.md` with progress

### 6. Complete

Run `/complete` when you're done. By default it auto-detects what's ready to complete (task, phase, or feature) and handles it. You can also be explicit:

| Command | When to use |
|---------|-------------|
| `/complete` | **Auto-detect** — figures out what's done and completes it |
| `/complete task` | Finished a single task |
| `/complete phase` | All tasks in a phase are done |
| `/complete all` | Entire feature is finished |
| `/complete session` | Stopping mid-task, need to hand off to next session |

Completion validates acceptance criteria, updates tracking files, and commits changes. Phase completion archives everything to `external-memory/completed/`.

---

## Session Management

### Starting a Session

Run `/catchup` at the start of every session. It:

1. Reads `status.md` to find active workstreams
2. Loads your feature's `master-plan.md`
3. Shows task progress (how many acceptance criteria are checked)
4. Checks git state
5. Recommends what to do next

### Ending a Session

- **Work is done:** Run `/complete` — it auto-detects what's ready to complete
- **Stopping mid-task:** Run `/complete session` to log a summary for the next session

---

## File Structure

```
external-memory/
  status.md              # Index of active features/workstreams
  phases/                # Active phase files
    phase-1-setup.md
    phase-2-api.md
  tasks/                 # Task structures organized by domain
    backend/
      master-plan.md     # Feature tracking, session log, decisions
      phase-01-setup/
        overview.md      # Phase objectives and task table
        task-01-models.md
        task-02-routes.md
  completed/             # Archived finished phases
    my-feature/
      phase-01-setup/
```

---

## Quick Reference

| Step | Skill | What happens |
|------|-------|-------------|
| Plan | Plan mode (built-in) | Create and approve a plan |
| Review | `/review-plan` | Staff engineer reviews the plan |
| Phases | `/create-phases` | Plan becomes phase files |
| Tasks | `/task-planning-workflow` | Phase becomes task files |
| Implement | (manual) | Build using task files as guides |
| Complete | `/complete` | Update tracking, commit, archive |
| Resume | `/catchup` | Load context, see progress, get recommendation |

---

## Extra Skills

The 5 core skills (`catchup`, `complete`, `create-phases`, `review-plan`, `task-planning-workflow`) power the workflow. Any additional skills your team creates should go in `.claude/skills/extras/`.

```
.claude/skills/
  catchup/                    # Core workflow skills
  complete/
  create-phases/
  review-plan/
  task-planning-workflow/
  extras/                     # Team-specific skills
    api-design/               # Example: API design patterns reference
      SKILL.md
    your-custom-skill/
      SKILL.md
```

Extra skills are reference material or utilities that don't belong in the core workflow — things like coding standards, design pattern guides, or project-specific helpers. Add whatever your team needs.

---

## Native Tool Integration

The skills leverage Claude Code's built-in tools to keep you in the loop:

- **Tasks** — `/task-planning-workflow` and `/catchup` can create native task entries so you see progress tracked in Claude Code's task list, not just in markdown files
- **AskUserQuestion** — Skills prompt you with structured choices at decision points (selecting a phase, choosing a domain, confirming before creating files) rather than assuming defaults

This means the workflow is interactive — skills guide you through each step instead of running silently.

---

## Setup

1. Copy the `.claude/` directory and `external-memory/` directory into your project
2. Fill in the commented-out sections in `CLAUDE.md` with your project's details (tech stack, architecture, file structure, conventions, verification commands)
3. Start planning
