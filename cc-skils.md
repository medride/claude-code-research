# Claude Skills - Comprehensive Guide

## What Are Skills?

Skills teach Claude to perform tasks repeatedly through specialized folders containing instructions, scripts, and resources that load dynamically when relevant.

**How They Work:** Skills use progressive disclosure:
- Metadata (~100 tokens) loads first for relevance matching
- Full instructions (<5k tokens) load when applicable
- Resources load only as needed

> **Key Insight:** "If you find yourself typing the same prompt repeatedly across multiple conversations, it's time to create a Skill."

---

## How to Use Skills

Skills are invoked by typing a forward slash followed by the skill name:

```
/skill-name
```

You can also pass arguments to skills:

```
/skill-name argument
```

---

## Official Skills (Anthropic-Maintained)

### Document Skills

| Skill | Description |
|-------|-------------|
| `/docx` | Create, edit, and analyze Word documents with support for tracked changes and comments |
| `/pdf` | Comprehensive PDF manipulation toolkit for extracting text and tables |
| `/pptx` | Create, edit, and analyze PowerPoint presentations |
| `/xlsx` | Create, edit, and analyze Excel spreadsheets with support for formulas |

### Design & Creative

| Skill | Description |
|-------|-------------|
| `/algorithmic-art` | Generative art using p5.js with randomness and particle systems |
| `/canvas-design` | Visual art creation in PNG and PDF formats |
| `/slack-gif-creator` | Animated GIFs optimized for Slack constraints |

### Development

| Skill | Description |
|-------|-------------|
| `/frontend-design` | Avoids generic aesthetics; works with React and Tailwind |
| `/artifacts-builder` | Complex HTML artifacts using React, Tailwind, and shadcn/ui |
| `/mcp-builder` | Guide for creating high-quality MCP servers to integrate external APIs |
| `/webapp-testing` | Browser automation using Playwright for UI verification |

### Communication

| Skill | Description |
|-------|-------------|
| `/brand-guidelines` | Apply Anthropic's official brand colors and typography |
| `/internal-comms` | Write status reports, newsletters, FAQs |

### Skill Creation

| Skill | Description |
|-------|-------------|
| `/skill-creator` | Interactive skill creation tool that guides you through building new skills |

### Common CLI Skills

| Skill | Description |
|-------|-------------|
| `/commit` | Creates a git commit with an AI-generated commit message based on staged changes |
| `/review-pr` | Reviews a pull request for code quality, bugs, and improvements |
| `/help` | Displays help information about Claude Code |
| `/clear` | Clears the current conversation context |

---

## Community Skills

### Major Collections

#### obra/superpowers
20+ battle-tested skills including TDD, debugging, and collaboration.

**Install:**
```
/plugin marketplace add obra/superpowers-marketplace
```

#### obra/superpowers-lab
Experimental skills using refined techniques from the superpowers collection.

### Individual Community Skills

| Skill | Purpose |
|-------|---------|
| `ios-simulator-skill` | iOS app building and testing automation |
| `ffuf-web-fuzzing` | Web fuzzing guidance for penetration testing |
| `playwright-skill` | General-purpose browser automation |
| `claude-d3js-skill` | Data visualizations in d3.js |
| `claude-scientific-skills` | Scientific computing and database work |
| `web-asset-generator` | Favicons, app icons, social media images |
| `loki-mode` | Multi-agent autonomous startup orchestration |

### Tools for Creating Skills

**Skill_Seekers** - Converts documentation websites into Claude Skills automatically.

---

## Installation Instructions

### Claude.ai Web Interface

1. Go to **Settings > Capabilities**
2. Enable the **Skills toggle**
3. Browse or upload custom skills
4. For Team/Enterprise: Admin must enable organization-wide first

### Claude Code CLI

```bash
# Add from marketplace
/plugin marketplace add anthropics/skills

# Add from local directory
/plugin add /path/to/skill-directory
```

### Claude API

Access via `/v1/skills` endpoint with appropriate API key.

---

## Creating Your Own Skills

### Method 1: Automated (Recommended)

1. Enable `skill-creator` in Claude
2. Request: "Use skill-creator to help me build a skill for [task]"
3. Answer interactive questions
4. Claude generates complete structure

### Method 2: Manual Creation

**Folder Structure:**
```
my-skill/
├── SKILL.md          # Frontmatter + instructions (required)
├── scripts/          # Optional executables
└── resources/        # Optional supporting files
```

**SKILL.md Template:**
```yaml
---
name: skill-name
description: Brief discovery description
---

# Detailed Instructions

[Your detailed instructions here]

# Usage

[How to use the skill]

# Examples

[Specific examples]
```

### Best Practices for Skill Creation

- Keep descriptions concise for discovery
- Write clear, actionable instructions
- Include specific examples
- Use git tags for versioning
- Document dependencies thoroughly
- Test in non-production environments first

---

## Skills vs. Other Approaches

| Tool | Best For |
|------|----------|
| **Skills** | Reusable procedural knowledge across conversations |
| **Prompts** | One-time instructions and immediate context |
| **Projects** | Persistent workspace background knowledge |
| **Subagents** | Independent task execution with permissions |
| **MCP** | External data source integration |

---

## Security & Best Practices

> ⚠️ **Warning**: Skills can execute arbitrary code. Only install from trusted sources.

### Vetting Requirements

- Review `SKILL.md` and all scripts before enabling
- Be cautious of sensitive data access requests
- Audit carefully before production deployment

### Security Concerns

- Malicious skills may enable data exfiltration
- Prompt injection attacks amplified through compromised skills
- Sandboxing limitations require understanding before enterprise use

### Team Deployment

- Use version control and internal repositories
- Establish approval policies
- Monitor usage and performance

---

## Troubleshooting

### Skills Not Appearing

- Verify **Settings > Capabilities** has Skills enabled
- For Team/Enterprise: Confirm admin enabled organization-wide
- Restart Claude after installation

### Skills Not Loading

- Verify proper YAML frontmatter formatting
- Check `name` and `description` fields are present
- Ensure file structure matches specifications

### Execution Failures

- Confirm script dependencies are installed
- Review error logs
- Test scripts independently outside Claude

---

## Resources

### Official Documentation

- [What are Skills?](https://support.claude.com/articles/12512176-what-are-skills)
- [Using Skills in Claude](https://support.claude.com/articles/12512180-using-skills-in-claude)
- [Skills API Documentation](https://platform.claude.com/docs/en/api/beta/skills)

### Community Resources

- [Awesome Claude Skills Repository](https://github.com/travisvn/awesome-claude-skills)
- [obra/superpowers Collection](https://github.com/obra/superpowers-marketplace)

---

*Last updated: January 2026*
