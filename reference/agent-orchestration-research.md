# Agent Orchestration Research Findings

> Research conducted: 2026-01-20
> Purpose: Compare orchestration approaches and recommend improvements

---

## Executive Summary

After analyzing your current approaches and researching industry best practices, I found that **both approaches have merit and can be combined** into a more powerful hybrid system. Your manager's AGENT-ORCHESTRATION.md provides excellent sequential phase discipline, while the workflow-guide.md Section 5.6 offers flexibility and parallelization.

**Recommendation**: Combine both approaches into a **Pattern-Aware Orchestration System** that selects the right pattern based on task characteristics.

---

## Current Approaches Compared

### Approach A: AGENT-ORCHESTRATION.md (Manager's 4-Phase Pipeline)

```
Phase 1 (Planning) → Phase 2 (Implementation) → Phase 3 (Testing) → Phase 4 (Review)
```

**Strengths:**
- Clear sequential progression with explicit handoffs
- Project-specific conventions baked in (.NET, React patterns)
- Well-defined outputs for each phase
- Simple mental model for the team

**Weaknesses:**
- Always sequential (no parallelization option)
- Single pattern for all task types
- May be overkill for simple tasks
- No routing logic for different situations

**Best for:** Medium-complexity features, algorithm work, API endpoints with business logic

---

### Approach B: Workflow-guide.md Section 5.6 (Multiple Patterns)

**Pattern Menu:**
| Pattern | Use Case |
|---------|----------|
| Orchestrator Pattern | Large features, multiple domains |
| 3 Amigos (PM→UX→Dev) | Unclear requirements, user-facing |
| Domain-Based Splitting | Full-stack features |
| Research-Plan-Execute | Unfamiliar territory |
| Quality Gate Pattern | Pre-PR verification |

**Strengths:**
- Flexibility to match pattern to task
- Supports parallel execution
- Comprehensive anti-patterns documented
- Clear decision framework

**Weaknesses:**
- Requires judgment to select pattern
- More complex to learn
- No project-specific conventions built in
- Can lead to over-engineering

**Best for:** Complex multi-domain features, research-heavy tasks, parallelizable work

---

## Industry Best Practices (2026 Research)

### 1. Claude-Flow Framework
- **Hierarchical (queen/workers)** or **mesh (peer-to-peer)** patterns
- Self-learning: Routes work to specialized experts based on past success
- Agents can spawn sub-workers and share context

### 2. Programmatic Tool Calling (Anthropic Official)
- Express orchestration logic in **code (Python)** rather than natural language
- More reliable control flow with explicit loops, conditionals, error handling
- Reduces round-trip overhead

### 3. CC Mirror Patterns
- **Fan-Out**: Distribute to many workers, aggregate results
- **Pipeline**: Sequential transformation chain
- **Map-Reduce**: Parallel processing with aggregation
- "Conductor" identity: Absorb complexity, radiate simplicity

### 4. Critical Constraint: No Nested Subagents
From official Claude Code docs: **Subagents cannot spawn other subagents**. If nested delegation is needed, use Skills or chain from the main conversation.

### 5. Practical Tips (from Boris Cherny, Claude Code creator)
- Treat CLAUDE.md as a living knowledge base, refined through failures
- Use Opus 4.5 with thinking enabled for everything
- Run 5+ parallel Claude sessions in terminal tabs
- Use system notifications to know when Claude needs input

---

## Recommended: Pattern-Aware Orchestration System

Combine both approaches into a unified system with **automatic pattern selection**.

### Core Principles

1. **Task Analysis First**: Evaluate task characteristics before selecting pattern
2. **Default to Simple**: Use 4-phase pipeline unless criteria trigger a different pattern
3. **Explicit Parallelization**: Only parallelize when all conditions are met
4. **Project Conventions Always Apply**: .NET and React patterns regardless of orchestration choice

### Decision Tree

```
                    ┌─────────────────────────┐
                    │     New Task Arrives    │
                    └───────────┬─────────────┘
                                │
                    ┌───────────▼─────────────┐
                    │  Is it a simple task?   │
                    │  (< 3 files, obvious)   │
                    └───────────┬─────────────┘
                           YES │ NO
                    ┌──────────┘ └──────────┐
                    ▼                        ▼
            ┌───────────┐         ┌─────────────────────┐
            │  Direct   │         │ Are requirements    │
            │  (no sub- │         │ clear?              │
            │  agents)  │         └─────────┬───────────┘
            └───────────┘              YES │ NO
                              ┌────────────┘ └────────────┐
                              ▼                           ▼
                    ┌─────────────────────┐     ┌─────────────────────┐
                    │ Is it parallelizable│     │ 3 Amigos Pattern    │
                    │ (independent domains│     │ (PM → UX → Dev)     │
                    │ no file overlap)?   │     │ OR                  │
                    └─────────┬───────────┘     │ Research-Plan-Exec  │
                         YES │ NO               └─────────────────────┘
              ┌──────────────┘ └──────────────┐
              ▼                               ▼
    ┌─────────────────────┐        ┌─────────────────────┐
    │ Orchestrator Pattern │        │ 4-Phase Pipeline    │
    │ (Parallel Agents)    │        │ (Sequential)        │
    └─────────────────────┘        └─────────────────────┘
```

### Unified Orchestration File

Replace or merge into a single AGENT-ORCHESTRATION.md:

```markdown
# Agent Orchestration System

## Pattern Selection (Auto-Select)

Before starting any non-trivial task, evaluate:

| Criteria | Check |
|----------|-------|
| Simple task (< 3 files, obvious solution)? | → Direct execution, skip orchestration |
| Requirements unclear or user-facing UX? | → Use 3 Amigos or Research-Plan-Execute |
| Multiple independent domains? | → Use Orchestrator Pattern (parallel) |
| Dependencies between components? | → Use 4-Phase Pipeline (sequential) |

## Default: 4-Phase Pipeline

**Use for most coding tasks:**

### Phase 1: Planning Agent (Explore)
- Codebase reconnaissance
- Identify patterns, conventions, integration points
- Output: Map of changes, patterns to follow, risks

### Phase 2: Implementation Agent
- Follow Phase 1 discoveries
- Project conventions (see below)
- Output: Functional code in correct locations

### Phase 3: Testing Agent
- Run build/tests, fix errors
- Write new tests for critical paths
- Output: All checks passing

### Phase 4: Review & Polish Agent
- Error handling, logging, security
- Remove dead code, cleanup
- Output: Production-ready code

## Alternative: Orchestrator Pattern (Parallel)

**Use when ALL conditions are met:**
- 3+ independent domains (e.g., frontend, backend, database)
- No file overlap between domains
- Clear boundaries

**Main Thread Responsibilities:**
- Do NOT write code directly
- Break into sub-agent assignments
- Each sub-agent gets: objective, file boundaries, deliverables
- Run independent agents in parallel
- Synthesize results, flag conflicts

**Sub-Agent Template:**
```
Agent: [Name]
Objective: [What to accomplish]
Files OWNED: [Can modify]
Files FORBIDDEN: [Cannot touch]
Dependencies: [Which agents must complete first]
Deliverables: [Expected outputs]
```

## Alternative: 3 Amigos Pattern

**Use when requirements are unclear or UX-heavy:**

1. **PM Agent**: Define requirements, acceptance criteria, constraints
2. **UX Agent**: Design components, user flows, interaction patterns
3. **Implementation Agent**: Code based on PM + UX specs

Run sequentially. Approve each phase before proceeding.

## Project Conventions (Always Apply)

### Backend (.NET)
- PascalCase classes/methods, camelCase parameters/variables
- Async/await for all I/O
- Clean Architecture: Domain → Application → Infrastructure → API
- Constructor injection, register in Program.cs
- ILogger<T> with structured logging
- FluentValidation for DTOs

### Frontend (React)
- PascalCase components, camelCase functions
- @/ prefix for absolute imports
- Zustand for global state, useState for local
- kebab-case file names
- Named exports, functional components

## Anti-Patterns to Avoid

❌ Over-parallelizing (10 agents for simple feature)
❌ Under-specifying sub-agents (vague objectives)
❌ No file boundaries (causes merge conflicts)
❌ Forgetting to synthesize results
❌ Skipping quality gates
❌ Nested sub-agents (not supported—chain from main thread)

## Quality Gates (Run Before PR)

Regardless of pattern used, run these gates:
1. **Lint Gate**: Run linter, fix issues
2. **Test Gate**: All tests pass, coverage adequate
3. **Security Gate**: No secrets, injection vulnerabilities
4. **Simplify Gate**: Remove unnecessary complexity
5. **Docs Gate**: Update README/comments if needed
```

---

## Implementation Recommendations

### Option 1: Minimal Change (Quick Win)

Update CLAUDE.md to include pattern selection guidance. Keep AGENT-ORCHESTRATION.md for detailed phase instructions.

**Add to CLAUDE.md:**
```markdown
## Orchestration Pattern Selection

Before starting non-trivial tasks:
- Simple (< 3 files): Direct execution
- Unclear requirements: 3 Amigos (PM → UX → Dev)
- Multiple independent domains: Orchestrator Pattern (parallel)
- Default: 4-Phase Pipeline from AGENT-ORCHESTRATION.md
```

### Option 2: Unified System (Recommended)

Replace AGENT-ORCHESTRATION.md with the unified file above. This gives you:
- Automatic pattern selection
- Parallelization when beneficial
- Sequential pipeline as the default
- Project conventions always applied
- Quality gates built in

### Option 3: Advanced (Worktree-Based True Parallelization)

Use the Worktree Manager skill for truly parallel Claude instances:
- Each worktree gets its own Claude Code session
- Real concurrent execution (not just parallel tool calls)
- Best for large features with clear domain splits

---

## Comparison Matrix

| Feature | Manager's Approach | Workflow Guide 5.6 | Recommended Hybrid |
|---------|-------------------|-------------------|-------------------|
| Pattern selection | Single pattern | Manual selection | Auto-select |
| Parallelization | No | Yes | Yes (when criteria met) |
| Sequential pipeline | Yes (always) | Optional | Default |
| Project conventions | Built-in | Not included | Built-in |
| Anti-patterns | Not documented | Documented | Documented |
| Quality gates | Phase 4 covers | Separate pattern | Integrated |
| Complexity | Low | Medium | Medium |
| Flexibility | Low | High | High |

---

## Next Steps

1. **Review this document** with your manager
2. **Decide on implementation option** (1, 2, or 3)
3. **Test the pattern selection** on your next feature
4. **Iterate based on results**—refine the decision tree

---

## Sources

- Internal: AGENT-ORCHESTRATION.md, workflow-guide.md (Section 5.6)
- [Claude Code Official Docs - Subagents](https://code.claude.com/docs/en/sub-agents)
- [Claude-Flow Framework](https://github.com/ruvnet/claude-flow)
- [Multi-Agent Orchestration Patterns](https://sjramblings.io/multi-agent-orchestration-claude-code-when-ai-teams-beat-solo-acts/)
- [Anthropic - Advanced Tool Use](https://www.anthropic.com/engineering/advanced-tool-use)
- [wshobson/agents Framework](https://github.com/wshobson/agents)
