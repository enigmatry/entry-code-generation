# Agent Configuration

This directory contains configuration for AI coding agents (e.g. GitHub Copilot coding agent).

## MCP Servers

The repository `.mcp.json` includes the **Azure DevOps** MCP server, which is project-specific and pinned to a specific version for reproducibility.

### Optional integrations

The following MCP servers are **not included by default** because they require individual developer authentication and call out to third-party services. Add them to your own [user-level MCP settings](https://code.visualstudio.com/docs/copilot/chat/mcp-servers) if you need them:

**Atlassian (Jira / Confluence):**
```json
{
  "atlassian": {
    "type": "stdio",
    "command": "npx",
    "args": ["-y", "mcp-remote", "https://mcp.atlassian.com/v1/sse"]
  }
}
```

**Context7 (library documentation):**
```json
{
  "Context7": {
    "type": "http",
    "url": "https://mcp.context7.com/mcp/oauth",
    "tools": ["query-docs", "resolve-library-id"]
  }
}
```
