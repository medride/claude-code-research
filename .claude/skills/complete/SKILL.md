---
name: complete
description: Automates task, phase, and session completion workflows - updates all tracking files, commits changes.
---

# Complete

Triggers the completion workflow for tasks, phases, or session handoffs, updating all tracking files.

## Usage

No paths or arguments required. Everything is guided through prompts.

```
/complete           # Auto-detect what's ready and complete it
/complete task      # Pick a task to complete
/complete phase     # Pick a phase to complete
/complete all       # Complete all phases in a domain
/complete session   # Log session summary and commit (mid-task handoff)
```

That's it. You'll be guided through selection if needed.

---

## Options

No flags or arguments needed for options. Everything is guided through AskUserQuestion prompts:

**Verification prompt** (shown when a Verification section exists):
```
Question: "Run verification commands before completing?"
Header: "Verify"
Options:
  - "Run verification (Recommended)" | "Execute verification commands and check results"
  - "Skip verification" | "Complete without running verification"
```

**Commit prompt** (shown after all updates are done):
```
Question: "Commit the completion changes?"
Header: "Commit"
Options:
  - "Commit (Recommended)" | "Stage and commit all changes"
  - "Skip commit" | "Leave changes uncommitted"
```

**Force complete prompt** (shown only when acceptance criteria are incomplete):
```
Question: "Not all acceptance criteria are checked. Force complete?"
Header: "Force"
Options:
  - "Force complete" | "Mark all criteria as complete and proceed"
  - "Stop (Recommended)" | "Don't complete — fix incomplete criteria first"
```

**Jira ticket prompt** (shown before every commit):
```
Question: "Add a Jira ticket number to the commit?"
Header: "Jira"
Options:
  - "No ticket (Recommended)" | "Commit without a ticket reference"
  - "Add ticket" | "I'll enter the ticket number (e.g., PROJ-123)"
```

If "Add ticket" selected, use AskUserQuestion with free text to get the ticket number.
Prepend to commit subject: `[TICKET] type(scope): description`.
If the user already provided a ticket earlier in this session, offer "Use [TICKET] again" as the first option.

**Session summary prompt** (shown during `/complete session`):
```
Question: "How would you like to create the session summary?"
Header: "Summary"
Options:
  - "Draft summary (Recommended)" | "Auto-generate from git diff + task progress"
  - "Enter manually" | "Type your own summary"
```

---

## Smart Complete Workflow (no arguments)

When `/complete` is invoked with no arguments:

### 1. Find Active Domain
- Look in `external-memory/tasks/` for domains with master-plan.md files
- If multiple domains, use AskUserQuestion:
  ```
  Question: "Multiple domains found. Which one to complete?"
  Header: "Domain"
  Options: [list each domain found, e.g., "backend", "frontend"]
  ```
- If single domain, use it

### 2. Read Master Plan
- Read `external-memory/tasks/[domain]/master-plan.md`
- Find "Current Status" to identify active phase
- Find active phase directory

### 3. Determine What to Complete
- List all task files in active phase
- Check each task's acceptance criteria status:
  - All `[x]` = complete
  - Mixed = in progress
  - All `[ ]` = pending

### 4. Take Action
- **If all tasks complete**: Run phase completion workflow
- **If current task complete but others pending**: Complete the task, report next task
- **If nothing ready**: Report status ("Task X has 2/5 criteria complete"), then offer session mode:
  ```
  Question: "No tasks are ready to complete. End session instead?"
  Header: "Action"
  Options:
    - "End session (Recommended)" | "Log session summary and commit"
    - "Cancel" | "Keep working"
  ```
  If "End session" selected, run the Session Completion Workflow below.

---

## Task Completion Workflow

When `/complete task` is invoked:

### 1. Find the Task
- Find the active domain (same as Smart Complete step 1 — prompt if multiple)
- Read master-plan.md to find the active phase
- List all `task-*.md` files in the active phase directory
- Check each task's acceptance criteria status
- If multiple tasks are ready (all `[x]`), use AskUserQuestion:
  ```
  Question: "Which task to complete?"
  Header: "Task"
  Options: [list ready tasks, e.g., "task-01-models — Entity models", "task-02-services — Service layer"]
  ```
- If only one task is ready, auto-select it
- If no tasks are ready, report status (e.g., "task-01-models has 3/5 criteria complete")

### 2. Read and Validate Task File
- Read the selected task file
- Parse `## Acceptance Criteria` section
- Check if ALL criteria are marked `[x]`
- If not all checked: **STOP** and report which criteria are incomplete

### 3. Run Verification (Optional)
- Parse `## Verification` section
- Use AskUserQuestion:
  ```
  Question: "Run verification commands before completing?"
  Header: "Verify"
  Options:
    - "Run verification (Recommended)" | "Execute verification commands and check results"
    - "Skip verification" | "Complete without running verification"
  ```
- If "Run verification": execute each command and report results
- If any fail: **STOP** and report failure
- If "Skip verification": proceed to next step

### 4. Update Phase Overview
- Read `external-memory/tasks/[domain]/[phase-dir]/overview.md`
- Find the task in the Tasks list/table
- Mark it as complete: `[x]` or update Status column
- Write the updated file

### 5. Update Master Plan
- Read `external-memory/tasks/[domain]/master-plan.md`
- Update "Current Status" section to indicate task complete
- Optionally add session log entry with date

### 6. Update Native Task (if present)
- Check task file for `## Native Task` section
- If ID exists, call `TaskUpdate(taskId, status: "completed")`

### 7. Report Summary
Output:
```
Task completed: [task name]
  - Phase overview updated
  - Master plan updated
  - Native task updated (if applicable)
```

---

## Phase Completion Workflow

When `/complete phase` is invoked:

### 1. Find the Phase
- Find the active domain (same as Smart Complete step 1 — prompt if multiple)
- Read master-plan.md to find the active phase
- If multiple incomplete phases exist, use AskUserQuestion:
  ```
  Question: "Which phase to complete?"
  Header: "Phase"
  Options: [list incomplete phases, e.g., "Phase 1 — Setup", "Phase 2 — Integration"]
  ```
- If only one active phase, auto-select it

### 2. Verify All Tasks Complete
- List all `task-*.md` files in the phase directory
- For each task file:
  - Parse `## Acceptance Criteria`
  - Verify ALL criteria are `[x]`
- If any task incomplete: **STOP** and list incomplete tasks

### 3. Run Phase Verification
- Read `overview.md` and parse `## Verification` section
- Use AskUserQuestion:
  ```
  Question: "Run phase verification commands?"
  Header: "Verify"
  Options:
    - "Run verification (Recommended)" | "Execute all phase verification commands"
    - "Skip verification" | "Complete phase without running verification"
  ```
- If "Run verification": execute each command and report results
- If any fail: **STOP** and report
- If "Skip verification": proceed to next step

### 4. Update Phase Overview
- Mark all objectives as `[x]` complete
- Add completion date
- Update status to "COMPLETED"

### 5. Consolidate Phase File into Task Directory
- Look for matching phase file in `external-memory/phases/`
- Match by phase name/number (e.g., `phase-01-setup/` matches `phase-1-setup.md` or `phase-01-setup.md`)
- Copy the phase `.md` file **into** the task directory (alongside overview.md and task files)
- Remove the original from `external-memory/phases/`
- If not found: warn but continue

### 6. Move Entire Task Directory to Completed (Feature-Scoped)
1. Determine feature slug from `status.md`:
   - Read `external-memory/status.md` and find the Feature name for the active domain/master-plan
   - Convert feature name to kebab-case slug: lowercase, replace spaces/special chars with hyphens, remove consecutive hyphens
   - Examples: "Data Model Entities" → `data-model-entities`, "Dark Mode + Data Table Search & Filters" → `dark-mode-data-table-search-filters`
2. Create feature directory if it doesn't exist: `external-memory/completed/[feature-slug]/`
3. Move the entire phase task directory into it: `external-memory/completed/[feature-slug]/[phase-dir]/`
- This moves overview.md, all task-*.md files, and the consolidated phase .md file together as one unit
- The completed directory becomes a self-contained archive of the phase

**Note:** Existing flat completed directories are left as-is. Only new phase completions use the feature-scoped structure.

### 7. Update Master Plan
- Read `external-memory/tasks/[domain]/master-plan.md`
- Mark phase as `[x]` in Phase Overview checklist
- Update "Current Status" to next phase (or "All phases complete")
- Add session log entry with date and summary

### 8. Update CHANGELOG.md
- Read `CHANGELOG.md` from project root
- Find `[Unreleased]` section
- Add phase accomplishments as bullet points
- If no CHANGELOG.md exists: skip with warning

### 9. Check Feature Completion
- Read master-plan.md Phase Overview
- If ALL phases are `[x]`, use AskUserQuestion:
  ```
  Question: "All phases complete! Update status.md to mark feature as done?"
  Header: "Status"
  Options:
    - "Update status.md (Recommended)" | "Mark feature complete in status.md"
    - "Skip" | "Leave status.md unchanged"
  ```
- If "Update status.md": update `external-memory/status.md`

### 10. Jira Ticket (Optional)
- Use AskUserQuestion (Jira ticket prompt from Options section above)
- If "Add ticket": prompt for ticket number, prepend to commit subject
- If a ticket was already provided earlier in this session, offer "Use [TICKET] again" as first option

### 11. Commit Changes
- Use AskUserQuestion:
  ```
  Question: "Commit the completion changes?"
  Header: "Commit"
  Options:
    - "Commit (Recommended)" | "Stage and commit all changes"
    - "Skip commit" | "Leave changes uncommitted"
  ```
- If "Commit": stage all modified files, commit following `.claude/rules/commits.md` format with scope `[domain]` and description `complete [phase-name]`
- If Jira ticket provided: prepend ticket to commit subject (e.g., `PROJ-123 feat(frontend): complete Phase 5`)
- If "Skip commit": proceed to report

### 12. Report Summary
Output:
```
Phase completed: [phase name]
  - X tasks verified complete
  - Phase overview updated
  - Phase file consolidated into task directory
  - Task directory moved to external-memory/completed/[feature-slug]/
  - Master plan updated
  - CHANGELOG.md updated
  - Committed: [TICKET] [commit hash]
```

---

## Complete All Workflow

When `/complete all` is invoked:

### 1. Find Domain and Read Master Plan
- Find the active domain (same as Smart Complete step 1 — prompt if multiple)
- Read `external-memory/tasks/[domain]/master-plan.md`
- Parse "Phase Overview" to get list of all phases
- Identify which phases are already `[x]` complete

### 2. Process Each Incomplete Phase
For each phase not yet marked complete (in order):
1. Run the full Phase Completion Workflow (steps 1-11 above)
2. If any phase fails validation, **STOP** and report which phase/task failed

### 3. Update Feature Status
- After all phases complete, update `external-memory/status.md`
- Mark feature as "Complete" or remove from active work

### 4. Jira Ticket (Optional)
- Use AskUserQuestion (Jira ticket prompt from Options section)
- If "Add ticket": prompt for ticket number, prepend to commit subject

### 5. Final Commit
- Use AskUserQuestion:
  ```
  Question: "Commit the feature completion?"
  Header: "Commit"
  Options:
    - "Commit (Recommended)" | "Stage and commit all changes"
    - "Skip commit" | "Leave changes uncommitted"
  ```
- If "Commit": single commit following `.claude/rules/commits.md` format with scope `[domain]` and description `complete [feature-name]`
- If Jira ticket provided: prepend ticket to commit subject
- If "Skip commit": proceed to report

### 6. Report Summary
Output:
```
Feature completed: [domain]
  - Phase 1: [name] - done
  - Phase 2: [name] - done
  - Phase 3: [name] - done
  - status.md updated
  - Committed: [commit hash]
```

---

## Session Completion Workflow

When `/complete session` is invoked:

### 1. Find Active Domain
- Same as Smart Complete step 1 — look in `external-memory/tasks/` for domains with master-plan.md files
- If multiple domains, prompt with AskUserQuestion
- If single domain, auto-select
- If no active workstreams, offer "session handoff anyway" (commit-only) or cancel

### 2. Check Git Status
- Run `git status` and `git diff --stat`
- Show changed files to the user
- If unstaged changes exist, warn prominently:
  ```
  ⚠ Unstaged changes detected — these will NOT be included in the commit unless staged.
  ```
- If working tree is clean:
  ```
  Question: "Working tree is clean. Add session log entry anyway?"
  Header: "Action"
  Options:
    - "Add log entry" | "Update master-plan session log without committing"
    - "Cancel" | "Nothing to do"
  ```

### 3. Gather Session Summary
- Use AskUserQuestion (Session summary prompt from Options section):
  ```
  Question: "How would you like to create the session summary?"
  Header: "Summary"
  Options:
    - "Draft summary (Recommended)" | "Auto-generate from git diff + task progress"
    - "Enter manually" | "Type your own summary"
  ```
- If "Draft summary":
  - Read git diff (staged + unstaged) for a summary of code changes
  - Read active task files to check acceptance criteria progress
  - Generate a concise 1-2 sentence summary
  - Present for approval:
    ```
    Question: "Use this summary?"
    Header: "Approve"
    Options:
      - "Use this summary" | "[the generated summary]"
      - "Edit" | "I'll write my own version"
    ```
  - If "Edit": prompt for free-text input via AskUserQuestion
- If "Enter manually": prompt for free-text input via AskUserQuestion

### 4. Add Session Log Entry
- Read `external-memory/tasks/[domain]/master-plan.md`
- Find the `## Session Log` table
- Append a new row: `| [today's date] | [summary] |`
- If no Session Log section exists, append one:
  ```markdown
  ## Session Log
  | Date | Summary |
  |------|---------|
  | [today's date] | [summary] |
  ```

### 5. Jira Ticket (Optional)
- Use AskUserQuestion (Jira ticket prompt from Options section)
- If "Add ticket": prompt for ticket number, prepend to commit subject

### 6. Commit Changes
- Use AskUserQuestion:
  ```
  Question: "Commit the session changes?"
  Header: "Commit"
  Options:
    - "Commit all (Recommended)" | "Stage and commit all changes"
    - "Commit tracking only" | "Only commit external-memory/ tracking files"
    - "Skip commit" | "Leave changes uncommitted"
  ```
- If "Commit all": stage all modified files, commit
- If "Commit tracking only": stage only `external-memory/` files, commit
- If Jira ticket provided: prepend ticket to commit subject
- Commit message format: `chore([domain]): session handoff — [summary]`
- If "Skip commit": proceed to report

### 7. Report Summary
Output:
```
Session logged for: [domain]
  - Master plan session log updated
  - Summary: [summary]
  - Committed: [commit hash] (or "not committed")
```

### What `/complete session` Does NOT Do
- Does NOT mark tasks or phases as complete
- Does NOT update CHANGELOG.md (no deliverable to log yet)
- Does NOT move files to `completed/`
- Does NOT check acceptance criteria

---

## File Movement Reference

| File Type | From | To |
|-----------|------|-----|
| Phase .md file | `external-memory/phases/phase-X-name.md` | Consolidated into task directory, then moved with it |
| Task directory | `external-memory/tasks/[domain]/phase-XX-name/` | `external-memory/completed/[feature-slug]/phase-XX-name/` |
| Overview | Moves with task directory | `external-memory/completed/[feature-slug]/phase-XX-name/overview.md` |
| Task files | Move with task directory | `external-memory/completed/[feature-slug]/phase-XX-name/task-*.md` |
| Master plan | Stays in `external-memory/tasks/[domain]/master-plan.md` | (updated, not moved) |

### Feature Slug Generation
Derive `[feature-slug]` from the **Feature column in `status.md`**:
- Lowercase the feature name
- Replace spaces, `+`, `&`, and other special characters with hyphens
- Remove consecutive hyphens and trim leading/trailing hyphens

| status.md Feature | Completed Subdirectory |
|---|---|
| Data Model Entities | `completed/data-model-entities/` |
| Consistency Backlog | `completed/consistency-backlog/` |
| React CRUD Admin Panel | `completed/react-crud-admin-panel/` |
| Dark Mode + Data Table Search & Filters | `completed/dark-mode-data-table-search-filters/` |

### Completed Directory Structure
After phase completion, `external-memory/completed/[feature-slug]/phase-XX-name/` contains:
```
external-memory/completed/[feature-slug]/phase-XX-name/
├── phase-X-name.md          # Original phase file (consolidated from phases/)
├── overview.md               # Phase overview with all objectives checked
├── task-01-name.md           # Task files with all criteria checked
├── task-02-name.md
└── ...
```

**Note:** Existing flat directories in `completed/` are left as-is. Only new completions use feature-scoped structure.

---

## Edge Cases

| Scenario | Handling |
|----------|----------|
| Task criteria not all checked | Stop, list incomplete criteria |
| Verification fails | Stop, show failure output |
| Phase file not in external-memory/phases/ | Warn, continue without consolidating |
| CHANGELOG.md doesn't exist | Warn, skip CHANGELOG update |
| No master-plan.md | Error, cannot complete |
| Already completed task/phase | Warn, then AskUserQuestion: "Already complete. Proceed anyway?" — Options: "Re-complete" / "Cancel" |
| Completed directory already has phase-XX-name/ | AskUserQuestion: "Completed directory already exists." — Options: "Overwrite" / "Skip" / "Cancel" |
| Session: no active workstreams | Offer "session handoff anyway" (commit-only) or cancel |
| Session: git working tree clean | "Working tree clean. Add session log entry anyway?" |
| Session: unstaged changes | Warn prominently before commit |
| Session: session log section missing | Append new Session Log section to master-plan |
| Jira: invalid ticket format | Accept any string — don't validate format |
| Jira: same ticket for multiple commits | Offer "Use [TICKET] again" as first option |

---

## Examples

### Smart complete - just finish whatever is ready
```
/complete
```

### Complete a task - you'll be prompted to pick one
```
/complete task
```

### Complete a phase - you'll be prompted to pick one
```
/complete phase
```

### Complete entire feature (all phases)
```
/complete all
```

### End session (mid-task handoff)
```
/complete session
```

All five commands auto-discover your active work. No paths to memorize.

---

## Implementation Details

### Parsing Acceptance Criteria
When parsing `## Acceptance Criteria` sections:
- Look for lines matching `- [ ]` (incomplete) or `- [x]` (complete)
- Count total criteria and completed criteria
- Report progress as "X/Y criteria complete"

### Finding Phase Files
To match phase files in `external-memory/phases/`:
1. Extract phase number from directory name (e.g., `phase-01-setup` → `01`)
2. Look for files matching patterns:
   - `phase-1-*.md` or `phase-01-*.md`
   - Same phase name keywords
3. If multiple matches, prefer exact number match

### Git Commit Format
Follow `.claude/rules/commits.md` for format, type, and Co-Authored-By line. Use scope `[domain]` and a descriptive message. Include key accomplishments from phase objectives in the commit body.

### CHANGELOG.md Format
Add entries under `[Unreleased]`:
```markdown
## [Unreleased]

### Added
- [New feature from phase]

### Changed
- [Modification from phase]
```
