#!/usr/bin/env python3
"""Install this pack; preserve canonical specs and unrelated project contents."""
import argparse
from pathlib import Path
import shutil

def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--project', required=True, type=Path)
    parser.add_argument('--replace-skills', action='store_true')
    parser.add_argument('--remove-legacy', action='store_true')
    args = parser.parse_args()
    source = Path(__file__).resolve().parents[1]
    project = args.project.expanduser().resolve()
    if not all((project / name).is_dir() for name in ('Assets', 'Packages', 'ProjectSettings')):
        parser.error('Target must be an existing Unity project with Assets, Packages and ProjectSettings.')
    destinations = project / '.agents' / 'skills'
    spec_dir = project / 'character-production'
    specs = list((source / 'character-production').glob('*.md'))
    skills = sorted((source / 'skills').iterdir())
    if len(skills) != 13 or any(not (s / 'SKILL.md').is_file() for s in skills):
        parser.error('Source pack must contain exactly 13 complete skill directories.')
    existing = [s.name for s in skills if (destinations / s.name).exists()]
    if existing and not args.replace_skills:
        parser.error('Existing pack skills: ' + ', '.join(existing) + '. Use --replace-skills to update them; specs stay preserved.')
    # Prevent replacement through a symlink or copying into the pack itself.
    targets = [project / '.agents', destinations, spec_dir] + [destinations / s.name for s in skills]
    targets.append(destinations / 'unity-eight-direction-character')
    if any(p.is_symlink() for p in targets):
        parser.error('Installation targets must not be symlinks.')
    if source == project or source in project.parents:
        parser.error('Install into a separate Unity project, not inside the pack source.')
    destinations.mkdir(parents=True, exist_ok=True)
    spec_dir.mkdir(parents=True, exist_ok=True)
    for skill in skills:
        target = destinations / skill.name
        if target.exists():
            if not target.is_dir():
                parser.error('Skill target is not a directory: ' + str(target))
            shutil.rmtree(target)
        shutil.copytree(skill, target, ignore=shutil.ignore_patterns('__pycache__', '*.pyc'))
    created = []
    for template in specs:
        target = spec_dir / template.name.replace('.template.md', '.md')
        if not target.exists():
            shutil.copy2(template, target)
            created.append(target.name)
    legacy = destinations / 'unity-eight-direction-character'
    if args.remove_legacy and legacy.exists():
        if not legacy.is_dir():
            parser.error('Legacy skill target is not a directory.')
        shutil.rmtree(legacy)
    print('Installed 13 skills in ' + str(destinations))
    print('Created specs: ' + (', '.join(created) or 'none; existing specs preserved'))

if __name__ == '__main__':
    main()
