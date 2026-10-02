# Official Unity plugin requirement

Install the official Unity plugin in Codex separately. This pack contributes instructions; it cannot silently install plugins, activate account connections, or guarantee tool availability. Every one of the 13 skills requires a fresh actual plugin call, even artwork stages and reviews, as requested by this pack's workflow.

Open the target Unity project and use the installed `unity:unity-cli` instructions. The documented CLI bridge supports Unity 6.0+ with `com.unity.pipeline`. Reuse compatible projects; migrating an older project needs an explicit project decision.

```bash
unity --version
unity pipeline install --project-path "/absolute/path/MyUnityGame"
unity skill install codex --local
unity mcp configure codex --local --project-path "/absolute/path/MyUnityGame"
unity status --format json
unity command --caller plugin --skill master-character --project-path "/absolute/path/MyUnityGame" --format json
```

Follow live CLI help and installed plugin instructions if the installed version differs. Pipeline install/configuration is setup, not something to run automatically on every skill use. `unity skill install` mirrors official skill documentation; it does not install the Codex Unity plugin. `unity plugin install` manages CLI-adjacent tools and is not the Codex plugin installer.

For every invocation use that invocation's actual skill name on each `unity command`, and explicitly target the real project. Discover available commands before executing; don't assume an editor supports `eval` or a particular command name. Equivalent Unity MCP readiness/discovery calls are acceptable after discovering actual tool names.

Success requires a valid live response for the selected Editor; checking CLI version or reading documentation alone is insufficient. Status may omit a headless Editor. Diagnose Safe Mode and sandbox visibility using the official recovery instructions. If unavailable, report the blocker and stop dependent production rather than silently hand-editing asset YAML or giving offline implementation as completion.

Readiness must be rechecked at each stage skill invocation. Never claim Unity compilation, image generation, rendered validation or Play mode tests were performed if only pack files were edited. Editing/distributing this skill pack does not invoke the production skills against a Unity game.
