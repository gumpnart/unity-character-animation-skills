#!/usr/bin/env python3
"""Validate pack structure, metadata, references and mirrored templates."""
from pathlib import Path
import json
import re

def main():
    root = Path(__file__).resolve().parents[1]
    expected = {
        'master-character', 'direction-master', 'rig-ready-parts', 'unity-asset-import',
        'unity-skeleton-rig', 'joint-skinning-validation', 'animation-clip-authoring',
        'directional-animation-system', 'modular-equipment-system', 'animator-controller',
        'runtime-character-controller', 'character-production-qa', 'character-production-orchestrator',
    }
    dirs = {p.name for p in (root / 'skills').iterdir() if p.is_dir()}
    assert dirs == expected, ('Unexpected/missing skills', dirs ^ expected)
    for name in sorted(expected):
        path = root / 'skills' / name / 'SKILL.md'
        text = path.read_text(encoding='utf-8')
        match = re.match(r'^---\nname: ([a-z0-9-]+)\ndescription: (.+)\n---\n', text)
        assert match and match[1] == name, path
        assert isinstance(json.loads(match[2]), str) and len(json.loads(match[2])) > 40, path
        assert '--caller plugin --skill ' + name in text, path
        assert 'On EVERY invocation' in text and 'stop dependent production' in text, path
        assert 'CHARACTER_SPEC.md' in text and 'QA_CHECKLIST.md' in text, path
        # Resolve shared reference paths for both repository and installed sibling layout.
        base = path.parent
        for relative in ('../character-production-orchestrator/references/production-contract.md',
                         '../character-production-orchestrator/references/stage-map.md'):
            assert (base / relative).is_file(), (path, relative)
    templates = {'CHARACTER_SPEC.template.md', 'DIRECTION_SPEC.template.md',
                 'ANIMATION_SPEC.template.md', 'EQUIPMENT_SPEC.template.md', 'QA_CHECKLIST.md'}
    assert {p.name for p in (root / 'character-production').glob('*.md')} == templates
    embedded = root / 'skills/character-production-orchestrator/assets/character-production'
    for name in templates:
        assert (root / 'character-production' / name).read_bytes() == (embedded / name).read_bytes(), name
    links = 0
    for path in root.rglob('*.md'):
        text = path.read_text(encoding='utf-8')
        for target in re.findall(r'\[[^\]]+\]\(([^)]+)\)', text):
            if '://' in target or target.startswith('#'):
                continue
            assert (path.parent / target.split('#')[0]).exists(), (path, target)
            links += 1
    assert not (root / 'skills/unity-eight-direction-character').exists()
    print(json.dumps({'skills': len(expected), 'templates': len(templates), 'local_links': links, 'status': 'passed'}))

if __name__ == '__main__':
    main()
