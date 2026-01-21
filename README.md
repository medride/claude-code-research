# Claude Code Workflow

> A template repository and reference hub for Claude Code best practices.

This repository serves two purposes:
1. **Template**: Clone it to bootstrap new projects with Claude Code configuration
2. **Reference**: Living documentation for Claude Code workflows, skills, and patterns

---

### Using as a Reference

Browse the `reference/` folder for comprehensive guides:
- [`reference/workflow-guide.md`](reference/workflow-guide.md) - Complete team guide
- [`reference/skills-guide.md`](reference/skills-guide.md) - Skills reference
- [`reference/quick-reference.md`](reference/quick-reference.md) - Cheatsheet

---

## Repository Structure

```
claude-code-research/
├── README.md                           # You are here
├── CLAUDE.md                           # Example project instructions
├── .claudeignore                       # Files for Claude to ignore
│
├── .claude/
│   ├── settings.json                   # Permissions configuration
│   ├── settings.local.json.example     # Local settings example
│   ├── hooks.json                      # Hooks configuration
│   │
│   ├── commands/                       # 12 custom slash commands
│   │   ├── catch-up.md
│   │   ├── checkpoint.md
│   │   ├── debug.md
│   │   ├── handoff.md
│   │   ├── orchestrate.md
│   │   ├── plan-feature.md
│   │   ├── quality-gates.md
│   │   ├── review.md
│   │   ├── simplify.md
│   │   ├── status.md
│   │   ├── verify.md
│   │   └── worktree.md
│   │
│   └── skills/                         # 9 reusable skills
│       ├── api-design/                 # API design patterns
│       │   ├── SKILL.md
│       │   ├── examples/
│       │   └── templates/
│       ├── dotnet-api/                 # .NET API development
│       │   ├── SKILL.md
│       │   ├── templates/
│       │   └── examples/
│       ├── dotnet-api-auth/            # .NET API auth patterns
│       │   ├── SKILL.md
│       │   └── examples/
│       ├── code-review/                # Code review standards
│       ├── deployment-checklist/       # Pre-deploy verification
│       ├── documentation/              # Doc writing standards
│       ├── git-commit/                 # Commit message generation
│       ├── project-memory/             # Context persistence
│       └── worktree-manager/           # Git worktree management
│
├── docs/                               # External memory
│   ├── README.md
│   ├── status.md                       # Current work status
│   ├── tech-debt.md                    # Technical debt tracking
│   └── decisions/                      # Architecture Decision Records
│       ├── README.md
│       └── TEMPLATE.md
│
├── reference/                          # Comprehensive guides
│   ├── README.md
│   ├── workflow-guide.md               # Full Claude Code guide
│   ├── skills-guide.md                 # Skills reference
│   └── quick-reference.md              # Cheatsheet
│
└── templates/                          # For new projects
    ├── README.md
    ├── CLAUDE.md.template
    ├── claudeignore.template
    ├── commands/                       # All 12 command templates
    └── skills/                         # Skill templates
        ├── README.md
        ├── basic-skill/
        └── advanced-skill/
```

---

## What's Included

### Commands (12 total)

| Command | Purpose |
|---------|---------|
| `/catch-up` | Resume context from previous sessions |
| `/review` | Code review staged changes |
| `/simplify` | Clean up recently written code |
| `/plan-feature` | Start planning a new feature |
| `/debug` | Structured debugging workflow |
| `/quality-gates` | Run quality checks before PR |
| `/orchestrate` | Large task coordination pattern |
| `/verify` | Post-implementation verification |
| `/status` | Quick project status check |
| `/handoff` | Prepare handoff notes |
| `/checkpoint` | Create checkpoint before risky changes |
| `/worktree` | Manage git worktrees |

### Skills (9 included)

| Skill | Purpose |
|-------|---------|
| `worktree-manager` | Create and manage git worktrees |
| `api-design` | Design APIs following best practices |
| `dotnet-api` | .NET Web API development patterns |
| `dotnet-api-auth` | .NET API authentication/authorization |
| `code-review` | Team code review standards |
| `git-commit` | Generate commit messages |
| `project-memory` | Persist context across sessions |
| `deployment-checklist` | Pre-deploy verification |
| `documentation` | Documentation standards |

---

## Core Principles

### Context Window Conservation

The #1 priority when working with Claude Code. This repository emphasizes:

- **Commands > Skills > MCPs** - Use the most context-efficient tool
- **Concise CLAUDE.md** - Keep under 2,500 tokens
- **Progressive disclosure** - Load details only when needed
- **External memory** - Use `docs/` for persistent context

### Context Efficiency Hierarchy

1. **CLAUDE.md** - Static project knowledge (always loaded)
2. **Skills** - Procedural knowledge (loads when relevant)
3. **Commands** - User-invoked workflows (loads when called)
4. **MCPs** - External data access (adds tools to every request)

