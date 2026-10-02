#!/usr/bin/env python3
"""Read-only live Unity CLI handshake. Uses the official plugin's CLI bridge."""

import argparse
import json
from pathlib import Path
import shutil
import subprocess
import sys


def fail(message, next_step):
    print(json.dumps({"ready": False, "blocker": message, "next_step": next_step}, indent=2))
    return 1


def call_unity(binary, arguments, project):
    try:
        result = subprocess.run(
            [binary, *arguments, "--format", "json", "--non-interactive", "--no-pager", "--no-banner"],
            cwd=str(project), text=True, capture_output=True, timeout=30,
        )
    except subprocess.TimeoutExpired:
        return None, None, "Unity call timed out after 30 seconds."
    except OSError as error:
        return None, None, str(error)
    try:
        payload = json.loads(result.stdout)
    except json.JSONDecodeError:
        payload = None
    diagnostic = (result.stderr or result.stdout).strip()[:1200]
    return result.returncode, payload, diagnostic


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--project-path", required=True, help="Target Unity project directory")
    args = parser.parse_args()
    project = Path(args.project_path).expanduser().resolve()
    if not (project / "ProjectSettings" / "ProjectVersion.txt").is_file():
        return fail("Target is not a recognized Unity project: " + str(project),
                    "Supply the actual project path, or create the requested project through the Unity plugin.")
    binary = shutil.which("unity")
    if binary is None:
        return fail("Unity CLI is unavailable in this Codex shell.",
                    "Load the Unity plugin's unity-cli skill and complete its CLI/connection setup; do not generate assets offline.")

    # Fresh calls on every run. Status alone can omit resident headless editors.
    status_code, status, status_error = call_unity(binary, ["status"], project)
    discovery_code, discovery, discovery_error = call_unity(binary, [
        "command", "--caller", "plugin", "--skill", "unity-eight-direction-character",
        "--project-path", str(project),
    ], project)
    if discovery_code != 0 or not isinstance(discovery, dict) or discovery.get("success") is not True:
        message = "Live Unity command discovery did not succeed for " + str(project)
        if isinstance(discovery, dict) and discovery.get("errors"):
            message += ": " + json.dumps(discovery["errors"])[:1200]
        elif discovery_error:
            message += ": " + discovery_error
        return fail(message,
                    "Verify the Unity plugin, Pipeline/editor readiness, project targeting, Safe Mode, and sandbox visibility; then retry.")

    print(json.dumps({
        "ready": True,
        "route": "Unity plugin CLI bridge",
        "project": str(project),
        "calls": ["unity status", "unity command (live discovery, explicit project, plugin/skill labels)"],
        "status_exit_code": status_code,
        "status_reported_success": isinstance(status, dict) and status.get("success") is True,
        "discovery_exit_code": discovery_code,
        "note": "Live command discovery verified the target. Continue with the installed Unity plugin workflow.",
    }, indent=2))
    return 0


if __name__ == "__main__":
    sys.exit(main())
