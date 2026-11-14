---
uid: fb49ebbd-b124-4dd0-9b3c-bf79e21ccfa5
title: Modeling with AI Agents (MCP)
---
Macad\|3D contains a server for the [Model Context Protocol](#see-also) (MCP). Through it, an AI agent like Claude, ChatGPT/Codex, GitHub Copilot or Gemini can use the modeling tools of Macad\|3D: it creates bodies and sketches, applies modifiers, measures the result and looks at it. You describe what you want in plain words, the agent works out the steps, and you watch the model grow in the viewport, where you can change it at any time.

## What an Agent Can Do

The agent works on the document currently open in Macad\|3D, with the same building blocks you use yourself: Everything the agent creates is a normal, parametric Macad\|3D model. The shape stack shows each step, and you can edit the parameters in the property panel like those of any other body.

The following model was built with the MCP tools only, starting from the request:

> Build a 1:64 scale model of a Victorian ship cannon.

^![The resulting model of the cannon](McpServerCannon.png)

> [!TIP]
> Give the agent the information you would give a person: the purpose of the model, the important dimensions and the scale. Ask it to check its work with an image, and to explain what it did. You can also select a body in Macad\|3D and refer to "the selected body" in your request.

### Configuring the Agent

To make Macad\|3D available, add an MCP server of the type __stdio__ (local command) to the configuration of your agent:

Setting | Value
--------|------
Name | `macad`, or any name you like
Type | `stdio` (local command)
Command | The full path of `Macad.McpProxy.exe`, e.g. `C:\Program Files\Macad3D\Macad.McpProxy.exe`
Arguments | None

Restart the agent or reload its MCP servers afterwards. The tools of Macad\|3D are listed by the agent at once, even if Macad\|3D is not running yet.

Most agents use a JSON file for their configuration. Note that backslashes in paths must be doubled in JSON. Here are examples for some common agents. The location and format of their configuration may change with new versions, check the documentation of your agent if the examples do not work.

# [Claude Desktop](#tab/claude-desktop)

Open __Settings__ > __Developer__ > __Edit Config__, and add the server to `claude_desktop_config.json`:

```json
{
  "mcpServers": {
    "macad": {
      "command": "C:\\Program Files\\Macad3D\\Macad.McpProxy.exe"
    }
  }
}
```

# [Claude Code](#tab/claude-code)

Add the server with the command line, for all your projects:

```
claude mcp add --scope user macad -- "C:\Program Files\Macad3D\Macad.McpProxy.exe"
```

# [VS Code (GitHub Copilot)](#tab/vscode)

Run the command __MCP: Add Server__, or add the server to the file `.vscode/mcp.json` of your workspace:

```json
{
  "servers": {
    "macad": {
      "type": "stdio",
      "command": "C:\\Program Files\\Macad3D\\Macad.McpProxy.exe"
    }
  }
}
```

The tools are available in the agent mode of the Copilot chat.

# [Cursor](#tab/cursor)

Add the server to `%USERPROFILE%\.cursor\mcp.json`:

```json
{
  "mcpServers": {
    "macad": {
      "command": "C:\\Program Files\\Macad3D\\Macad.McpProxy.exe"
    }
  }
}
```

# [Codex](#tab/codex)

Add the server to `%USERPROFILE%\.codex\config.toml`:

```toml
[mcp_servers.macad]
command = 'C:\Program Files\Macad3D\Macad.McpProxy.exe'
```

# [Gemini CLI](#tab/gemini)

Add the server to `%USERPROFILE%\.gemini\settings.json`:

```json
{
  "mcpServers": {
    "macad": {
      "command": "C:\\Program Files\\Macad3D\\Macad.McpProxy.exe"
    }
  }
}
```

---

### Starting and Stopping the Server

* If Macad\|3D is not running when the agent uses the first tool, the proxy starts it with the MCP server enabled. This takes a few seconds.
* If Macad\|3D is already running, the server must be switched on: click __MCP Server__ on the ribbon tab __Toolbox__. The button shows whether the server is running. Click it again to stop the server, which disconnects all agents. The agent then gets a message that asks you to start the server again; after that, it can continue without being restarted.
* Only one Macad\|3D instance can run the server at a time. If you start it in a second instance, you are asked whether the server should be moved from the other instance. The connected agents follow automatically.

## Limits

* **Local only:** The server can only be reached from the same computer and the same Windows user. Agents running in the cloud without a local component, like a chat in a web browser, cannot use it.
* **No file handling:** The agent can start a new document and export bodies to STEP or STL, but it cannot open, save or import files. Save your work yourself.
* **No interaction with the user interface:** The agent cannot click into the viewport or use the interactive tools. It can read the current selection, but not change it. Drawings, slice contours and scripts are not available.
* **Faces and edges are numbered:** The agent refers to faces and edges by their index in the current shape. After a modification, the indices may change, so the agent has to list them again. Usually it does that by itself, but it may pick a wrong edge for a fillet or chamfer now and then. Check the result.
* **Images:** The agent can only look at the model if it supports images in tool results. Hidden bodies are not shown in the image.
* **Results depend on the agent:** How good the model gets depends on the AI model used, and on your request. Agents make mistakes, especially with complex sketches and with the orientation of parts. Always check what the agent built, before you use the model for production.
* **Undo:** Each tool call of the agent is one undo step. If the agent goes wrong, you can undo its steps in Macad\|3D, or ask the agent to undo them. A tool call that fails does not change the model.
* **Orientation:** Z is always up. Which side of the model is the front is set in the preferences (__Forward Direction__ of the viewport). The agent only learns this setting when it captures an image, so words like "front" or "left" in your request can be misunderstood. Give directions as axes, like "along +X", if they are important.

## See Also

* [Model Context Protocol](https://modelcontextprotocol.io) - Specification and list of clients.
* [Running Scripts](xref:0857be7d-eabe-40e1-bda3-a3415b29a4cd) - Automate Macad\|3D with C# scripts.
