---
description: Git commit standards
globs:
---

When making git commits, use conventional commit format:
```
[TICKET] <type>(<scope>): <description>

[optional body]

Co-Authored-By: Claude <claude_model> <noreply@anthropic.com>
```

The `[TICKET]` prefix is optional. When provided (e.g., a Jira ticket), prepend it to the subject line.

Types: feat, fix, docs, style, refactor, test, chore, build, ci, perf

Examples:
- `feat(auth): add Google OAuth sign-in`
- `fix(api): handle null response from user endpoint`
- `PROJ-123 feat(frontend): complete Phase 5 — Dark Mode Toggle`
- `PROJ-456 chore(frontend): session handoff — wired search toolbar`

Keep the subject line under 72 characters. Use imperative mood ("add" not "added").
