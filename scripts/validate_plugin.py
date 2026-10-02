#!/usr/bin/env python3
"""Check the marketplace/plugin schema subset and all 13 bundled workflows."""
import json
from pathlib import Path
import re
import subprocess
import sys


def validate(root):
    root = Path(root).resolve()
    catalog = json.loads((root / '.agents/plugins/marketplace.json').read_text(encoding='utf-8'))
    assert catalog['name'] == 'gumpnart-unity-character'
    assert isinstance(catalog['interface']['displayName'], str)
    assert len(catalog['plugins']) == 1
    entry = catalog['plugins'][0]
    name = entry['name']
    assert re.fullmatch(r'[a-z0-9]+(?:-[a-z0-9]+)*', name) and len(name) <= 64
    assert entry['source'] == {'source': 'local', 'path': './plugins/' + name}
    assert entry['policy'] == {'installation': 'AVAILABLE', 'authentication': 'ON_INSTALL'}
    assert entry['category'] == 'Developer Tools'
    plugin = root / 'plugins' / name
    manifest = json.loads((plugin / '.codex-plugin/plugin.json').read_text(encoding='utf-8'))
    assert manifest['name'] == plugin.name == name
    assert re.fullmatch(r'\d+\.\d+\.\d+', manifest['version'])
    assert manifest['skills'] == './skills/'
    assert manifest['description'] and manifest['author']['name'] == 'gumpnart'
    assert not any(k in manifest for k in ('hooks', 'mcpServers', 'apps')), 'Only implemented surfaces should be declared.'
    interface = manifest['interface']
    assert interface['displayName'] and interface['developerName'] == 'gumpnart'
    assert interface['category'] == entry['category']
    assert set(interface['capabilities']) == {'Interactive', 'Read', 'Write'}
    assert re.fullmatch(r'#[0-9A-Fa-f]{6}', interface['brandColor'])
    assert 1 <= len(interface['defaultPrompt']) <= 3
    assert all(isinstance(p, str) and len(p) <= 128 for p in interface['defaultPrompt'])
    for key in ('composerIcon', 'logo'):
        if key in interface:
            relative = interface[key]
            assert relative.startswith('./assets/') and (plugin / relative).is_file(), key
    for path in interface.get('screenshots', []):
        assert path.startswith('./assets/') and path.endswith('.png') and (plugin / path).is_file()
    assert '[TODO:' not in json.dumps(manifest) + json.dumps(catalog)
    result = subprocess.run([sys.executable, str(plugin / 'scripts/validate_pack.py')], capture_output=True, text=True)
    assert result.returncode == 0, result.stdout + result.stderr
    pack_result = json.loads(result.stdout)
    assert pack_result['skills'] == 13
    links = 0
    for path in root.rglob('*.md'):
        if 'dist' in path.relative_to(root).parts:
            continue
        for target in re.findall(r'\[[^\]]+\]\(([^)]+)\)', path.read_text(encoding='utf-8')):
            if '://' in target or target.startswith('#'):
                continue
            assert (path.parent / target.split('#')[0]).exists(), (path, target)
            links += 1
    for folder in (root / 'scripts', plugin / 'scripts'):
        for path in folder.glob('*.py'):
            compile(path.read_text(encoding='utf-8'), str(path), 'exec')
    return {'plugin': name, 'version': manifest['version'], 'skills': 13, 'templates': 5,
            'local_links': links, 'status': 'passed'}


if __name__ == '__main__':
    print(json.dumps(validate(Path(__file__).resolve().parents[1])))
