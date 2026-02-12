# Workflow Changes

Summary of changes made to the Claude Code project workflow for team reference.

---

## 1. Post-Plan Approval Pipeline

**Where:** `CLAUDE.md`

After a plan is approved in plan mode, you must now run `/create-phases` before writing any code. This ensures phase files exist in `external-memory/phases/` so that `/task-planning-workflow` has something to process.

Previously, there was no enforced step between plan approval and implementation — tasks could be started without phases or acceptance criteria.

---

## 2. Ticket Numbers in Commits

**Where:** `/complete` skill, `.claude/rules/commits.md`

The `/complete` skill now prompts for an optional Jira ticket number during completion. If provided, it gets prepended to the commit message subject line:

```
PROJ-123 feat(backend): complete Phase 1 — Database Models
```

The commit rules in `.claude/rules/commits.md` were updated to document this `[TICKET]` prefix format.

---

## 3. Session Handoff Moved into `/complete`

**Where:** `/complete` skill

Session handoff is now a sub-command of the `/complete` skill: `/complete session`. This logs a session summary to the feature's `master-plan.md` session log table and commits tracking files — without marking any tasks or phases as done.

Previously, session handoff was a separate `/handoff` command. It's now consolidated under `/complete` alongside task, phase, and feature completion.

---

## 4. New `/catchup` Skill

**Where:** `.claude/skills/catchup/`

Created a new skill for starting sessions. Run `/catchup` at the beginning of every new session. It:

- Reads `status.md` to find active workstreams
- Loads the feature's `master-plan.md`
- Parses task files to show acceptance criteria progress
- Checks git branch and uncommitted changes
- Offers native task hydration (see change #5)
- Recommends the next action

This replaces the older `/catch-up` command with a more comprehensive, interactive flow.

---

## 5. Task Hydration Moved to `/catchup`

**Where:** `/catchup` skill, `/task-planning-workflow` skill

Native task hydration (recreating Claude Code's built-in task tracking from markdown files) has been removed from `/task-planning-workflow` and consolidated into `/catchup`.

The `/catchup` skill now supports hydrating any phase — not just the active one. It scans all task directories across all domains and lets you pick which phase to hydrate.

Previously, `/task-planning-workflow` had a separate "Hydrate Mode" that duplicated the same logic. Since hydration is a session-start concern (native tasks are ephemeral), it belongs in `/catchup`.

---

## 6. Feature-Named Directories in `completed/`

**Where:** `/complete` skill

When completing a phase, the `/complete` skill now uses the feature name from `status.md` to create the archive directory structure:

```
external-memory/completed/
  react-crud-admin-panel/        # ← feature name from status.md
    phase-01-database-models/
      overview.md
      task-01-user-entity.md
      ...
```

Previously, completed phases were not organized by feature name, making it harder to find archived work when multiple features existed.

---

## 7. Extra Skills Directory

**Where:** `.claude/skills/extras/`

Added a separate `extras/` directory inside `.claude/skills/` to keep non-workflow skills organized. The 5 core workflow skills (`catchup`, `complete`, `create-phases`, `review-plan`, `task-planning-workflow`) stay at the top level. Any additional team-specific skills — reference material, design guides, coding standards — go in `extras/`.

```
.claude/skills/
  catchup/                    # Core workflow skills
  complete/
  create-phases/
  review-plan/
  task-planning-workflow/
  extras/                     # Team-specific skills
    api-design/
```

The `api-design` skill was moved into `extras/` as the first example of this pattern.
