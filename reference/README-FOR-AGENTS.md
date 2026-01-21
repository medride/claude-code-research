# Agent Orchestration System

This directory contains specialized agents for the multi-phase orchestration workflow.

## Available Agents

### 4-Phase Pipeline (Default Workflow)

Use for most coding tasks: features, bug fixes, refactors, API endpoints.

```
planning-agent → implementation-agent → testing-agent → review-agent
```

| Agent | Phase | Role | Model |
|-------|-------|------|-------|
| `planning-agent` | 1 | Codebase reconnaissance, pattern discovery | Haiku |
| `implementation-agent` | 2 | Write code following discovered patterns | Sonnet |
| `testing-agent` | 3 | Run tests, fix errors, add coverage | Sonnet |
| `review-agent` | 4 | Security, performance, production readiness | Sonnet |

### Orchestrator Pattern (Parallel Execution)

Use for large features spanning multiple independent domains.

| Agent | Role | Model |
|-------|------|-------|
| `orchestrator` | Coordinates sub-agents, doesn't write code | Opus |

### 3 Amigos Pattern (Requirements Clarification)

Use when requirements are unclear or for user-facing UX-heavy features.

```
pm-agent → ux-agent → implementation-agent
```

| Agent | Phase | Role | Model |
|-------|-------|------|-------|
| `pm-agent` | 1 | Requirements, acceptance criteria | Sonnet |
| `ux-agent` | 2 | Component design, user flows | Sonnet |
| `implementation-agent` | 3 | Build based on PM + UX specs | Sonnet |

## Usage

### Automatic Delegation

Claude will automatically delegate to these agents based on their descriptions. Just describe your task and Claude will choose the appropriate agent.

### Manual Invocation

You can explicitly request an agent:

```
"Use the planning-agent to explore the authentication code"
"Run the testing-agent on the changes I just made"
"Use the orchestrator to coordinate building the new dashboard feature"
```

### Workflow Examples

**Standard Feature (4-Phase Pipeline):**
```
You: Add a new endpoint for user preferences

Claude: [Uses planning-agent to explore codebase]
Claude: [Uses implementation-agent to write code]
Claude: [Uses testing-agent to verify]
Claude: [Uses review-agent to polish]
```

**Large Multi-Domain Feature (Orchestrator):**
```
You: Use the orchestrator to build user authentication with frontend, backend, and database

Claude: [Uses orchestrator to create plan and spawn parallel agents]
```

**Unclear Requirements (3 Amigos):**
```
You: I need some kind of notification system, not sure exactly what

Claude: [Uses pm-agent to define requirements]
Claude: [Uses ux-agent to design the experience]
Claude: [Uses implementation-agent to build]
```

## Pattern Selection Guide

| Task Characteristics | Use This |
|---------------------|----------|
| Simple (< 3 files, obvious) | Direct execution (no agents) |
| Standard feature/bug fix | 4-Phase Pipeline |
| Multiple independent domains | Orchestrator |
| Unclear requirements | 3 Amigos (PM → UX → Dev) |
| Unfamiliar codebase | planning-agent first |

## Agent Files

```
.claude/agents/
├── README.md              # This file
├── planning-agent.md      # Phase 1: Reconnaissance
├── implementation-agent.md # Phase 2: Build
├── testing-agent.md       # Phase 3: Verify
├── review-agent.md        # Phase 4: Polish
├── orchestrator.md        # Parallel coordination
├── pm-agent.md            # Requirements (3 Amigos)
└── ux-agent.md            # Design (3 Amigos)
```

## Customization

Each agent is a markdown file with YAML frontmatter. You can customize:

- `tools`: Which tools the agent can use
- `model`: Which Claude model to use (haiku, sonnet, opus)
- `description`: When Claude should delegate to this agent
- System prompt: The agent's instructions and behavior

See [Claude Code Subagents Documentation](https://code.claude.com/docs/en/sub-agents) for more details.
