# Agent Orchestration System v2

> A unified pattern-aware orchestration system combining sequential pipelines with parallel execution.

---

## Quick Reference: Pattern Selection

| Task Characteristics | Pattern | Execution |
|---------------------|---------|-----------|
| Simple (< 3 files, obvious solution) | Direct | No sub-agents |
| Requirements unclear, UX-heavy | 3 Amigos | Sequential |
| Unfamiliar codebase/API | Research-Plan-Execute | Sequential |
| Multiple independent domains | Orchestrator | Parallel |
| Standard feature/bug fix | 4-Phase Pipeline | Sequential |

---

## Pattern 1: 4-Phase Pipeline (Default)

**Use for:** Most coding tasks—features, bug fixes, refactors, API endpoints.

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│  Phase 1        │     │  Phase 2        │     │  Phase 3        │     │  Phase 4        │
│  PLANNING       │ ──► │  IMPLEMENTATION │ ──► │  TESTING        │ ──► │  REVIEW         │
│  (Explore)      │     │  (Build)        │     │  (Verify)       │     │  (Polish)       │
└─────────────────┘     └─────────────────┘     └─────────────────┘     └─────────────────┘
```

### Phase 1: Planning Agent

**Role:** Codebase reconnaissance and validation

**Subagent type:** `Explore`

**Tasks:**
- Locate existing implementations and patterns relevant to the task
- Identify affected files, components, and dependencies
- Validate architectural patterns and conventions in use
- Document current state and potential integration points
- Flag any potential conflicts or breaking changes

**Output:**
- Clear map of where changes need to be made
- List of existing patterns/conventions to follow
- Identified dependencies and imports needed
- Risk assessment of proposed changes

---

### Phase 2: Implementation Agent

**Role:** Core development work

**Subagent type:** `General-purpose`

**Tasks:**
- Follow patterns identified in Phase 1
- Implement changes incrementally
- Use existing components/utilities before creating new ones
- Write type-safe code with proper interfaces
- Integrate with existing patterns

**Backend (.NET) Conventions:**
- PascalCase for classes/methods, camelCase for parameters/variables
- Async/await for all I/O operations
- Clean Architecture: Domain → Application → Infrastructure → API
- Constructor injection, register in Program.cs
- ILogger<T> with structured logging
- FluentValidation for DTOs

**Frontend (React) Conventions:**
- PascalCase for components, camelCase for functions/variables
- `@/` prefix for absolute imports
- Zustand for global state, useState for local UI state
- kebab-case file names, named exports
- Functional components with TypeScript

**Output:**
- Functional implementation following codebase conventions
- Type-safe code with proper imports/dependencies
- Code ready for testing phase

---

### Phase 3: Testing Agent

**Role:** Quality assurance and validation

**Subagent type:** `General-purpose`

**Tasks:**
- Run build and type checks
- Fix all compile/type/lint errors
- Ensure all existing tests pass
- Write new tests for complex logic and critical paths
- Test edge cases and error scenarios

**Commands:**

```bash
# Backend (.NET)
dotnet build
dotnet test --verbosity detailed

# Frontend (React)
npm run type-check
npm run lint
npm run test
npm run build
```

**Output:**
- All type checks passing
- All linting rules satisfied
- Test suite passing with new tests added
- List of any known limitations

---

### Phase 4: Review & Polish Agent

**Role:** Code quality and production readiness

**Subagent type:** `General-purpose`

**Tasks:**
- Review error handling patterns
- Add appropriate logging for debugging/monitoring
- Validate security (no secrets, proper input validation)
- Check for edge cases and null handling
- Review performance (N+1 queries, inefficient algorithms)
- Remove dead code, debug logs, commented-out code
- Final consistency check

**Output:**
- Production-ready, polished code
- Proper error handling throughout
- Ready for commit/PR

---

## Pattern 2: Orchestrator (Parallel Execution)

**Use for:** Large features spanning multiple independent domains.

**Trigger criteria (ALL must be met):**
- 3+ independent tasks or domains
- No shared files between domains
- Clear boundaries with no overlap

```
┌─────────────────────────────────────────────────────────┐
│              MAIN THREAD (Orchestrator)                 │
│  - Does NOT write code directly                         │
│  - Creates sub-agent assignments                        │
│  - Monitors progress                                    │
│  - Synthesizes results                                  │
│  - Flags conflicts                                      │
└─────────────────────────────────────────────────────────┘
         │              │              │              │
         ▼              ▼              ▼              ▼
   ┌──────────┐   ┌──────────┐   ┌──────────┐   ┌──────────┐
   │ Frontend │   │ Backend  │   │ Database │   │  Tests   │
   │  Agent   │   │  Agent   │   │  Agent   │   │  Agent   │
   └──────────┘   └──────────┘   └──────────┘   └──────────┘
         │              │              │              │
         └──────────────┴──────┬───────┴──────────────┘
                               ▼
                    ┌────────────────────┐
                    │  Results Synthesis │
                    │  + Quality Gates   │
                    └────────────────────┘
```

### Sub-Agent Assignment Template

```markdown
## Sub-Agent: [Name]

**Objective:** [What to accomplish]

**Files OWNED:** [Can create/modify]
- path/to/files/*
- path/to/other/files/*

**Files FORBIDDEN:** [Cannot touch]
- other/path/*
- shared/files/*

**Dependencies:** [Which agents must complete first]
- None (can run in parallel)
- OR: Requires [Agent X] output

**Deliverables:**
- [ ] Specific output 1
- [ ] Specific output 2
- [ ] All tests passing

**Conventions:** Follow project conventions from Phase 2
```

### Parallel vs Sequential Decision

| Run in PARALLEL when | Run SEQUENTIALLY when |
|---------------------|----------------------|
| Tasks are independent | Tasks have dependencies |
| No shared files | Shared files or state |
| Clear domain boundaries | Unclear scope |
| Example: Frontend + Backend + DB | Example: Schema → API → Frontend |

---

## Pattern 3: 3 Amigos (Requirements Clarification)

**Use for:** New features with unclear requirements or UX-heavy work.

```
┌─────────────────────────────────────────────────────────┐
│  1. PM Agent (Requirements)                             │
│     - Analyze the request                               │
│     - Define acceptance criteria                        │
│     - Identify technical constraints                    │
│     Output: Mini PRD                                    │
└─────────────────────────────────────────────────────────┘
                          ↓ (Approval checkpoint)
┌─────────────────────────────────────────────────────────┐
│  2. UX Agent (Design)                                   │
│     - Design component structure                        │
│     - Define user flows                                 │
│     - Specify interaction patterns                      │
│     Output: Component specs                             │
└─────────────────────────────────────────────────────────┘
                          ↓ (Approval checkpoint)
┌─────────────────────────────────────────────────────────┐
│  3. Implementation Agent (Code)                         │
│     - Implement based on PM + UX specs                  │
│     - Write tests                                       │
│     - Follow project conventions                        │
│     Output: Working code + tests                        │
└─────────────────────────────────────────────────────────┘
```

**Key feature:** Approval checkpoint after each phase before proceeding.

---

## Pattern 4: Research-Plan-Execute

**Use for:** Unfamiliar territory—new APIs, new parts of codebase, complex technical decisions.

```
┌─────────────────────────────────────────────────────────┐
│  RESEARCH PHASE (Parallel exploration)                  │
│  - Agent 1: Explore existing codebase                   │
│  - Agent 2: Research external docs/APIs                 │
│  - Agent 3: Find similar implementations                │
│  Output: Synthesis report                               │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│  PLAN PHASE (Main thread)                               │
│  - Review research findings                             │
│  - Create detailed implementation plan                  │
│  - Identify risks and unknowns                          │
│  Output: Approved plan                                  │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│  EXECUTE PHASE (Sub-agents per domain)                  │
│  - Follow the approved plan                             │
│  - Each agent owns their domain                         │
│  Output: Working implementation                         │
└─────────────────────────────────────────────────────────┘
```

---

## Quality Gates (Always Run Before PR)

Regardless of which pattern is used, run these gates:

| Gate | Check | Command |
|------|-------|---------|
| Build | Compiles without errors | `dotnet build` / `npm run build` |
| Lint | Code style compliance | `npm run lint` / IDE warnings |
| Test | All tests pass | `dotnet test` / `npm run test` |
| Security | No secrets, no injection vulnerabilities | Manual review |
| Simplify | No unnecessary complexity | Manual review |

---

## Anti-Patterns to Avoid

| Anti-Pattern | Problem | Solution |
|--------------|---------|----------|
| Over-parallelizing | 10 agents for simple feature wastes tokens | Use direct execution for simple tasks |
| Under-specifying | "Handle the frontend" is too vague | Specify files owned, deliverables, constraints |
| No file boundaries | Merge conflicts when agents edit same files | Explicit OWNED and FORBIDDEN file lists |
| Forgetting synthesis | Sub-agents complete but no integration review | Main thread must synthesize and verify |
| Skipping quality gates | Bugs ship to production | Always run gates before PR |
| Nested sub-agents | Not supported by Claude Code | Chain from main thread instead |

---

## Usage Notes

### When to Skip Orchestration

- Simple bug fixes (single line changes)
- Documentation updates
- Configuration tweaks
- Trivial refactors

### Adapting Phases

**Collapse phases** for smaller tasks:
- Simple features: Combine Phase 2 + 3 (implement and test together)
- Very small changes: Skip Phase 1 if integration points are obvious

**Split phases** for complex tasks:
- Large features: Split Phase 2 into sub-phases (2A, 2B, 2C)
- Algorithm work: Run tests incrementally after each sub-phase

### Key Principles

1. **Incremental Progress**: Each phase builds on the previous
2. **Test Early**: Don't wait until Phase 3—run tests after significant changes
3. **Follow Patterns**: Phase 1 discoveries guide Phase 2 implementation
4. **Quality First**: Phase 4 ensures production readiness, not just "working code"
5. **Document Decisions**: Each phase leaves clear handoff notes
