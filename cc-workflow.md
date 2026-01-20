# Claude Code: Complete Team Guide

> A comprehensive guide to using Claude Code effectively as a team.

---

## Table of Contents

1. [Getting Started](#1-getting-started)
2. [Core Concepts](#2-core-concepts)
3. [Project Setup](#3-project-setup)
4. [Prompting Effectively](#4-prompting-effectively)
5. [Workflow Patterns](#5-workflow-patterns)
6. [Context Management](#6-context-management)
7. [Debugging & Recovery](#7-debugging--recovery)
8. [Optimization](#8-optimization)
9. [Security](#9-security)
10. [Common Pitfalls & Solutions](#10-common-pitfalls--solutions)
11. [Team Practices](#11-team-practices)
12. [Quick Reference](#12-quick-reference)
13. [Glossary](#13-glossary)

---

## 1. Getting Started

### 1.1 Installation

**macOS/Linux:**
```bash
npm install -g @anthropic-ai/claude-code
```

**Verify installation:**
```bash
claude --version
```

### 1.2 Authentication

```bash
claude auth login
```

This opens a browser window to authenticate with your Anthropic account. Your API key is stored securely in your system keychain.

**For team/enterprise setups:**
```bash
claude auth login --org your-org-id
```

### 1.3 First Run

Navigate to your project and start Claude:

```bash
cd your-project
claude
```

Claude will automatically:
1. Detect your project structure
2. Read any existing `CLAUDE.md` file
3. Understand your tech stack from config files

### 1.4 IDE Integration

**VS Code:**
- Install the Claude Code extension
- Use `Cmd+Shift+P` → "Claude: Start Session"

**Terminal (recommended for full control):**
- Run `claude` in any terminal within your project
- Works with iTerm2, Warp, standard Terminal, etc.

### 1.5 Initial Configuration

After first run, configure your preferences:

```bash
claude config set model opus-4.5
claude config set thinking enabled
```

Or edit `~/.claude/config.json`:
```json
{
  "model": "opus-4.5",
  "thinking": true,
  "compactMode": false,
  "autoAccept": false
}
```

---

## 2. Core Concepts

### 2.1 The Context Window

**What it is:** The context window is Claude's "working memory"—the total amount of text it can see and process at once. Think of it like a desk: you can only have so many papers out before things fall off.

**Why it matters:**
- Once the context fills up, older information is pushed out
- Claude literally forgets earlier parts of your conversation
- This is the #1 thing that causes Claude to "lose track" of what you're doing

**Current limits:**
- Opus 4.5: ~200K tokens (~150K words)
- Sounds like a lot, but code is token-heavy

**Practical example:**
```
You: "Remember that auth bug we fixed earlier?"
Claude: "I don't see any previous discussion about an auth bug."

This happens because that conversation was pushed out of context.
```

### 2.2 Tokens

**What they are:** Tokens are chunks of text—roughly 4 characters or ¾ of a word on average.

**Token examples:**
| Text | Tokens |
|------|--------|
| "Hello" | 1 |
| "authentication" | 1 |
| "getUserById" | 3 |
| 100 lines of code | ~300-500 |
| A typical React component | ~200-400 |

**Why tokens matter:**
- Tokens = cost (you pay per token)
- Tokens = context space (limited budget)
- Being token-efficient means saving money and keeping context clean

### 2.3 Modes of Operation

| Mode | What Happens | When to Use |
|------|--------------|-------------|
| **Interactive** | Claude asks permission for each action | Learning, sensitive operations |
| **Plan Mode** | Claude creates a plan without executing | Starting new features, big changes |
| **Auto Accept** | Claude executes allowed commands automatically | Trusted operations, finalized plans |
| **Bypass** | Skips ALL permission checks | ❌ **Never use this** |

**Switching modes:**
```
You: /plan
Claude: [Now in plan mode - I'll describe what I would do without executing]

You: /auto
Claude: [Now in auto-accept mode - I'll execute allowed commands automatically]
```

### 2.4 Extended Thinking

**What it is:** Extended thinking lets Claude "think out loud" before responding, working through complex problems step by step.

**Always use Opus 4.5 with thinking enabled.** It significantly improves:
- Code architecture decisions
- Bug diagnosis
- Complex refactoring
- Understanding ambiguous requirements

**Enable it:**
```bash
claude config set thinking enabled
```

### 2.5 Sub-Agents

**What they are:** Separate Claude instances spawned for specific tasks, each with their own fresh context window.

**Why they're powerful:**
```
Main Thread (Orchestrator)
├── Sub-Agent 1: Code simplification (own context)
├── Sub-Agent 2: Test verification (own context)  
└── Sub-Agent 3: Documentation (own context)
```

Each sub-agent:
- Starts with a clean context (no pollution from main thread)
- Can work in parallel
- Reports back to the main thread

### 2.6 MCPs vs Skills

| Concept | Definition | When to Use |
|---------|------------|-------------|
| **MCP** (Model Context Protocol) | Connections to external systems (APIs, databases, services) | When you need live external data |
| **Skills** | Reusable prompts/instructions that teach Claude patterns | Everything else |

**Example - MCP:**
```
Connecting to your PostgreSQL database to query live data
Fetching from a REST API
Reading from a third-party service
```

**Example - Skill:**
```markdown
# .claude/skills/react-component.md
When creating React components:
1. Use functional components with hooks
2. Props interface defined above component
3. Export as named export
4. Colocate styles in .module.css file
```

**Rule: Use MCPs only when external data is needed. Use skills for everything else.**

### 2.7 Checkpoints

**What they are:** Snapshots of your project state that Claude can create before making changes.

**Creating checkpoints:**
```
You: Before you refactor this, create a checkpoint
Claude: [Creates checkpoint: pre-refactor-2026-01-19]
```

**Restoring checkpoints:**
```
You: That didn't work, restore the checkpoint
Claude: [Restores to: pre-refactor-2026-01-19]
```

**When to use them:**
- Before large refactors
- Before risky operations
- When trying experimental approaches

---

## 3. Project Setup

### 3.1 The CLAUDE.md File

The `CLAUDE.md` file is your project's instruction manual for Claude. It lives at the root of your project.

**Key principles:**
- Keep it **under 2,500 tokens**
- Be concise and specific
- Update it as your project evolves

#### Recommended Template:

````markdown
# Project: [Your Project Name]

## Tech Stack
- Frontend: React 18, TypeScript, Tailwind CSS
- Backend: Node.js, Express, PostgreSQL
- Testing: Jest, React Testing Library
- Infrastructure: Docker, AWS ECS

## Project Structure
```
/src
  /components    # React components
  /hooks         # Custom hooks
  /api           # API client and routes
  /utils         # Shared utilities
  /types         # TypeScript types
/server
  /routes        # Express routes
  /models        # Database models
  /middleware    # Express middleware
```

## Code Style & Conventions
- Named exports for components
- Absolute imports using @/ prefix
- Props interfaces named [Component]Props
- Async/await over .then() chains
- Error boundaries around major sections

## Do NOT
- Modify /legacy folder without explicit approval
- Use `any` type - always define proper types
- Skip writing tests for new features
- Commit directly to main branch
- Store secrets in code - use environment variables

## Common Commands
- `npm run dev` - Start development server
- `npm run test` - Run test suite
- `npm run test:watch` - Run tests in watch mode
- `npm run lint:fix` - Auto-fix linting issues
- `npm run build` - Production build

## State Management
- Zustand for global state (auth, theme, user preferences)
- React Query for server state
- useState for component-local state
- URL state for filters/pagination

## Error Handling
- Wrap async operations in try/catch
- Use ErrorBoundary for React component errors
- Log errors to console in dev, to /logs in prod
- User-facing errors should be friendly, not technical

## Testing Requirements
- Unit tests for utilities and hooks
- Integration tests for API endpoints
- Component tests for user interactions
- Minimum 80% coverage for new code

## Git Workflow
- Branch naming: feature/ticket-description
- Commits: conventional commits (feat:, fix:, refactor:)
- PRs require at least one approval
- Squash merge to main

## External Memory
- Document decisions in /docs/decisions/YYYY-MM-DD-topic.md
- Track technical debt in /docs/tech-debt.md
- Update /docs/status.md when switching context
````

#### Multi-Service Projects

For monorepos, create a CLAUDE.md in each service:

```
/project-root
  CLAUDE.md              # Global context (shared conventions)
  /frontend
    CLAUDE.md            # Frontend-specific context
  /backend
    CLAUDE.md            # Backend-specific context
  /infrastructure
    CLAUDE.md            # DevOps/infra context
```

### 3.2 Permissions Configuration

Configure allowed and denied commands in your project's `.claude/settings.json`:

```json
{
  "permissions": {
    "allow": [
      "npm run *",
      "npx *",
      "git status",
      "git diff",
      "git add",
      "git commit",
      "git log",
      "git branch",
      "cat *",
      "ls *",
      "mkdir *",
      "touch *"
    ],
    "deny": [
      "rm -rf",
      "sudo *",
      "chmod 777",
      "git push --force",
      "npm publish",
      "aws *",
      "kubectl delete"
    ]
  }
}
```

**Guidelines:**
- ✅ Explicitly allow commands Claude needs
- ✅ Deny destructive or irreversible commands
- ✅ Deny production-impacting commands
- ❌ **Never use bypass mode**

### 3.3 Custom Commands

Create reusable prompts for workflows you repeat often. These live in `.claude/commands/` as `.md` files.

**Directory structure:**
```
.claude/
  commands/
    catch-up.md
    review.md
    simplify.md
    new-component.md
    debug.md
```

#### Essential Commands to Create:

**`/catch-up` - Resume context between sessions:**
```markdown
<!-- .claude/commands/catch-up.md -->
Review and summarize the current project state:

1. Read /docs/status.md for last known state
2. Check `git log --oneline -20` for recent commits
3. Check `git status` for uncommitted changes
4. Read any recent entries in /docs/decisions/

Summarize:
- What was completed
- What's in progress
- Any blockers or pending decisions

Update /docs/status.md with current state after this review.
```

**`/plan-feature` - Start new feature development:**
```markdown
<!-- .claude/commands/plan-feature.md -->
Switch to plan mode and help me design this feature.

1. Clarify requirements (ask questions if ambiguous)
2. Identify affected files and components
3. Consider edge cases and error states
4. Outline the implementation steps
5. Identify any risks or dependencies

Do NOT execute any code changes until I approve the plan.
```

**`/review` - Code review staged changes:**
```markdown
<!-- .claude/commands/review.md -->
Review the staged changes (`git diff --staged`).

Check for:
- [ ] Code style violations
- [ ] TypeScript errors or `any` usage
- [ ] Missing error handling
- [ ] Potential bugs or edge cases
- [ ] Missing tests
- [ ] Security concerns
- [ ] Performance issues

Format feedback as a list of specific, actionable items.
```

**`/simplify` - Clean up after implementation:**
```markdown
<!-- .claude/commands/simplify.md -->
Review the code I just wrote/modified and simplify it:

1. Remove redundant code
2. Consolidate duplicate logic
3. Improve variable/function naming
4. Simplify complex conditionals
5. Extract reusable utilities if appropriate

Keep functionality identical. Show me the diff of proposed changes.
```

**`/debug` - Structured debugging:**
```markdown
<!-- .claude/commands/debug.md -->
Help me debug this issue systematically:

1. First, understand the expected vs actual behavior
2. Identify the relevant code paths
3. Add strategic console.logs or use debugger
4. Form hypotheses about the cause
5. Test each hypothesis

Ask me clarifying questions before diving in.
```

### 3.4 The .claudeignore File

Similar to `.gitignore`, this tells Claude which files to ignore.

```gitignore
# .claudeignore

# Dependencies
node_modules/
vendor/

# Build outputs
dist/
build/
.next/

# Sensitive files
.env
.env.*
*.pem
*.key
secrets/

# Large generated files
*.min.js
*.bundle.js
coverage/

# Not relevant to code
*.log
*.lock
```

### 3.5 Hooks Configuration

Hooks run automatically before or after Claude performs actions.

**Configure in `.claude/hooks.json`:**

```json
{
  "hooks": {
    "post-write": {
      "*.ts": "npx prettier --write $FILE",
      "*.tsx": "npx prettier --write $FILE",
      "*.js": "npx prettier --write $FILE",
      "*.json": "npx prettier --write $FILE"
    },
    "post-commit": {
      "*": "npm run lint:fix"
    },
    "pre-push": {
      "*": "npm run test"
    }
  }
}
```

This ensures:
- All code Claude writes is auto-formatted
- Linting runs after commits
- Tests run before any push

---

## 4. Prompting Effectively

**This is the most important skill for your team to develop.** Claude's output quality is directly proportional to your input quality.

### 4.1 The Anatomy of a Good Prompt

```
[Context] + [Specific Task] + [Constraints] + [Output Format]
```

**Bad prompt:**
```
Fix the login bug
```

**Good prompt:**
```
Context: Users are reporting that login fails silently on mobile Safari.

Task: Find and fix the root cause.

Constraints:
- Don't modify the auth API contract
- Solution must work on Safari 15+

Show me the bug first, then propose a fix before implementing.
```

### 4.2 Be Specific, Not Vague

| ❌ Vague | ✅ Specific |
|----------|-------------|
| "Make this better" | "Reduce the time complexity from O(n²) to O(n)" |
| "Fix the bug" | "Fix the null pointer exception on line 47 when user.email is undefined" |
| "Add some tests" | "Add unit tests for the calculateTotal function covering: empty cart, single item, discounts, and tax" |
| "Clean this up" | "Extract the validation logic into a separate validateUserInput() function" |
| "Make it faster" | "This query takes 3s. Optimize it to under 200ms. Consider adding an index on user_id" |

### 4.3 Provide Context Efficiently

**Don't dump everything. Provide what's relevant.**

**Bad:**
```
Here's my entire codebase: [pastes 50 files]
Fix the problem
```

**Good:**
```
I have a React form that should validate on blur.

Relevant files:
- src/components/LoginForm.tsx (the form)
- src/hooks/useValidation.ts (validation hook)

The issue: validation runs on every keystroke instead of on blur.

Current behavior: [describe]
Expected behavior: [describe]
```

### 4.4 Course-Correcting Without Starting Over

When Claude goes in the wrong direction, don't just say "no" or start over.

**Ineffective:**
```
You: That's wrong. Try again.
Claude: [Tries something random, probably still wrong]
```

**Effective:**
```
You: This isn't quite right. The issue is:
1. You're modifying state directly instead of using the setter
2. The useEffect dependency array is missing `userId`

Keep the overall approach but fix these two issues.
```

**Other course-correction phrases:**
- "Keep X but change Y"
- "You're on the right track, but consider..."
- "Go back to step 2 and take a different approach to..."
- "That's close. The one thing missing is..."

### 4.5 Asking Claude to Explain First

For complex tasks, ask Claude to explain its understanding before coding.

```
You: Before implementing, explain back to me:
1. What you understand the requirements to be
2. What approach you'll take
3. What files you'll modify

Let me confirm before you start coding.
```

This catches misunderstandings early, before wasted effort.

### 4.6 Controlling Output Verbosity

**When Claude is too verbose:**
```
Give me just the code, no explanation.
```
```
Summarize in 3 bullet points max.
```
```
Skip the preamble, show me the solution.
```

**When you need more detail:**
```
Walk me through this step by step.
```
```
Explain why you chose this approach over alternatives.
```
```
Add comments explaining the non-obvious parts.
```

### 4.7 Using References Effectively

**Reference specific code:**
```
Look at the handleSubmit function in src/components/Form.tsx, lines 45-67.
Apply the same error handling pattern to the new handleDelete function.
```

**Reference patterns:**
```
Follow the same pattern we used in UserService.ts when you create OrderService.ts
```

**Reference documentation:**
```
According to the React Query docs, queries should...
Implement it that way.
```

### 4.8 The "Yes, And" Technique

Build on Claude's work iteratively instead of rejecting and restarting.

```
You: Create a user registration form

Claude: [Creates basic form]

You: Yes, and add email validation

Claude: [Adds validation]

You: Yes, and add a loading state during submission

Claude: [Adds loading state]

You: Yes, and handle the case where the email is already taken

Claude: [Adds error handling]
```

### 4.9 Prompt Templates for Common Tasks

**Bug Fix:**
```
## Bug Report
**Current behavior:** [what happens now]
**Expected behavior:** [what should happen]
**Steps to reproduce:**
1. [step 1]
2. [step 2]
**Relevant files:** [file paths]
**Error message (if any):** [paste error]

Find the root cause and propose a fix before implementing.
```

**New Feature:**
```
## Feature Request
**Goal:** [what we want to achieve]
**User story:** As a [user], I want to [action] so that [benefit]
**Acceptance criteria:**
- [ ] [criterion 1]
- [ ] [criterion 2]
**Technical constraints:** [any limitations]
**Affected areas:** [what parts of the codebase]

Start in plan mode. Don't implement until I approve the approach.
```

**Refactoring:**
```
## Refactoring Task
**Current state:** [describe the problem]
**Desired state:** [describe the goal]
**Files to refactor:** [list files]
**Must preserve:** [behavior that can't change]
**Out of scope:** [what NOT to touch]

Create a checkpoint first. Show me the plan before executing.
```

---

## 5. Workflow Patterns

### 5.1 The Plan-Execute-Verify Loop

This is the core workflow for any significant change:

```
┌─────────────────────────────────────────────────────────┐
│  1. PLAN MODE                                           │
│     - Describe the task                                 │
│     - Claude creates a detailed plan                    │
│     - Iterate until plan is solid                       │
│     - Create a checkpoint                               │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│  2. EXECUTE (Auto-Accept Mode)                          │
│     - Claude implements the plan                        │
│     - Monitor for unexpected changes                    │
│     - Pause if something looks wrong                    │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│  3. VERIFY                                              │
│     - Run tests                                         │
│     - Manual verification                               │
│     - Run simplifier sub-agent                          │
│     - Run verification sub-agent                        │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│  4. COMMIT                                              │
│     - Review the diff                                   │
│     - Write descriptive commit message                  │
│     - Push for PR                                       │
└─────────────────────────────────────────────────────────┘
```

### 5.2 Starting a New Feature

**Step 1: Enter Plan Mode**
```
You: /plan

I need to add a password reset feature.

Requirements:
- User enters email
- System sends reset link (valid 24 hours)
- Link leads to password reset form
- Password must meet complexity requirements
- User is logged in after successful reset

What's your implementation plan?
```

**Step 2: Iterate on the Plan**
```
Claude: [Provides detailed plan]

You: What about rate limiting to prevent abuse?

Claude: [Updates plan with rate limiting]

You: Good. What about if the email doesn't exist?

Claude: [Updates plan with that case]
```

**Step 3: Approve and Execute**
```
You: Plan looks good. Create a checkpoint, then switch to auto-accept and implement.
```

**Step 4: Verify**
```
You: /verify

Check that the implementation:
- Sends emails correctly
- Tokens expire after 24 hours
- Rate limiting works
- Invalid emails handled gracefully
```

### 5.3 Bug Fixing Workflow

**Step 1: Reproduce and Understand**
```
You: Users report that checkout fails with "undefined" error.

Before fixing, help me understand:
1. Where is this error thrown?
2. What state causes it?
3. How can I reproduce it locally?
```

**Step 2: Diagnose**
```
You: Add logging to trace the data flow through the checkout process.
Let's find where the undefined comes from.
```

**Step 3: Fix with Minimal Change**
```
You: Now that we know the cause, fix it with the smallest possible change.
Don't refactor anything else right now.
```

**Step 4: Prevent Regression**
```
You: Add a test case that would have caught this bug.
```

### 5.4 Refactoring Workflow

**Golden Rule: Never refactor while adding features. Do one or the other.**

**Step 1: Checkpoint**
```
You: I want to refactor the UserService to use the repository pattern.
Create a checkpoint first.
```

**Step 2: Plan Incrementally**
```
You: /plan

Don't refactor everything at once. Give me a plan that:
1. Keeps the app working after each step
2. Has no step that takes more than 30 minutes
3. Has clear verification for each step
```

**Step 3: Execute Step by Step**
```
You: Let's do step 1. Implement it, then I'll verify before we continue.

[After verification]

You: Step 1 works. Proceed to step 2.
```

**Step 4: Compare Final State**
```
You: Show me a summary of all changes made since the checkpoint.
Verify that the external API contract is identical.
```

### 5.5 Code Review Workflow

**Before creating a PR:**
```
You: /review

Review my changes as if you were a senior engineer.
Be critical. What would you flag in a code review?
```

**When reviewing someone else's PR:**
```
You: Review this PR: [paste diff or link]

Focus on:
- Correctness
- Performance implications
- Test coverage
- Potential edge cases
- Style consistency with our codebase
```

## 5.5 Advanced Workflows

Different situations call for different approaches. This section covers proven workflow patterns your team can adopt based on the type of work you're doing.

### 5.5.1 The Orchestrator Pattern

**What it is:** Your main Claude thread acts purely as a coordinator—it doesn't write code itself. Instead, it creates and manages sub-agents, each handling specific tasks. The main thread synthesizes results.

**Why it's powerful:**
- Main context stays clean (only holds the plan and summaries)
- Each sub-agent gets a fresh, focused context window
- Parallel execution speeds up large tasks
- Failed sub-agents don't pollute your main session

**Architecture:**
```
┌─────────────────────────────────────────────────┐
│           Main Thread (Orchestrator)            │
│   - Holds the plan and requirements             │
│   - Creates and delegates to sub-agents         │
│   - Synthesizes results                         │
│   - Makes final decisions                       │
│   - DOES NOT write code directly                │
└─────────────────────────────────────────────────┘
           │         │         │         │
           ▼         ▼         ▼         ▼
      ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐
      │Frontend│ │Backend │ │ Tests  │ │  Docs  │
      │ Agent  │ │ Agent  │ │ Agent  │ │ Agent  │
      └────────┘ └────────┘ └────────┘ └────────┘
           │         │         │         │
           └─────────┴────┬────┴─────────┘
                          ▼
                   ┌────────────┐
                   │  Results   │
                   │ Synthesis  │
                   └────────────┘
```

**When to use:**
- Large features spanning multiple domains
- Work that can be parallelized (frontend + backend + tests)
- When context window management is critical
- Complex multi-file changes

**How to invoke:**
```
You: Act as an orchestrator. Do not write code yourself.

For this feature (user authentication with JWT), create sub-agents for:
1. Backend API routes and middleware
2. Frontend login/register components  
3. Database schema and migrations
4. Test coverage for all of the above

Give each sub-agent specific instructions including:
- What files they own
- What they should NOT touch
- Expected deliverables
- How to report back

Coordinate their work and synthesize results. Flag any conflicts.
```

**Add to CLAUDE.md for orchestrator behavior:**
```markdown
## Orchestrator Mode
When acting as orchestrator:
- Do NOT write code directly
- Break tasks into independent sub-agent assignments
- Each sub-agent gets: objective, file boundaries, deliverables
- Run independent tasks in parallel
- Run dependent tasks sequentially
- Synthesize and review all sub-agent outputs
- Flag conflicts between sub-agents immediately
```

---

### 5.5.2 Parallel vs Sequential Sub-Agents

Not all sub-agent work can run in parallel. Use this decision framework:

**Parallel Dispatch (all conditions must be met):**
- Tasks are independent (no shared state)
- No file overlap between agents
- Clear domain boundaries

**Sequential Dispatch (any condition triggers):**
- Tasks have dependencies (B needs output from A)
- Shared files or state (merge conflict risk)
- Unclear scope (need to understand before proceeding)

**Add routing rules to CLAUDE.md:**
```markdown
## Sub-Agent Routing Rules

**Run in PARALLEL when:**
- 3+ unrelated tasks or independent domains
- No shared state between tasks
- Clear file boundaries with no overlap
- Example: Frontend components + Backend API + Database migrations

**Run SEQUENTIALLY when:**
- Tasks have dependencies (schema → API → frontend)
- Shared files or state exist
- Scope is unclear and needs exploration first
- Example: Research → Planning → Implementation
```

**Example parallel workflow:**
```
You: Spawn three sub-agents in parallel:

1. Frontend Agent: Build the login form component
   - Files: src/components/auth/*
   - Do NOT touch: src/api/*, src/server/*
   
2. Backend Agent: Create auth API endpoints
   - Files: src/server/routes/auth.ts, src/server/middleware/auth.ts
   - Do NOT touch: src/components/*
   
3. Test Agent: Write tests for both
   - Files: tests/auth/*
   - Wait for Agent 1 and 2 to complete first

Wait for all to complete, then review for integration issues.
```

---

### 5.5.3 The 3 Amigos Pattern

**What it is:** Three specialized agents working in sequence—PM Agent → UX Agent → Implementation Agent—each building on the previous output.

**Workflow:**
```
┌──────────────────────────────────────────────────────────┐
│  1. PM Agent (Requirements)                              │
│     - Analyzes the request                               │
│     - Defines acceptance criteria                        │
│     - Identifies technical constraints                   │
│     - Outputs: PRD, technical requirements               │
└──────────────────────────────────────────────────────────┘
                          ↓
┌──────────────────────────────────────────────────────────┐
│  2. UX Agent (Design)                                    │
│     - Takes PM Agent output                              │
│     - Designs component structure                        │
│     - Defines user flows                                 │
│     - Outputs: Component specs, interaction patterns     │
└──────────────────────────────────────────────────────────┘
                          ↓
┌──────────────────────────────────────────────────────────┐
│  3. Implementation Agent (Code)                          │
│     - Takes PM + UX Agent outputs                        │
│     - Implements the solution                            │
│     - Writes tests                                       │
│     - Outputs: Working code, tests                       │
└──────────────────────────────────────────────────────────┘
```

**When to use:**
- New features with unclear requirements
- User-facing features where UX matters
- When you want structured thinking before code

**How to invoke:**
```
You: Use the 3 Amigos pattern for this feature.

Feature: Add a notification preferences page

Step 1 - PM Agent:
- Define what settings users need
- List acceptance criteria
- Identify any technical constraints
- Output a mini PRD

Step 2 - UX Agent (after PM completes):
- Design the page structure
- Define the component hierarchy
- Specify interaction patterns
- Output component specifications

Step 3 - Implementation Agent (after UX completes):
- Implement based on PM + UX specs
- Write tests
- Follow our code conventions

Run each step as a sub-agent. Do not proceed to the next step until I approve.
```

---

### 5.5.4 Domain-Based Splitting

**What it is:** Assign sub-agents based on architectural domains, each owning their vertical slice.

**Common domain splits:**

| Domain | Owns | Example Files |
|--------|------|---------------|
| Frontend | UI, state, routing | `src/components/`, `src/hooks/`, `src/pages/` |
| Backend | API, business logic | `src/server/`, `src/api/`, `src/services/` |
| Database | Schema, migrations, queries | `prisma/`, `src/models/`, `src/repositories/` |
| Infrastructure | Config, deployment | `docker/`, `.github/`, `terraform/` |

**When to use:**
- Full-stack features
- When team members have domain expertise
- Clear separation of concerns in codebase

**How to invoke:**
```
You: Split this feature by domain:

Feature: User profile with avatar upload

Frontend Agent:
- Profile page component
- Avatar upload widget
- Form state management
- Files: src/components/profile/*

Backend Agent:
- Profile API endpoints
- Avatar upload handling (S3)
- Validation logic
- Files: src/server/routes/profile.ts

Database Agent:
- User profile schema updates
- Migration for avatar_url field
- Files: prisma/schema.prisma, prisma/migrations/*

Run frontend and backend in parallel.
Run database first (they depend on schema).
```

---

### 5.5.5 Research-Plan-Execute Pattern

**What it is:** Dedicated research phase before planning, especially useful for unfamiliar territory.

**Workflow:**
```
┌─────────────────────────────────────────────────┐
│  1. RESEARCH (Sub-agents explore in parallel)   │
│     - Agent 1: Explore existing codebase        │
│     - Agent 2: Research external docs/APIs      │
│     - Agent 3: Find similar implementations     │
│     Outputs: Synthesis report                   │
└─────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────┐
│  2. PLAN (Main thread, informed by research)    │
│     - Review research findings                  │
│     - Create detailed implementation plan       │
│     - Identify risks and unknowns               │
│     Output: Approved plan                       │
└─────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────┐
│  3. EXECUTE (Sub-agents implement in parallel)  │
│     - Follow the approved plan                  │
│     - Each agent owns their domain              │
│     Output: Working implementation              │
└─────────────────────────────────────────────────┘
```

**When to use:**
- Integrating with unfamiliar APIs
- Working in a new part of the codebase
- Complex technical decisions needed upfront

**How to invoke:**
```
You: Use Research-Plan-Execute for this task.

Task: Integrate Stripe subscriptions

RESEARCH PHASE (parallel sub-agents):
1. Codebase Agent: Find how we currently handle payments, if at all
2. Stripe Agent: Research Stripe subscription API best practices
3. Patterns Agent: Find how other projects structure subscription logic

Synthesize findings before planning.

PLAN PHASE:
Based on research, create a detailed implementation plan.
Wait for my approval.

EXECUTE PHASE:
Implement according to approved plan.
```

---

### 5.5.6 Quality Gate Pattern

**What it is:** Mandatory verification sub-agents that run after implementation.

**Architecture:**
```
Implementation Complete
         ↓
┌─────────────────────────────────────────────────┐
│              QUALITY GATES                      │
│  (Each is a sub-agent, all must pass)          │
├─────────────────────────────────────────────────┤
│  ✓ Linter Agent: Code style compliance         │
│  ✓ Test Agent: All tests pass, coverage ok     │
│  ✓ Security Agent: No vulnerabilities          │
│  ✓ Simplifier Agent: No unnecessary complexity │
│  ✓ Docs Agent: Documentation updated           │
└─────────────────────────────────────────────────┘
         ↓
    Ready for PR
```

**Create as a /command:**
```markdown
<!-- .claude/commands/quality-gates.md -->
Run quality gates on recent changes. Each gate is a sub-agent.

1. **Lint Gate**: Run linter, fix any issues
2. **Test Gate**: Run tests, ensure all pass, check coverage
3. **Security Gate**: Check for hardcoded secrets, SQL injection, XSS
4. **Simplify Gate**: Review for unnecessary complexity, suggest simplifications
5. **Docs Gate**: Ensure README and comments are updated

Report pass/fail for each gate.
Do not proceed to PR until all gates pass.
```

**Usage:**
```
You: /quality-gates

[Claude runs all gates, reports results]
```

---

### 5.5.7 Workflow Selection Guide

| Situation | Recommended Workflow |
|-----------|---------------------|
| Large feature, multiple domains | Orchestrator Pattern |
| New feature, unclear requirements | 3 Amigos Pattern |
| Full-stack feature | Domain-Based Splitting |
| Unfamiliar territory | Research-Plan-Execute |
| Before every PR | Quality Gate Pattern |
| Independent tasks | Parallel Sub-Agents |
| Dependent tasks | Sequential Sub-Agents |
| Simple bug fix | Single agent, no sub-agents needed |

---

### 5.5.8 Anti-Patterns to Avoid

**❌ Over-parallelizing:**
Spawning 10 agents for a simple feature wastes tokens and creates coordination overhead.

**❌ Under-specifying sub-agents:**
```
# Bad
"Agent 1: Handle the frontend"

# Good  
"Agent 1: Build LoginForm component in src/components/auth/LoginForm.tsx.
 Use our form patterns from src/components/forms/. Include email/password
 fields with validation. Do NOT modify any files outside src/components/auth/."
```

**❌ No file boundaries:**
Agents stepping on each other's files causes merge conflicts.

**❌ Forgetting to synthesize:**
Sub-agents complete but no one reviews for integration issues.

**❌ Skipping verification:**
Shipping sub-agent code without running tests or quality gates.

---

### 5.5.9 Template: Orchestrator Mode Prompt

Save this as a /command for easy access:

```markdown
<!-- .claude/commands/orchestrate.md -->
Act as an orchestrator for this task. You will NOT write code directly.

Task: $ARGUMENTS

Your responsibilities:
1. Analyze the task and break it into sub-agent assignments
2. For each sub-agent, specify:
   - Clear objective
   - Files they own (and files they must NOT touch)
   - Dependencies on other agents
   - Expected deliverables
3. Determine which agents can run in parallel vs sequentially
4. Spawn sub-agents and monitor progress
5. Synthesize results and check for conflicts
6. Run quality gates before marking complete

Routing rules:
- Parallel: Independent domains, no file overlap
- Sequential: Dependencies exist, shared files

Begin by outputting your orchestration plan. Wait for approval before spawning agents.
```

### 5.5.10 Worktree Manager Skill

Create this file at `.claude/skills/worktree-manager/SKILL.md`:

````markdown
# Worktree Manager Skill

You are a **Worktree Manager**—an orchestrator that creates isolated git worktrees and launches Claude Code instances in separate terminal windows.

## What You Do

1. Create git worktrees for parallel development
2. Open new terminal windows (PowerShell on Windows, Terminal on macOS)
3. Launch Claude Code in each worktree
4. Help coordinate work across worktrees
5. Clean up worktrees when done

## Detecting the Operating System

Before running any terminal commands, detect the OS:

```bash
# Check OS
uname -s 2>/dev/null || echo "Windows"
```

- If result contains "Darwin" → macOS
- If result contains "Windows" or command fails in PowerShell → Windows

## Creating a Worktree

When asked to create a worktree:

```bash
# 1. Create the worktree
git worktree add ../worktree-<branch-name> -b <branch-name>

# 2. Copy environment files if they exist
cp .env ../worktree-<branch-name>/.env 2>/dev/null || true

# 3. Navigate and install dependencies
cd ../worktree-<branch-name>
npm install
```

## Launching Claude in a New Terminal

### Windows (PowerShell)

```powershell
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '<worktree-path>'; claude"
```

### macOS (Terminal)

```bash
osascript -e 'tell application "Terminal" to do script "cd <worktree-path> && claude"'
```

### macOS (iTerm2) - if installed

```bash
osascript -e 'tell application "iTerm" to create window with default profile command "cd <worktree-path> && claude"'
```

## Listing Active Worktrees

```bash
git worktree list
```

## Cleaning Up a Worktree

```bash
# 1. Remove the worktree
git worktree remove ../worktree-<branch-name>

# 2. Delete the branch if no longer needed
git branch -d <branch-name>
```

## Handling Port Conflicts

If a dev server fails because the port is in use, instruct the user:

**Node/npm:**
```bash
npm run dev -- --port 3001
```

**Vite:**
```bash
npm run dev -- --port 3001
```

**Next.js:**
```bash
npm run dev -- -p 3001
```

**General rule:** Increment the port number for each worktree (3000, 3001, 3002, etc.)

## Example Interactions

**User:** Create a worktree for feature/auth

**You:**
1. Create worktree at `../worktree-feature-auth`
2. Copy .env file
3. Install dependencies
4. Open new terminal with Claude Code
5. Report back with the path and next steps

**User:** What worktrees are active?

**You:** Run `git worktree list` and show the results.

**User:** Clean up the auth worktree

**You:**
1. Confirm any uncommitted changes are saved
2. Remove the worktree
3. Optionally delete the branch
4. Report completion

## Important Rules

1. **Never use `--dangerously-skip-permissions`** - launch Claude in normal mode
2. **Always check for uncommitted changes** before cleaning up a worktree
3. **Copy .env files** so each worktree has its environment configured
4. **Run `npm install`** in new worktrees (node_modules aren't shared)
5. **Remind users about ports** if they're running multiple dev servers
````

### 5.5.11 The Worktree Manager Workflow

**What it is:** A workflow pattern where your main Claude thread acts as a manager that creates isolated git worktrees and spawns separate Claude Code instances in new terminal windows.

> **Git Worktree:** A Git feature that lets you have multiple branches checked out simultaneously in separate folders. Each worktree is independent but shares the same git history.

**Why use this:**
- Each Claude instance gets a **completely fresh context window**
- **No file conflicts**—each worktree has its own copy of the code
- **Visual monitoring**—each agent runs in its own terminal window
- **True parallelism**—work on multiple features simultaneously

**Architecture:**
```
┌─────────────────────────────────────────────────────────┐
│              Main Terminal (You + Claude)               │
│                   Worktree Manager                      │
│                                                         │
│   "Create worktrees for auth, cart, and checkout"      │
└─────────────────────────────────────────────────────────┘
                          │
          ┌───────────────┼───────────────┐
          ▼               ▼               ▼
   ┌─────────────┐ ┌─────────────┐ ┌─────────────┐
   │  Terminal 2 │ │  Terminal 3 │ │  Terminal 4 │
   │             │ │             │ │             │
   │ worktree-   │ │ worktree-   │ │ worktree-   │
   │ auth/       │ │ cart/       │ │ checkout/   │
   │             │ │             │ │             │
   │ Claude:     │ │ Claude:     │ │ Claude:     │
   │ "Build JWT  │ │ "Build cart │ │ "Build      │
   │  login..."  │ │  logic..."  │ │  Stripe..." │
   └─────────────┘ └─────────────┘ └─────────────┘
```

---

#### Installation

Add the skill to your project:

```
.claude/
  skills/
    worktree-manager/
      SKILL.md        # The skill file above
```

Or install globally for all projects:

```
~/.claude/
  skills/
    worktree-manager/
      SKILL.md
```

---

#### Basic Commands

**Create a worktree:**
```
You: Create a worktree for feature/user-profile
```

Claude will:
1. Run `git worktree add ../worktree-user-profile -b feature/user-profile`
2. Copy your `.env` file
3. Run `npm install`
4. Open a new terminal window with Claude Code

**List active worktrees:**
```
You: What worktrees do I have?
```

Claude runs `git worktree list` and shows you all active worktrees.

**Clean up a worktree:**
```
You: Clean up the user-profile worktree
```

Claude will:
1. Check for uncommitted changes (warn you if any)
2. Remove the worktree
3. Optionally delete the branch

---

#### Cross-Platform Support

The skill works on both Windows and macOS:

| OS | Terminal Used | How Claude Opens It |
|----|---------------|---------------------|
| Windows | PowerShell | `Start-Process powershell` |
| macOS | Terminal.app | `osascript` command |
| macOS | iTerm2 (if installed) | `osascript` for iTerm |

Claude auto-detects your OS and uses the right command.

---

#### Handling Port Conflicts

When running multiple dev servers, each needs a different port. If you get a "port in use" error:

```bash
# Instead of default port 3000, use 3001, 3002, etc.
npm run dev -- --port 3001
```

**Quick reference:**
| Worktree | Suggested Port |
|----------|----------------|
| Main project | 3000 |
| Worktree 1 | 3001 |
| Worktree 2 | 3002 |
| Worktree 3 | 3003 |

---

#### Example: Building Features in Parallel

**Scenario:** You need to build three independent features.

**Step 1: Create worktrees from your main terminal**
```
You: I need to work on three features in parallel:
1. feature/dark-mode - UI theming
2. feature/notifications - Push notification system  
3. feature/export-pdf - PDF export functionality

Create worktrees for each and launch Claude in new terminals.
```

**Step 2: Claude creates everything**
```
Claude: Created 3 worktrees:

1. ../worktree-dark-mode (Terminal opened)
2. ../worktree-notifications (Terminal opened)
3. ../worktree-export-pdf (Terminal opened)

Each has Claude Code running. Switch to each terminal 
and give it instructions for its specific task.
```

**Step 3: Instruct each agent**

Switch to each terminal window and give specific instructions:

```
# Terminal 2 (dark-mode)
You: Implement dark mode theming using CSS variables.
Add a toggle in the settings page.

# Terminal 3 (notifications)
You: Set up push notifications using Firebase.
Add a notification preferences component.

# Terminal 4 (export-pdf)
You: Add PDF export using jsPDF.
Users should be able to export reports.
```

**Step 4: Monitor and merge**

Check progress from your main terminal:
```
You: List all worktrees and their git status
```

When a feature is complete:
```
You: The dark-mode worktree is done.
Merge it to main and clean up.
```

---

#### Example: Emergency Bug Fix

**Scenario:** You're mid-feature when an urgent bug is reported.

**Old way (context switching pain):**
1. Stash your changes
2. Switch branches
3. Fix bug
4. Switch back
5. Pop stash
6. Try to remember where you were

**Worktree way:**
```
You: I'm in the middle of the dashboard feature but need to fix 
an urgent payment bug. Create a worktree for fix/payment-bug.
```

Now you have:
- Your dashboard work untouched in the main directory
- A new terminal with Claude fixing the payment bug
- No context switching, no stashing

---

#### Best Practices

**1. Keep worktrees independent**

Only create worktrees for truly independent work:
```
✅ Good: Frontend feature + Backend feature + Docs update
❌ Bad: Two features that modify the same files
```

**2. Give each agent clear boundaries**
```
You: You're working in the notifications worktree.
Only modify files in src/notifications/.
Do not touch any other directories.
```

**3. Commit frequently in worktrees**
```
You: Commit your progress before I clean up this worktree.
```

**4. Merge in the right order**

If features have dependencies:
```
You: Merge in this order:
1. Database schema changes first
2. Backend API second
3. Frontend last
```

**5. Clean up when done**

Don't leave stale worktrees around:
```
You: List all worktrees. Clean up any that are merged.
```

---

#### Worktree vs Sub-Agent: When to Use Each

| Use Case | Better Option |
|----------|---------------|
| Quick parallel tasks (< 30 min) | Sub-agents |
| Long-running features (hours) | Worktrees |
| Need to see multiple terminals | Worktrees |
| Simple, coordinated tasks | Sub-agents |
| Fully independent work | Worktrees |
| Heavy file changes | Worktrees |

**Rule of thumb:** If you want to "set it and forget it" while working on something else, use worktrees.

---

#### Troubleshooting

**"Worktree already exists"**
```bash
# List existing worktrees
git worktree list

# Remove the old one first
git worktree remove ../worktree-name
```

**"Branch already exists"**
```bash
# Use existing branch instead of creating new
git worktree add ../worktree-name existing-branch-name
```

**"Cannot remove worktree with uncommitted changes"**
```bash
# Either commit the changes
cd ../worktree-name && git add . && git commit -m "WIP"

# Or force remove (loses changes!)
git worktree remove --force ../worktree-name
```

**Terminal doesn't open (Windows)**

Make sure you're running PowerShell as your shell. The command `Start-Process powershell` needs PowerShell to work.

**Terminal doesn't open (macOS)**

Make sure Terminal.app has permissions in System Preferences → Security & Privacy → Automation.

### 5.6 Sub-Agent Workflows

#### Setting Up the Simplifier Agent

```
You: Create a sub-agent for code simplification.

Instructions for the sub-agent:
- Review all files modified in this session
- Look for: redundant code, duplicate logic, unclear names
- Propose simplifications
- Keep functionality identical
- Present changes as a diff for my approval
```

#### Setting Up the Verification Agent

```
You: Create a sub-agent for verification.

Instructions for the sub-agent:
- Run the full test suite
- Verify the app builds without warnings
- Check that new features work end-to-end
- Test edge cases explicitly
- Report any issues with steps to reproduce
```

#### Orchestrating Multiple Sub-Agents

```
You: I've finished the feature. Run the following in sequence:

1. Simplifier agent - clean up my code
2. Verification agent - make sure everything works
3. Report back with results

Don't modify anything the simplifier suggests until I approve.
```

### 5.7 Background Agents for Long Tasks

For tasks that take a long time:

```
You: This migration will touch 200 files.
Run it as a background agent so I can do other work.
Notify me when complete or if you hit an error.
```

**Checking on background agents:**
```
You: /status

What's the progress on the background migration task?
```

### 5.8 End-of-Session Ritual

Before ending a session, always:

```
You: I'm ending this session. Please:

1. Summarize what we accomplished
2. List any incomplete tasks
3. Note any decisions that were made
4. Update /docs/status.md with this information
5. Commit any uncommitted work with appropriate messages
```

---

## 6. Context Management

### 6.1 Progressive Disclosure

**Definition:** Providing information to Claude step-by-step rather than all at once.

**Why it matters:**
- Keeps context window focused
- Improves response quality
- Prevents Claude from getting distracted by irrelevant details

**Bad approach:**
```
Here's my entire codebase structure, all the requirements, the database schema,
the API documentation, and the deployment configuration.

Now fix this small bug in the login form.
```

**Good approach:**
```
I have a bug in the login form.

[Describe the bug]

The relevant file is src/components/LoginForm.tsx.

[Only if Claude asks for more]: Here's the validation hook it uses...
```

**Key principle:** Provide just enough context. If Claude needs more, it will ask.

### 6.2 Attention Budget

Teach Claude to manage its own context by including in your CLAUDE.md:

```markdown
## Attention Budget
You have limited context. Be strategic:
- Don't load files you don't immediately need
- Summarize long outputs instead of keeping full text
- When done with a subtask, note the result but clear the details
- Ask before loading large files
```

Tell Claude explicitly:
```
You: Information not needed right now doesn't need to live in context.
Summarize what you learned from that file and move on.
```

### 6.3 Token-Efficient File Formats

**Use YAML for structured data (saves tokens vs. verbose JSON/SQL):**

```yaml
# database-schema.yaml
users:
  id: uuid, primary
  email: string, unique, indexed
  password_hash: string
  created_at: timestamp
  
orders:
  id: uuid, primary
  user_id: uuid, foreign(users.id)
  total: decimal
  status: enum(pending, paid, shipped, delivered)
```

**Use Markdown for documentation:**

```markdown
# Auth Flow
1. User submits credentials
2. Server validates and returns JWT
3. Client stores in httpOnly cookie
4. Subsequent requests include cookie
```

### 6.4 External Memory with Files

Since Claude forgets between sessions, use external files as persistent memory.

**Directory structure:**
```
/docs
  /decisions         # Architectural Decision Records
    2026-01-15-auth-approach.md
    2026-01-18-state-management.md
  /status.md         # Current work status
  /tech-debt.md      # Known technical debt
  /issues.md         # Active issues being tracked
```

**Decision Record Template:**
```markdown
# [Date] - [Decision Title]

## Context
[Why this decision was needed]

## Decision
[What was decided]

## Consequences
[What this means going forward]

## Alternatives Considered
[What else was considered and why it was rejected]
```

### 6.5 Git as Memory

Use git history to maintain context across sessions:

```
You: /catch-up

Check git log for recent commits and summarize what's been done.
```

**Commit messages should be descriptive:**

```bash
# Bad
git commit -m "fixes"

# Good
git commit -m "fix(auth): handle expired JWT tokens gracefully

- Add token refresh logic when receiving 401
- Show user-friendly message if refresh fails
- Redirect to login after failed refresh

Fixes #123"
```

### 6.6 When to Start Fresh

Signs your context is "polluted" and you should start a new session:

- Claude is referencing things incorrectly from earlier
- Claude seems confused about the current state
- You've changed direction significantly
- The conversation has gone on for 50+ exchanges
- Claude starts making mistakes it wasn't making before

**How to hand off to a new session:**
```
You: I'm going to start a fresh session.
Before I do, write a summary to /docs/handoff.md containing:
- What we've accomplished
- Current state of the code
- What still needs to be done
- Any important context the next session needs
```

### 6.7 Using /clear

Reset context without ending the session:

```
You: /clear

Keep only:
- The CLAUDE.md contents
- The current git status
- The file I'm about to share

Forget everything else from this conversation.
```

### 6.8 Warming Up a New Session

Efficiently bring Claude up to speed:

```
You: Starting a new session. Quick context:

Project: E-commerce platform
Current task: Implementing wishlist feature
Status: Database schema done, working on API endpoints
Key files:
- src/models/Wishlist.ts (new)
- src/routes/wishlist.ts (in progress)

Run /catch-up for more detail if needed.
```

---

## 7. Debugging & Recovery

### 7.1 Systematic Debugging with Claude

**Step 1: Describe the problem precisely**
```
You: Expected: Clicking "Add to Cart" increases cart count by 1
Actual: Cart count increases by 2 every time
Reproducible: 100% of the time in Chrome and Firefox
```

**Step 2: Form hypotheses**
```
You: What could cause a double-increment?
List the possible causes before we investigate.
```

**Step 3: Test each hypothesis**
```
You: Let's test hypothesis 1 first.
Add a console.log before and after the increment.
Don't fix anything yet, just gather data.
```

**Step 4: Find root cause**
```
You: The data shows the event handler is firing twice.
Now find WHY it fires twice. Is the component mounted twice?
Is there a duplicate event listener?
```

**Step 5: Fix and verify**
```
You: Now we know the cause. Implement the fix.
Then verify the original bug is resolved and no new issues appeared.
```

### 7.2 Sharing Error Information

**Good error report:**
```
You: Getting this error:

Error: Cannot read property 'map' of undefined
    at ProductList (src/components/ProductList.tsx:24:18)
    at renderWithHooks (react-dom.development.js:14985:18)

This happens when I navigate to /products directly (not from home).
If I navigate from home → products, it works fine.

The products array should come from this API call: [describe]
```

**Bad error report:**
```
You: It doesn't work
```

### 7.3 Rollback Strategies

**Using checkpoints:**
```
You: That change broke everything. Restore the checkpoint from before the refactor.
```

**Using git:**
```
You: Discard all changes and go back to the last commit.
```
```
You: Undo only the changes to UserService.ts, keep everything else.
```

**Selective rollback:**
```
You: The auth changes work, but the profile changes broke things.
Revert only the files in src/profile/ to their state before this session.
```

### 7.4 When Claude Breaks Something

Don't panic. Do this:

1. **Stop Claude from making more changes:**
   ```
   You: Stop. Don't make any more changes.
   ```

2. **Assess the damage:**
   ```
   You: Show me all files you modified this session.
   What was the original state vs. current state?
   ```

3. **Decide on recovery strategy:**
   ```
   You: Which of these changes are good and should be kept?
   Which should be reverted?
   ```

4. **Recover:**
   ```
   You: Revert the broken changes only.
   Keep the working ones.
   ```

### 7.5 Debugging Claude Itself

When Claude's behavior is the problem:

**Claude is stuck in a loop:**
```
You: Stop. You've tried the same approach 3 times.
Step back and explain what you're trying to do.
Let's find a different approach.
```

**Claude made assumptions:**
```
You: You assumed X, but actually Y.
Go back to where you made that assumption and redo from there.
```

**Claude is making things up:**
```
You: That file/function doesn't exist.
Run `ls src/utils` and show me what actually exists.
Don't assume - verify.
```

### 7.6 For Long-Running Tasks: Verify Work

Add this to your CLAUDE.md or say it for long tasks:

```
For any task taking more than 10 minutes:
- Pause every few steps and verify your work
- Run tests after each major change
- Check that the app still builds
- Don't continue if something is broken
```

---

## 8. Optimization

### 8.1 Cost Awareness

**How pricing works:**
- Input tokens (what you send to Claude): $X per million
- Output tokens (what Claude generates): $Y per million
- Thinking tokens (when extended thinking is on): $Z per million

**Check your usage:**
```bash
claude usage
```

### 8.2 Strategies to Reduce Costs

**1. Use compact mode for simple tasks:**
```bash
claude config set compactMode true
```
Or per-session:
```
You: /compact
```

**2. Be specific to reduce back-and-forth:**
```
# Costs more (multiple exchanges)
You: Create a function
Claude: What should it do?
You: Validate email
Claude: What validation rules?
You: Standard email format
Claude: [Creates function]

# Costs less (single exchange)
You: Create a validateEmail function that:
- Uses regex for standard email format
- Returns {valid: boolean, error?: string}
- Handles empty input
Claude: [Creates function]
```

**3. Don't load unnecessary files:**
```
You: Only read the function I mentioned, not the whole file.
```

**4. Summarize instead of keeping full text:**
```
You: Summarize what you found in 2 sentences, then close that file.
```

**5. Use sub-agents for isolated tasks:**
- Each has its own context
- Avoids polluting main context with temporary exploration

### 8.3 When to Use Lighter Models

Not every task needs Opus 4.5. Consider lighter models for:

- Simple formatting tasks
- Generating boilerplate
- Basic explanations
- Quick lookups

Use Opus 4.5 for:

- Complex reasoning
- Architecture decisions
- Tricky bugs
- Anything requiring judgment

### 8.4 Compact Mode

**What it does:** Claude produces shorter responses, fewer explanations.

**Enable it:**
```
You: /compact
```

**Good for:**
- When you know what you're doing
- Rapid iteration
- Simple tasks

**Disable for:**
- Learning new concepts
- Complex decision-making
- Debugging (you want verbose info)

### 8.5 Efficient Code Reading

**Don't:**
```
You: Read the entire file and understand it.
```

**Do:**
```
You: Read only lines 45-80 of src/services/UserService.ts.
I need to understand the updateProfile method.
```

**Do:**
```
You: What does the handleAuth function return?
Just tell me the return type and a one-sentence summary.
```

---

## 9. Security

### 9.1 What NOT to Share

**Never include in prompts or CLAUDE.md:**
- API keys
- Database credentials
- Private keys or certificates
- JWT secrets
- OAuth client secrets
- Any password or token
- Production URLs with sensitive endpoints

### 9.2 Using Environment Variables

**Bad:**
```
You: Connect to the database at postgres://admin:secretpassword123@prod.db.com
```

**Good:**
```
You: Connect to the database using the DATABASE_URL environment variable.
```

**In your CLAUDE.md:**
```markdown
## Environment Variables
Use these env vars (never hardcode values):
- DATABASE_URL: Database connection string
- JWT_SECRET: Token signing key
- API_KEY: External service API key

Never commit .env files. Copy from .env.example.
```

### 9.3 The .claudeignore File

Prevent Claude from reading sensitive files:

```gitignore
# .claudeignore

# Environment files
.env
.env.*
*.env

# Secrets
secrets/
*.pem
*.key
*secret*

# CI/CD with secrets
.github/workflows/*-deploy.yml

# Credentials
credentials.json
service-account.json
```

### 9.4 Reviewing Claude's Code for Security

Before committing code Claude generates, check for:

**1. Hardcoded values:**
```typescript
// BAD - Claude might do this
const apiKey = "sk-abc123...";

// GOOD
const apiKey = process.env.API_KEY;
```

**2. Exposed internal details:**
```typescript
// BAD - exposes internal structure
return { error: `Database error: ${err.message}` };

// GOOD
return { error: "An unexpected error occurred" };
```

**3. SQL injection / XSS:**
```typescript
// BAD
const query = `SELECT * FROM users WHERE id = ${userId}`;

// GOOD
const query = `SELECT * FROM users WHERE id = $1`;
```

**4. Overly permissive CORS or permissions:**
```typescript
// BAD - Claude might do this for "simplicity"
app.use(cors({ origin: '*' }));

// GOOD
app.use(cors({ origin: process.env.ALLOWED_ORIGINS.split(',') }));
```

### 9.5 Security Checklist for Claude-Generated Code

Add to your PR review checklist:

- [ ] No hardcoded secrets or credentials
- [ ] Environment variables used for configuration
- [ ] Error messages don't expose internals
- [ ] User input is validated and sanitized
- [ ] Database queries use parameterization
- [ ] Auth checks are present where needed
- [ ] CORS and permissions are restrictive

---

## 10. Common Pitfalls & Solutions

### 10.1 Hallucinated Files or Functions

**The problem:** Claude references files, functions, or APIs that don't exist.

**Solution:**
```
You: Before using any function, verify it exists.
Run `grep -r "functionName" src/` to check.
Don't assume - verify.
```

**Preventive measure in CLAUDE.md:**
```markdown
## Verification Rule
Before calling any function or importing any module:
1. Verify it exists with grep or file reading
2. Check the actual signature/interface
3. Don't assume based on naming conventions
```

### 10.2 Overly Eager Changes

**The problem:** Claude changes things you didn't ask it to change.

**Example:**
```
You: Fix the bug in the login function

Claude: [Fixes the bug AND refactors the whole auth module AND updates dependencies]
```

**Solution:**
```
You: Fix ONLY the specific bug I described.
Don't refactor anything else.
Don't update any dependencies.
Don't improve code style.

Just the bug fix, nothing more.
```

**Preventive measure in CLAUDE.md:**
```markdown
## Scope Rule
When asked to do X, do ONLY X unless explicitly asked to do more.
Ask before making changes outside the requested scope.
```

### 10.3 Over-Apologizing and Over-Correcting

**The problem:** When you give feedback, Claude apologizes profusely and swings too far in the other direction.

**Example:**
```
You: This function is too long

Claude: "I sincerely apologize! You're absolutely right! I've now split it into 15 micro-functions..."
```

**Solution:**
```
You: Don't over-correct. The function was 100 lines, maybe 30 would be right.
Don't split it into a dozen tiny functions.

Also, no need to apologize - just fix it.
```

### 10.4 Stuck in Loops

**The problem:** Claude keeps trying the same failing approach.

**Signs:**
- Same error appears repeatedly
- Claude says "Let me try again" and does the same thing
- Token usage spikes without progress

**Solution:**
```
You: Stop. You've tried this same approach 3 times and it's not working.

1. Explain what you're trying to achieve
2. Explain what's blocking you
3. List 3 DIFFERENT approaches you haven't tried
4. Let's pick a new approach together
```

### 10.5 Claude Can't Find the Bug

**The problem:** Going in circles without finding the root cause.

**Solution:**
```
You: Let's be systematic. Create a hypothesis log:

| Hypothesis | Test | Result |
|------------|------|--------|
| [What might be wrong] | [How to verify] | [What happened] |

We'll fill this out together until we find it.
```

### 10.6 Wrong Mental Model of Your Codebase

**The problem:** Claude makes wrong assumptions about how your code is structured.

**Solution:**
```
You: You seem to be assuming [wrong assumption].

Actually:
- We use [X] not [Y]
- Data flows like [this]
- State is managed with [this approach]

Re-read the CLAUDE.md and adjust your understanding.
```

### 10.7 Generated Code Doesn't Match Style

**The problem:** Claude writes code that looks different from your codebase.

**Solution:**
```
You: Look at src/services/ExistingService.ts.
Match that style exactly:
- Same import ordering
- Same function structure
- Same naming conventions
- Same comment style
```

**Preventive measure:** Strengthen your CLAUDE.md code style section with specific examples.

### 10.8 Claude Gets Verbose

**The problem:** Claude writes lengthy explanations when you just want code.

**Solution:**
```
You: Just the code, no explanation.
```
```
You: Give me 3 bullet points max.
```
```
You: Skip preamble.
```

### 10.9 Claude Claims It Can't Do Something

**The problem:** Claude says "I can't access the internet" or "I can't run commands" when it can.

**Solution:**
```
You: You do have the ability to run commands.
Try running `ls -la` and show me the output.
```

(Claude Code does have these capabilities - sometimes Claude forgets its current environment.)

### 10.10 Timeout on Long Operations

**The problem:** Long-running commands time out.

**Solution:**
```
You: Run this as a background task.
Checkpoint progress every 5 minutes.
I'll check back for results.
```

---

## 11. Team Practices

### 11.1 Sharing CLAUDE.md Improvements

When someone discovers a useful instruction:

1. Test it in their session
2. If it works well, add to CLAUDE.md
3. Commit with message: `docs(claude): add [guideline] for [reason]`
4. Share in team channel

### 11.2 Shared Command Library

Create a team library of /commands:

```
.claude/
  commands/
    team/             # Team-wide commands
      catch-up.md
      review.md
      deploy-check.md
    personal/         # Individual preferences (gitignored)
      my-workflow.md
```

### 11.3 Code Review of Claude-Generated Code

Claude's code needs review just like human code. Pay special attention to:

| Area | What to Check |
|------|---------------|
| **Logic** | Does it actually solve the problem correctly? |
| **Edge cases** | Did Claude handle null, empty, error states? |
| **Style** | Does it match team conventions? |
| **Security** | No hardcoded secrets, proper validation? |
| **Tests** | Are tests meaningful or just superficial? |
| **Performance** | Any obvious N+1 queries or inefficiencies? |

### 11.4 When NOT to Use Claude

Be honest about limitations. Human judgment is better for:

- **Security-critical code**: Auth, encryption, access control (use Claude as first draft, expert review essential)
- **Performance-critical paths**: Hot loops, real-time systems (Claude may not optimize correctly)
- **Domain-specific logic**: Complex business rules unique to your company
- **Architectural decisions**: Claude can propose, but humans should decide
- **Sensitive data handling**: HIPAA, PII, financial data

### 11.5 Avoiding Conflicts

When multiple team members use Claude on the same codebase:

1. **Work on separate branches** - Normal git workflow applies
2. **Coordinate on shared files** - "I'm working on UserService today"
3. **Pull before starting** - Ensure Claude sees latest code
4. **Keep sessions short** - Merge frequently to avoid large divergences

### 11.6 Knowledge Sharing

**Weekly Claude Tips Session (15 min):**
- What worked well?
- What prompts did you discover?
- What pitfalls did you hit?
- Any CLAUDE.md improvements to propose?

**Document in a shared location:**
```markdown
# Claude Code Team Learnings

## 2026-01-19
- @alice: Found that asking Claude to "explain before implementing" catches bugs early
- @bob: Don't ask Claude to refactor while adding features - do one at a time
- @carol: Added /deploy-check command that runs our full validation suite
```

### 11.7 Onboarding New Team Members

For new team members:

1. Have them read this guide
2. Pair with experienced Claude user for first day
3. Start with low-risk tasks (tests, docs, simple features)
4. Review their Claude interactions for first week
5. Collect their fresh-eyes feedback on CLAUDE.md gaps

---

## 12. Quick Reference

### Essential Commands

| Command | Purpose |
|---------|---------|
| `/plan` | Enter plan mode (no execution) |
| `/auto` | Enable auto-accept for allowed commands |
| `/compact` | Shorter responses, less explanation |
| `/clear` | Clear context, start fresh |
| `/catch-up` | Summarize current state from git/docs |
| `/review` | Review staged changes |
| `/simplify` | Clean up recently written code |

### Mode Cheatsheet

| Situation | Mode |
|-----------|------|
| Starting new feature | Plan mode |
| Executing approved plan | Auto-accept |
| Sensitive/learning | Interactive |
| Quick question | Compact |

### Prompt Starters

| Need | Prompt |
|------|--------|
| Understand before acting | "Before implementing, explain your understanding of..." |
| Control scope | "Do ONLY this, nothing else..." |
| Get brevity | "Just the code, no explanation" |
| Course correct | "Keep X but change Y..." |
| Debug systematically | "Let's form hypotheses and test each one..." |

### Workflow Checklist

**Before starting a feature:**
- [ ] Pull latest changes
- [ ] Create feature branch
- [ ] Enter plan mode
- [ ] Create checkpoint

**Before committing:**
- [ ] Run `/review`
- [ ] Run simplifier agent
- [ ] Run verification agent
- [ ] Check for hardcoded secrets
- [ ] Write descriptive commit message

**End of session:**
- [ ] Summarize work in `/docs/status.md`
- [ ] Commit all changes
- [ ] Note any incomplete tasks

### Red Flags (Stop and Reassess)

🚩 Claude is looping (same approach 3+ times)
🚩 Claude is modifying files you didn't mention
🚩 Token usage spiking without progress
🚩 Claude referencing things that don't exist
🚩 You're confused about what Claude did

---

## 13. Glossary

| Term | Definition |
|------|------------|
| **Context Window** | The total text Claude can "see" at once (~200K tokens for Opus 4.5) |
| **Token** | A chunk of text, roughly 4 characters or ¾ of a word |
| **CLAUDE.md** | Project configuration file that instructs Claude about your codebase |
| **MCP** | Model Context Protocol - connects Claude to external systems (APIs, databases) |
| **Skill** | Reusable prompt/instruction for teaching Claude a pattern |
| **Sub-Agent** | Separate Claude instance for isolated tasks, with its own context |
| **Background Agent** | Sub-agent that runs long tasks independently |
| **Plan Mode** | Mode where Claude describes actions without executing |
| **Auto-Accept** | Mode where Claude executes allowed commands automatically |
| **Checkpoint** | Snapshot of project state for easy rollback |
| **Hook** | Script that runs automatically before/after Claude actions |
| **Progressive Disclosure** | Providing information step-by-step, not all at once |
| **Compact Mode** | Setting that produces shorter, less verbose responses |
| **.claudeignore** | File listing paths Claude should not read |
| **Extended Thinking** | Feature allowing Claude to reason step-by-step before responding |

---

## Team Adoption Checklist

### Week 1: Foundation
- [ ] All team members have Claude Code installed and authenticated
- [ ] CLAUDE.md created for main project (< 2,500 tokens)
- [ ] Dangerous commands denied in settings.json
- [ ] Everyone reads this guide

### Week 2: Commands & Workflow
- [ ] `/catch-up` command created
- [ ] `/review` command created
- [ ] `/simplify` command created
- [ ] Team practices plan-execute-verify workflow

### Week 3: Advanced Features
- [ ] Simplifier sub-agent configured
- [ ] Verification sub-agent configured
- [ ] Post-write formatting hook configured
- [ ] .claudeignore set up for sensitive files

### Week 4: Optimization
- [ ] First team retrospective on Claude usage
- [ ] CLAUDE.md refined based on learnings
- [ ] Shared command library established
- [ ] Cost monitoring in place

---

*Version 1.0 - January 2026*
*Maintained by: [Your Team]*

