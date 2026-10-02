# ติดตั้ง Unity 2.5D Character ใน Codex Desktop

Plugin: `unity-2-5d-character` · Version: `1.0.0` · รวมทั้ง 13 skills

## 1. ดาวน์โหลดไว้บนเครื่องที่ใช้ Codex

Clone repo ไว้ในโฟลเดอร์ถาวรบนเครื่องที่ใช้ Codex Desktop อย่าย้ายหรือลบโฟลเดอร์ระหว่างที่ยังใช้ local marketplace นี้อยู่

```bash
git clone https://github.com/gumpnart/unity-character-animation-skills.git
cd unity-character-animation-skills
```

โครงสร้างสำคัญคือ `.agents/plugins/marketplace.json` และ `plugins/unity-2-5d-character/.codex-plugin/plugin.json` ซึ่งชี้ถึง 13 skills ภายใน plugin Clone ให้ครบ รวมไฟล์และโฟลเดอร์ที่ขึ้นต้นด้วยจุด

## 2. เปิดหน้าติดตั้ง plugin

ใช้ Python 3 บนเครื่องของคุณ:

```bash
python scripts/open_plugin.py
```

บน macOS/Linux หากคำสั่งชื่อ `python3` ให้ใช้ `python3` แทน สคริปต์จะแสดง View และ Share link ที่ใส่ path จริงของ marketplace บนเครื่องคุณแล้ว เปิด View link ด้วย Codex Desktop หรือใช้:

```bash
python scripts/open_plugin.py --open
```

ในหน้ารายละเอียด plugin ให้กด Install และเปิดใช้งาน หากแอปให้เลือกว่าจะเปิดลิงก์นี้ด้วยโปรแกรมใด ให้เลือก Codex สคริปต์นี้เปิดหน้ารายละเอียดเท่านั้น; มันไม่ได้ยืนยันว่าติดตั้งหรือเปิดใช้สำเร็จแล้ว

ถ้า Codex ไม่รับลิงก์ ให้ตรวจว่าติดตั้ง Codex Desktop รุ่นที่รองรับ plugins, โฟลเดอร์ repo ยังอยู่ และพาธในลิงก์เป็นพาธบนเครื่องเดียวกันกับแอป ลิงก์ที่มี `/workspace/...` ของ cloud session จะไม่ชี้ถึงไฟล์บน desktop ของคุณ ให้สร้างลิงก์ด้วยสคริปต์บนเครื่องแทน

## 3. เชื่อมต่อ Unity

ติดตั้ง official Unity plugin ใน Codex แยกต่างหาก และเปิด Unity Editor ของเกมเป้าหมาย ทั้ง 13 skills จะเรียก plugin นี้ใหม่ทุกครั้งก่อนเริ่มงาน ตรวจ [ขั้นตอน Unity](../plugins/unity-2-5d-character/skills/character-production-orchestrator/references/unity-plugin.md) และทำตามคำสั่งของ official plugin ที่ติดตั้งอยู่จริง

แพ็กนี้รวม workflow และ templates; ไม่ได้ฝัง Unity Editor หรือ tooling ของ official Unity plugin การตั้งค่า CLI/Pipeline ที่อธิบายไว้รองรับ Unity 6.0+ และไม่ควรอัปเกรดโปรเจกต์เดิมโดยอัตโนมัติ

## 4. เปิด Unity repo ใน Codex และเริ่มงาน

เลือก repo เกมที่มี `Assets`, `Packages`, `ProjectSettings` แล้วเริ่ม thread ใหม่:

```text
Use the character-production-orchestrator skill from the unity-2-5d-character plugin.
Start a new modular 2.5D RPG character from master-character.
Inspect the repository first.
Create the character production specification before directional artwork or rigging.
Do not skip stages.
```

หาก Codex แสดงชื่อสกิลแบบมี prefix ให้เลือกสกิลจาก plugin นี้ ข้อมูล canonical จะบันทึกใน `character-production/` ของ repo เกม ไม่ใช่โฟลเดอร์ plugin และไม่ต้องพึ่ง context จากแชตเพียงอย่างเดียว

เรียกขั้นตอนใดโดยตรงได้ เช่น:

```text
Use the animation-clip-authoring skill from the unity-2-5d-character plugin.
Create SwordLightAttack for South using my canonical character specs.
```

ถ้าเคยติดตั้ง 13 skills แบบ standalone ไว้ใน `.agents/skills/` ของเกม ให้ตรวจ custom edits ก่อนนำสำเนาซ้ำออกเมื่อเปลี่ยนมาใช้ plugin เพื่อให้ Codex เลือกได้ชัดเจน

## อัปเดตและแชร์

อัปเดต repo ด้วย `git pull` แล้วเปิดหน้ารายละเอียดอีกครั้งเพื่ออัปเดต/รีโหลด plugin ตามแอป รุ่นใน manifest เป็นเวอร์ชันของ plugin; อย่าถือว่าตัวติดตั้งรันซ้ำแล้วเกมผ่าน QA ใหม่ทันที

ใช้ `python scripts/open_plugin.py --share --open` เพื่อเปิด Share view ของ local marketplace บนเครื่องคุณ หรือแชร์ URL ของ GitHub repo ให้ผู้รับ clone repo และสร้างลิงก์ด้วยเครื่องของเขาเอง
