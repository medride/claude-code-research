# Task: [Task Name]

## Context Required
<!-- Helps orchestrator assemble the right context for subagents -->
- master-plan: [yes/no] <!-- yes for: architecture, UI/UX, cross-cutting concerns -->
- phase-overview: yes
- conventions: [yes/no] <!-- yes if following established patterns matters -->

## Dependencies
<!-- What must exist before this task starts -->

### Tasks
- [task-XX must be complete]

### Requires From Codebase
- [e.g., AuthContext and useAuth hook must exist]
- [e.g., API client configured in src/services/api.ts]

## Objective
[Single clear goal for this task]

## Implementation Notes
[Any specific guidance, constraints, or patterns to follow. Keep minimal - trust Claude's judgment.]
<!-- For tasks creating controllers/endpoints, add: "Follow API conventions from `.claude/skills/api-design/SKILL.md` (response envelope, pagination, naming, PUT for updates)" -->

## Files Likely Modified
- `src/path/to/file.ts`
- `src/path/to/another.ts`

## Acceptance Criteria
- [ ] Criterion 1
- [ ] Criterion 2
- [ ] Criterion 3

## Verification
- [ ] `dotnet build` passes (backend)
- [ ] `dotnet test` passes (backend)
- [ ] `npm run type-check` passes (frontend)
- [ ] [Specific verification for this task]

## Native Task
<!-- Auto-generated when native task tracking is enabled. Session-specific. -->
<!-- Run /task-planning-workflow and select "Hydrate existing tasks" if starting a new session. -->
ID:
Status: pending
