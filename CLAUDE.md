## Overview

Claude Code follows: **Plan → Implement → Verify → Review**

- Always use plan mode for non-trivial tasks
- Define acceptance criteria before starting — this is the most important thing you can provide

---

## Project

<!-- Add your own codebase description -->

<!-- **TimefoldInt — .NET 8 Clean Architecture API Scaffold**

A production-ready .NET 8 API scaffold implementing Clean Architecture with CQRS, async job processing, and multi-tenant support. Provides established patterns (Repository, Unit of Work, Specification) as a starting point for building APIs. -->

### Tech Stack

<!-- Add your own codebase tech stack -->

<!-- | Layer | Technologies |
|---|---|
| Backend | .NET 8, ASP.NET Core, Entity Framework Core 8 |
| CQRS & Messaging | MediatR, FluentValidation, AutoMapper |
| Database | PostgreSQL 15 |
| Job Processing | AWS SQS, AWS DynamoDB |
| Infrastructure | Docker Compose | -->

### Architecture

<!-- Add your own codebase architecture -->

<!-- Clean Architecture:
```
Domain (Core Entities & Business Logic)
    ↑
Application (CQRS Features, Contracts, Specifications)
    ↑
Persistence (EF Core, Repositories, Migrations)
    ↑
Infrastructure (AWS Services, Job Processing, Logging)
    ↑
API (Controllers, Middleware, Workers)
```

- Identity layer exists as placeholder for future auth -->

---

## File Structure

<!-- Ask Claude to add the file structure -->

<!-- ```
project-root/
├── TimefoldInt.API/              # ASP.NET Core Web API (controllers, middleware, workers)
├── TimefoldInt.Application/      # CQRS features, contracts, DTOs, specifications
├── TimefoldInt.Domain/           # Core entities (Tenant, Vehicle), value objects
├── TimefoldInt.Infrastructure/   # AWS services, job processing, logging
├── TimefoldInt.Persistence/      # EF Core DbContext, repositories, migrations
├── TimefoldInt.Identity/         # Auth placeholder (not yet implemented)
├── test/
│   ├── TimefoldInt.Api.IntegrationTests/
│   ├── TimefoldInt.Application.UnitTests/
│   └── TimefoldInt.Persistence.IntegrationTests/
├── external-memory/              # Claude's persistent context
│   ├── status.md
│   ├── phases/
│   └── completed/
├── TimefoldInt.sln
└── docker-compose.yml
``` -->

---

## External Memory

The `external-memory/` directory serves as persistent context across sessions.

### Files
- **status.md** — Index of active workstreams. Points to feature-specific `master-plan.md` files. Update only when adding/removing features.
- **phases/** — Active phase files for current work
- **completed/** — Archived phases (move here when done)

### Parallel Feature Work
Each feature/workstream maintains its own `master-plan.md` for detailed tracking. This allows multiple features to be developed in parallel without status conflicts.

### Session Start
Run `/catchup` — it reads status.md, loads master-plan context, shows progress, and recommends next action.

### Phase Completion
When a phase is fully complete:
1. Move the phase file from `phases/` to `completed/`
2. Update the feature's `master-plan.md` to reflect completion
3. If feature is complete, remove it from `status.md` index
4. Update CHANGELOG.md with phase accomplishments

---

## Context Management

### Session Hygiene
- **Start fresh sessions for new tasks** — long conversations degrade output quality
- Each task should be completable in one session when possible
- Use `/clear` or start new conversation when switching tasks

### Session Handoff
Run `/complete session` — it logs a session summary to master-plan and commits. For completed work, use `/complete task` or `/complete phase` instead.

### Task Files as Context
Use task files to carry context between sessions — not one massive conversation. Task files are self-contained with specific context, but always read master-plan.md and phase overview.md first for the bigger picture.

---

## Planning

1. Use plan mode for all new work
2. Collaborate with user until plan is approved - never proceed without approval
3. Use Explore agent to gather codebase context before finalizing

**Plan files are saved to:** `~/.claude/plans/`

### Small tasks
- Use plan mode's default plan file
- After approval, implement directly

### Larger projects
- Break into phase files in `external-memory/phases/`
- Naming: `phase-1-core-setup.md`, `phase-2-integration.md`
- Optional: Run `/review-plan` for staff engineer review before proceeding
- Run `/create-phases` to generate phase files from plan
- Run `/task-planning-workflow` to create task structure


### Post-Plan Approval (MANDATORY)
After a plan is approved (ExitPlanMode accepted), do NOT implement directly. Follow this pipeline:

**Multi-phase plan** → Run `/create-phases` immediately. Do not write any code until phases exist in `external-memory/phases/`.

Single-phase or trivial plans (no phases in plan) may skip `/create-phases` and implement directly.

**Why this matters:** Without phase files, `/task-planning-workflow` has nothing to process, and tasks won't have acceptance criteria, verification steps, or proper tracking.

### Task Scoping
- **Keep tasks small and focused** — Claude performs significantly better on "add break scheduling to the optimizer" than "build the backend"
- Break large work into phases, hand off sequentially using task files
- Each task should have clear inputs, expected outputs, and verification steps

---

## Task & Phase Management

Larger work is organized as: **Feature → Phases → Tasks**

| Artifact | Location | Purpose |
|----------|----------|---------|
| Plan file | `~/.claude/plans/` | Initial plan from plan mode |
| Master plan | `external-memory/tasks/[domain]/master-plan.md` | Project status, conventions, decisions |
| Phase files | `external-memory/phases/` | Define scope and objectives |
| Task structure | `external-memory/tasks/[domain]/phase-[NN]-[name]/` | Actionable implementation tasks |
| Completed phases | `external-memory/completed/` | Archived finished work |

### Workflow Skills

All skills are interactive - no flags or arguments needed:

| Step | Skill | What it does |
|------|-------|--------------|
| 1 | Plan mode | Creates plan in `~/.claude/plans/` |
| 2 | `/review-plan` | (Optional) Staff engineer review with iterations |
| 3 | `/create-phases` | Extracts phases from plan → `external-memory/phases/` |
| 4 | `/task-planning-workflow` | Creates task files from phase |
| 5 | `/complete` | Marks tasks/phases complete, or logs session handoff |
| 6 | `/catchup` | Start-of-session: loads context, shows progress, hydrates tasks |

---

## Implementation

1. Work through tasks created by `/task-planning-workflow`
2. Tasks include acceptance criteria — use these to define "done"
3. Run verification (tests, build) as you implement
4. Check off acceptance criteria as you complete them
5. Update the feature's `master-plan.md` as you progress
6. Mark tasks complete in phase file when all criteria are met

---

## Verification

Without verification, Claude is generating code. With it, Claude is delivering working code.

<!-- Add your own codebase specifics -->

<!-- | Change Type | Verification |
|---|---|
| Backend changes | `dotnet build TimefoldInt.sln` + `dotnet test TimefoldInt.sln` |
| Database schema | `dotnet ef database update --project TimefoldInt.Persistence --startup-project TimefoldInt.API` + verify API starts |
| Infrastructure/Docker | `docker-compose up -d` + `docker-compose logs -f` (check no errors) |
| Job processing | `dotnet test` + verify SQS listener processes jobs | -->

### Task Verification
Tasks created by `/task-planning-workflow` include acceptance criteria. Use these as your test checklist — a task is complete when all criteria pass.

### Testing Strategy (Trophy Model)

| Test Type | Target | Purpose |
|---|---|---|
| Integration | 60% | Verify components work together through real code paths |
| Unit | 20% | Test complex business logic, pure functions, algorithms |
| E2E | 20% | Validate critical user journeys only |

### Key Principles
- Integration tests give Claude the most signal per test — they catch real failures that mocks would miss
- Mock only at system boundaries (external APIs, third-party services)
- Avoid mocking internal code — mocked tests can pass while the real system is broken
- Acceptance criteria scripts are valid tests

### Test-Driven Development (TDD)

TDD is highly recommended, not mandatory.

**When to use TDD:**
- Business logic and algorithms
- Complex calculations or transformations
- Code with clear input/output requirements

**When to skip TDD:**
- UI layout and styling
- Prototyping and exploration
- Infrastructure configuration

**TDD Workflow:**
1. Write a failing test for the expected behavior
2. Run test - confirm it fails
3. Write minimal code to make test pass
4. Run test - confirm it passes
5. Refactor if needed, keeping tests green

---

## Review

The final quality gate before marking work complete.

### Self-Review Checklist
- [ ] All acceptance criteria met
- [ ] Verification steps pass (tests, build, type-check)
- [ ] No debug code or console.logs left behind
- [ ] Code follows project conventions
- [ ] Complex logic has comments explaining "why"
- [ ] No unaddressed TODOs
- [ ] CHANGELOG.md updated (for user-facing changes)

### Before Completing
- Update the feature's `master-plan.md` with completed work
- Note any decisions or trade-offs made
- Flag anything that needs human review
- Update CHANGELOG.md for user-facing changes

### When to Request Human Review
- Significant architectural changes
- Uncertain about approach taken
- Changes affect critical business logic

---

## Agent Orchestration

Use when working on multi-task implementations. For single focused tasks, work directly without orchestration.

### When to Orchestrate
- Multiple independent tasks that can run in parallel
- Large phase implementation with many tasks
- Work that benefits from separation (implement → review)
- Large context investigation (reviewing big PDFs, understanding complex subsystems like auth) — use Explore agent to summarize rather than loading everything into main context

### Workflow
1. Create plan with user (use Planning methodology above)
2. Use Explore agent to gather context if needed
3. For existing phases in `external-memory/phases/`, run `/task-planning-workflow` to create task structure

### Dispatching Agents
- **Sequential/dependent tasks**: One general-purpose agent (preserves shared context)
- **Independent tasks**: Up to 3-4 parallel general-purpose agents
- **Fixes needed**: Implementation agent to address issues
- **Review**: General-purpose agent to review and polish

### Error Handling
- If an agent fails or gets stuck, assess the blocker before retrying
- Update the feature's `master-plan.md` with any blockers encountered

### Completion Checklist
- [ ] All acceptance criteria checked off
- [ ] Verification steps pass
- [ ] Feature's `master-plan.md` updated with completed work

---

## Conventions

<!-- Add your own codebase conventions -->

<!-- ### Backend (.NET)
- **Naming**: PascalCase for classes/methods, camelCase for parameters
- **Async**: All I/O operations use async/await
- **Logging**: ILogger injected, use structured logging
- **Error Handling**: Custom exceptions in Domain, global exception middleware in API

### Database
- **Migrations**: Descriptive names (e.g., `AddVehicleStatusIndex`)
- **Naming**: snake_case for tables/columns (PostgreSQL convention)

### Validation
- FluentValidation is called manually in each handler via `ValidateAsync()`. This is intentional — no MediatR pipeline behavior is used.

### Delete Policy
- **Soft delete** (IsDeleted flag + `HasQueryFilter`): Used for compliance and core entities that must be retained for audit trails (e.g., AuditLog, Tenant, Vehicle, Employee, etc.)
- **Hard delete** (physical deletion, no query filter): Used for supporting/operational entities whose handlers perform physical deletion
- **Hard-delete entities**: Equipment, VehicleCredential, MaintenanceRecord, VehicleInspection, FuelLog, TripExecution, PassengerStop
- Do NOT add `HasQueryFilter` to hard-delete entity configurations -->

---

## Keeping This File Updated

Update CLAUDE.md when you encounter information valuable for future sessions:

| Trigger | Section to Update |
|---------|-------------------|
| Critical decision or feature | Project or relevant section |
| New business rule or constraint | Constraints & Business Rules |
| Common bug or mistake pattern | Add to relevant section with warning |
| New coding convention established | Conventions |
| Important file added/removed | File Structure |

**Keep it minimal** - only add what future sessions genuinely need.
