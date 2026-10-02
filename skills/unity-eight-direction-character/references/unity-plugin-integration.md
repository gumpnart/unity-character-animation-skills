# Required Unity plugin integration

The Unity plugin is a required dependency for this skill. Every invocation must
make an actual plugin call before character/animation work, including requests
for planning or reviewing an action. Installed documentation alone is insufficient.

The skill ZIP contains this integration policy and a readiness helper; the official
Unity plugin, CLI, editor, and Pipeline connection are separately installed. A
SKILL.md instruction cannot install or embed an external plugin automatically.

## Plugin skills to use

| Task | Installed Unity plugin skill |
|---|---|
| Every invocation: connect, inspect, author, save, and verify | `unity:unity-cli` / local `unity-cli` |
| Create a project only when the user's task needs a new one | `unity:new-unity-project` |
| Find/install required UPM packages | `unity:unity-package-management` |

Load the installed versions instead of assuming their APIs or flags. This character
skill orchestrates those plugin capabilities; it does not copy their full skill
trees or pretend to replace their tools.

## First calls on every invocation

Use the project's absolute path and read the Unity plugin's CLI instructions.
If Unity MCP tools are exposed, discover and call real status/discovery tools;
otherwise use the plugin's CLI bridge. From the project cwd:

```bash
unity status --format json
unity command --caller plugin --skill unity-eight-direction-character --project-path "/absolute/path/to/project" --format json
```

Alternatively execute the bundled helper (replace the skill path if installed
globally):

```bash
python3 ".agents/skills/unity-eight-direction-character/scripts/check-unity-plugin.py" --project-path "/absolute/path/to/project"
```

The helper runs Unity itself. It verifies a real Unity project, an available CLI,
a fresh status response, and successful live command discovery for the explicit
project. It prints an actionable blocker on failure; it does not install software,
edit project settings, or start an editor.

Every `unity command` invocation needs `--caller plugin --skill
unity-eight-direction-character`. Always target the project explicitly rather
than letting multiple running editors be selected by accident.

GUI editors normally appear in status with state ready. A persistent headless
editor may serve commands without appearing in status. Confirm it with explicit
command discovery, not the status list alone. A stale CLI registration is also
insufficient: discovery must reach the target editor.

## One-time setup on the user's machine

Use the installed Unity plugin's current setup instructions. The documented CLI
workflow supports Unity 6.0+ with the project's `com.unity.pipeline` package.
Do not silently upgrade an older project to satisfy that requirement; report the
version mismatch and use an existing compatible Unity plugin tool route if one
is verified.

With the Unity CLI installed, from the Unity project directory:

```bash
unity --version
unity pipeline install --project-path "/absolute/path/to/project"
unity skill install codex --local
unity mcp configure codex --local --project-path "/absolute/path/to/project"
unity open "/absolute/path/to/project"
```

These setup commands change the project/client configuration or launch the editor;
they are setup instructions, not steps to rerun on every invocation. Apply them
when required and within the user's authorized setup scope. Restart Codex after
client configuration changes. Pipeline installation is the plugin's supported
bridge installer; ordinary UPM packages use Package Manager as described below.

`unity mcp configure codex` configures the Unity tool server, while
`unity skill install codex` installs the CLI documentation. They serve different
purposes. If the connected editor exposes tools directly through the installed
Unity plugin already, use that verified route instead of adding redundant config.

The CLI's `unity plugin install` command manages CLI-adjacent tools such as UVCS
and UGS; it is not the installation command for the Codex Unity plugin.

## Work through the connected editor

1. Discover the actual command list; confirm active scene and target prefab.
2. Inspect the physical bone hierarchy, library/resolvers, existing clips,
   controller, and relevant item assets through Unity plugin editor tooling.
3. Create/update GameObjects, prefabs, profiles, action assets, clips, and states
   with plugin commands or plugin-driven C# editor execution. For example, after
   confirming the editor exposes eval:

   ```bash
   unity command eval --caller plugin --skill unity-eight-direction-character --project-path "/absolute/path/to/project" "return UnityEngine.Application.unityVersion;"
   ```

   Execute the package's builder/editor APIs through that same verified route.
   Do not merely write a generation script and claim its assets were created.
4. Runtime/editor C# source files can be edited normally when the implementation
   needs them. Allow Unity to import/compile them; resolve compile errors through
   the plugin recovery workflow. Generated assets and metadata belong to Unity.
5. Save, inspect clip/controller bindings and markers, enter the test scene/play
   workflow, and collect compile/visual evidence through available plugin tools.
   Do not assume screenshots or playback tools exist; discover actual capabilities.

Never hand-edit `.unity`, `.prefab`, `.asset`, `.anim`, or Sprite Library serialized
content to bypass an editor connection. Keep `.meta` files generated by Unity.
The same rig, pixel-art, action timing, and equipment constraints still apply.

## Packages

Use `unity:unity-package-management` for `com.unity.2d.animation` and
`com.unity.2d.psdimporter`. Inspect installed versions first and announce the
minimal required package list. Use Package Manager or its asynchronous C# Client
API; do not hand-edit Packages/manifest.json. Do not use a synchronous busy-wait
or a `unity run` process that quits before an asynchronous install completes.
Follow the installed plugin skill's live/headless polling instructions.

## Failure and recovery

- Missing Unity plugin/tools: identify the missing dependency and give installation
  instructions. Do not claim a CLI helper alone installed the plugin.
- Missing project: identify the required project path. For an explicitly requested
  new project, use the Unity project-creation plugin workflow, then reconnect.
- No editor: use authorized plugin opening/bootstrap operations, then retry.
- Status empty but an editor is expected: check headless discovery, Pipeline
  presence, Safe Mode, and sandbox visibility before declaring it absent.
- Safe Mode: use `unity pipeline list --format json` and editor logs/diagnostics;
  fixing C# compile errors and reopening the editor is an appropriate recovery.
- Sandbox visibility: follow the installed Unity CLI skill's recovery guidance;
  request only the access actually required by the tool/environment.
- Wrong/multiple project: target the verified path explicitly; do not guess.

If the verified plugin path cannot be made ready, **stop character/animation
implementation** and return the concrete blocker and next setup step. Setup
instructions and diagnostic explanations are allowed; standalone generated code,
asset specifications, or raw-file scene creation are not a substitute.

Completion reports name the real plugin route, target project, readiness result,
operations performed, and actual verification. Cached outcomes, printed examples,
and merely reading plugin documentation do not satisfy the per-invocation call.
