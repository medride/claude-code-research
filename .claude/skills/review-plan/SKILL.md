---
name: review-plan
description: Automates plan review with a staff engineer agent. Spawns reviewer, presents feedback, handles iterations - all through guided prompts.
---

# Review Plan

Automates the plan review workflow. After a plan is created, this skill spawns a "staff engineer" agent to review it, then presents both the original plan and review to the user for approval.

## Usage

```
/review-plan
```

That's it. No flags, no arguments. Everything is guided through prompts.

---

## Workflow

### 1. Locate Plan File

**If multiple plans exist in `~/~/.claude/plans/` (user home directory):**
Use AskUserQuestion to let user choose:
```
Question: "Which plan would you like to review?"
Header: "Plan"
Options: [list of plans with modification times]
```

**If single plan exists:**
Auto-select it, no prompt needed.

**If no plans exist:**
Display error: "No plan found in ~/.claude/plans/. Create one first with plan mode."

### 2. Read the Plan

Read the full plan content and extract:
- Feature/project name (from `# Plan: [Name]` or first heading)
- Summary section
- Phases/sections
- Any existing review notes

### 3. Gather Context for Reviewer

Before spawning the reviewer, gather context they'll need.

**Always read:**
- Full plan content
- `CLAUDE.md` - project conventions, architecture, constraints
- `external-memory/status.md` - current workstreams


**Conditionally read (if plan references them):**
- Relevant `master-plan.md` files mentioned
- Key files mentioned in the plan (first ~100 lines each)

**Context gathering steps:**
1. Read CLAUDE.md for project conventions
2. Read status.md for current workstreams
3. Parse plan for file/directory references (look for paths like `src/`, `tasks/`, etc.)
4. For each reference found, read relevant snippets
5. Bundle all context for the reviewer agent

### 4. Spawn Staff Engineer Reviewer

Launch a **Plan agent** with staff engineer persona.

**Agent prompt template:**
```
You are a Staff Engineer reviewing a technical plan for this project.

## Your Context

### Project Conventions (from CLAUDE.md):
$CLAUDE_MD_CONTENT

### Current Workstreams (from status.md):
$STATUS_MD_CONTENT

### Referenced Code (snippets from files mentioned in plan):
$REFERENCED_FILES_CONTENT

## Plan to Review:
$PLAN_CONTENT

---

## Your Review Criteria

1. **Convention alignment**: Does this follow our project patterns?
2. **Completeness**: Are all necessary components covered?
3. **Risks & edge cases**: What could go wrong?
4. **Conflicts**: Does this clash with current workstreams?
5. **Complexity**: Is the scope realistic? Over-engineered?
6. **Assumptions**: What needs validation before starting?

Be direct and critical. Good plans can handle scrutiny.

## Output Format

Structure your review exactly like this:

# Plan Review: [Feature Name]

**Reviewed:** [current date/time]
**Plan file:** [path to original plan]
**Iteration:** $ITERATION_NUMBER

## Overall Assessment
[APPROVE / CONCERNS / NEEDS WORK] - [1-2 sentence summary]

## Strengths
- [What's solid about this plan]

## Concerns
- [Specific issues that should be addressed]
- [Be actionable - say what to fix, not just what's wrong]

## Questions
- [Things that need clarification before implementation]

## Recommendations
- [Concrete improvements to make the plan better]
```

### 5. Save Review Output

Write the review to: `~/.claude/plans/[plan-name]-review.md`

If review file already exists, use AskUserQuestion:
```
Question: "A previous review exists. What would you like to do?"
Header: "Review"
Options:
  - "Start fresh" | "Overwrite with new review"
  - "View existing" | "Read the previous review first"
```

### 6. Present Results to User

Display summary of the review, then use AskUserQuestion:

**If assessment is APPROVE:**
```
Question: "Staff engineer approved the plan. Proceed?"
Header: "Action"
Options:
  - "Accept & continue (Recommended)" | "Plan approved. Next: /create-phases"
  - "View full review" | "See the complete review details"
  - "Request re-review" | "Have staff engineer look again"
```

**If assessment is CONCERNS or NEEDS WORK:**
```
Question: "Staff engineer has concerns. What would you like to do?"
Header: "Action"
Options:
  - "Iterate (Recommended)" | "Revise plan to address feedback, then re-review"
  - "Approve anyway" | "Proceed despite concerns"
  - "View full review" | "See detailed feedback before deciding"
  - "Cancel" | "Stop review process"
```

### 7. Handle User Response

**If "Accept" or "Approve anyway":**
- Output: "Plan approved. Next step: `/create-phases`"

**If "Iterate":**
- Go to Iteration Flow (below)

**If "View full review":**
- Read and display the full review file
- Return to the AskUserQuestion prompt

**If "Cancel":**
- Output: "Review cancelled. Plan unchanged."

---

## Iteration Flow

When user chooses "Iterate":

### Step 1: Spawn Revision Agent

Launch a **Plan agent** to revise the plan:

```
You are revising a technical plan based on staff engineer feedback.

## Original Plan:
$PLAN_CONTENT

## Staff Engineer Feedback:
$REVIEW_CONTENT

## Your Task:
Revise the plan to address the concerns and incorporate recommendations.
- Keep what works
- Fix what doesn't
- Add missing pieces flagged in the review
- Simplify anything flagged as over-engineered

Output the complete revised plan in the same format as the original.
At the top, add a revision note:

> **Revision $NEW_ITERATION** - Addressed feedback from staff engineer review
> Changes: [brief bullet list of what changed]
```

### Step 2: Update Plan File

Overwrite the original plan file with the revised content.

### Step 3: Re-run Review

- Increment iteration counter
- Go back to Step 4 (Spawn Staff Engineer Reviewer)
- The reviewer sees the updated plan

### Step 4: Present Again

After re-review completes, present results to user with AskUserQuestion (Step 6).

### Step 5: Multiple Iterations Warning

If iteration count reaches 3 or more, show warning via AskUserQuestion:
```
Question: "This is iteration $N. Persistent concerns may indicate scope issues."
Header: "Continue?"
Options:
  - "Keep iterating" | "Try one more revision"
  - "Approve current" | "Good enough, move forward"
  - "Pause and discuss" | "Save progress, talk to team"
```

---

## Edge Cases

| Scenario | Handling |
|----------|----------|
| No plan found | Error message with guidance to use plan mode first |
| Multiple plans | AskUserQuestion to select which one |
| Review already exists | AskUserQuestion: overwrite or view existing |
| Plan is very brief (<100 words) | AskUserQuestion: review anyway or expand first |
| 3+ iterations | Warning about scope issues, option to pause |
| Agent timeout | Inform user, offer to retry |
| CLAUDE.md not found | Proceed without conventions context, warn user |
| status.md not found | Proceed without workstream context |

---

## Example Session

```
> /review-plan

Found: ~/.claude/plans/auth-system.md
Gathering context...
  - Read CLAUDE.md (conventions)
  - Read status.md (workstreams)
  - Found 2 file references in plan
Starting staff engineer review...

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Staff Engineer Review Complete
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Assessment: CONCERNS

Top issues:
• Missing error handling for failed logins
• JWT expiration not specified
• No rate limiting mentioned

[AskUserQuestion prompt appears]

> User selects: "Iterate"

Revising plan based on feedback...
Re-reviewing...

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Staff Engineer Review (Iteration 2)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Assessment: APPROVE

The plan now addresses previous concerns.

[AskUserQuestion prompt appears]

> User selects: "Accept & continue"

Plan approved.
Next step: /create-phases
```

---

## Integration with Workflow

**Full workflow with review:**
```
Plan mode → /review-plan → (iterate?) → /create-phases → /task-planning-workflow → implement
```

| Step | Skill | Output |
|------|-------|--------|
| 1 | Plan mode | `~/.claude/plans/[name].md` |
| 2 | `/review-plan` | `~/.claude/plans/[name]-review.md` + approval |
| 3 | `/create-phases` | `external-memory/phases/*.md` |
| 4 | `/task-planning-workflow` | `external-memory/tasks/[domain]/phase-XX/*.md` |
| 5 | Implementation | Code changes |
| 6 | `/complete` | Updated tracking, commits |

---

## Staff Engineer Persona

The reviewer embodies these qualities:

**Mindset:**
- "What will break in production?"
- "What's the simplest solution that works?"
- "What will the next developer not understand?"

**Focus areas:**
- Architecture decisions and trade-offs
- Missing error handling / edge cases
- Unnecessary complexity
- Security considerations
- Testing strategy gaps
- Unclear requirements
- Convention violations

**Tone:**
- Direct but constructive
- Specific rather than vague ("add rate limiting to login endpoint" not "consider security")
- Actionable recommendations
