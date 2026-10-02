#!/usr/bin/env python3
"""Print local Codex plugin View/Share links; optionally open the selected view."""
import argparse
import json
import os
from pathlib import Path
import shutil
import subprocess
import sys
from urllib.parse import quote, urlencode


def plugin_links(root):
    root = Path(root).resolve()
    marketplace = root / '.agents/plugins/marketplace.json'
    catalog = json.loads(marketplace.read_text(encoding='utf-8'))
    entries = catalog['plugins']
    if len(entries) != 1:
        raise ValueError('Expected this bundle to contain one plugin entry.')
    entry = entries[0]
    source = entry['source']
    if source['source'] != 'local':
        raise ValueError('Expected a local marketplace source.')
    plugin = (root / source['path']).resolve()
    if not plugin.is_relative_to(root):
        raise ValueError('Plugin source must stay inside this repository.')
    manifest = json.loads((plugin / '.codex-plugin/plugin.json').read_text(encoding='utf-8'))
    if manifest['name'] != entry['name'] or plugin.name != manifest['name']:
        raise ValueError('Plugin manifest, folder and marketplace names do not match.')
    skills = plugin / manifest['skills']
    if len(list(skills.glob('*/SKILL.md'))) != 13:
        raise ValueError('Plugin must contain all 13 skills.')
    view = 'codex://plugins/' + quote(manifest['name'], safe='') + '?' + urlencode({'marketplacePath': str(marketplace)})
    return view, view + '&mode=share'


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--open', action='store_true', help='Open in Codex Desktop using the OS URL handler.')
    parser.add_argument('--share', action='store_true', help='Select Share instead of View when using --open.')
    args = parser.parse_args()
    try:
        view, share = plugin_links(Path(__file__).resolve().parents[1])
    except (OSError, KeyError, ValueError) as error:
        parser.exit(1, 'Invalid or incomplete plugin package: ' + str(error) + '\n')
    print('View: ' + view)
    print('Share: ' + share)
    print('Open the View link on this computer, then install/enable the plugin in Codex Desktop.')
    if args.open:
        uri = share if args.share else view
        try:
            if sys.platform == 'darwin':
                subprocess.run(['open', uri], check=True)
            elif os.name == 'nt':
                os.startfile(uri)
            else:
                opener = shutil.which('xdg-open')
                if not opener:
                    parser.exit(1, 'No URL opener found. Open the printed link manually in Codex Desktop.\n')
                subprocess.run([opener, uri], check=True)
        except (OSError, subprocess.CalledProcessError) as error:
            parser.exit(1, 'Could not open Codex: ' + str(error) + '. Use the printed link manually.\n')


if __name__ == '__main__':
    main()
