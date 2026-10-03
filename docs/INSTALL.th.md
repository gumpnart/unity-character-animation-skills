# ติดตั้งและอัปเดต Unity Character Creation ใน Codex Desktop

Plugin: `unity-character-creation-plugin` · Version: `2.0.0` · 13 skills

## ติดตั้งจาก source repository

บนเครื่องที่ใช้ Codex Desktop ให้ clone ลงโฟลเดอร์ถาวร:

```bash
git clone https://github.com/gumpnart/unity-character-creation-plugin.git
cd unity-character-creation-plugin
python scripts/open_plugin.py --open
```

ถ้าเปิดอัตโนมัติไม่ได้ ใช้ `python scripts/open_plugin.py` แล้วเปิดลิงก์ **View plugin** ที่พิมพ์ออกมา เลือก **Install** และเปิดใช้งานใน Codex Desktop ลิงก์ต้องชี้ marketplace ในเครื่องเดียวกับแอป อย่าย้ายหรือลบโฟลเดอร์ที่ local marketplace ใช้อยู่

ติดตั้ง official **Unity** plugin แยกต่างหาก เปิด Unity Editor และเปิด game project เป้าหมายใน Codex ทุกครั้งที่เรียก skill ต้องตรวจ connection และเรียก Unity tools จริง แม้เป็นงานวางแผนหรือ QA หากไม่เชื่อมต่อจะหยุดขั้น production ที่ขึ้นกับ Unity

แพ็กนี้รวม skills, คู่มือและ templates ไม่ได้รวม Unity bridge, exporter สำเร็จรูป หรือ character assets ที่ bake แล้ว Codex ต้องสร้าง/ปรับสิ่งเหล่านี้ในโปรเจกต์เกมและตรวจผลจริงตาม workflow

## อัปเดต clone เดิม

ถ้ายังใช้ชื่อ repo เก่า ให้แก้ origin ก่อน:

```bash
git remote set-url origin https://github.com/gumpnart/unity-character-creation-plugin.git
git pull --ff-only
python scripts/validate_plugin.py
python scripts/open_plugin.py --open
```

ตรวจหน้ารายละเอียดว่าเป็นชื่อ `unity-character-creation-plugin` และรุ่น `2.0.0` แล้วใช้ตัวเลือก update/reload ที่แอปมี หากยังแสดงรุ่นเดิม ให้ติดตั้งใหม่จาก marketplace นี้ตาม UI และเริ่ม thread ใหม่ในโปรเจกต์เกม เก็บชื่อ 13 skills เดิมไว้

ถ้ามี standalone skills ซ้ำใน `.agents/skills/` ของเกม ให้ตรวจ custom edits ก่อนลบสำเนาซ้ำเมื่อเลือกใช้ plugin

## เริ่มตัวละครใหม่

```text
Use character-production-orchestrator from unity-character-creation-plugin.
Inspect my Unity project first.
Use hybrid-baked-frames and start from master-character.
Create the six canonical specifications.
Use a neutral character in a fitted sleeveless top, shorts and bare feet.
Prove South Idle/Walk, one outfit and one weapon before expanding eight directions.
Do not skip baking, pixel cleanup or rendered QA.
```

master จะยึด identity, proportions, silhouette และ art style จากแบบที่ตกลงแล้ว ไม่จำเป็นต้องสร้างภาพเปลือย หากเปลี่ยนเสื้อผ้า ต้องเตรียมส่วนที่ซ่อนหรือถูกบังและพื้นที่ replacement ให้ครบ โดยแยกเสื้อกับกางเกง

## ย้ายตัวละครจาก v1

```text
Use character-production-orchestrator from unity-character-creation-plugin.
Migrate my existing character to the v2 hybrid pipeline.
Preserve accepted master art, source rigs, clips and item IDs.
Add missing specs without overwriting existing values.
Bake and clean South Idle/Walk, then implement and validate synchronized runtime layers.
```

v2 ใช้ rig สำหรับ authoring แล้ว bake เป็นเฟรม เก็บรายละเอียดพิกเซล และเล่น Body/Hair/Clothing/Armor/Weapon ตาม clock เดียวในเกม การติดตั้ง plugin ไม่แปลง prefab หรือแก้ภาพในเกมให้อัตโนมัติ ให้ทำตาม [migration guide](../plugins/unity-character-creation-plugin/skills/character-production-orchestrator/references/migration-v2.md) หลัง bank ใหม่ผ่าน QA จึงเปลี่ยนระบบภาพ runtime เดิม

ไฟล์ `character-production/FRAME_BANK_SPEC.md` เพิ่มขึ้นเพื่อบันทึก canvas, pivot, จำนวนเฟรม, เวลาแต่ละเฟรม, render passes, labels, marker และ coverage ของอุปกรณ์ รวมกับ CHARACTER_SPEC / DIRECTION_SPEC / ANIMATION_SPEC / EQUIPMENT_SPEC / QA_CHECKLIST เป็น 6 ไฟล์

อุปกรณ์ใหม่ใช้ source animation เดิมได้ แต่ยังต้องมีภาพแต่ละเฟรมของอุปกรณ์นั้นครบตาม action/ทิศที่ต้องการ ไม่ใช่เปลี่ยน PNG เดียวแล้วได้ทุก animation

## สร้าง action หรือวิเคราะห์ภาพแหว่ง

```text
Use animation-clip-authoring from unity-character-creation-plugin.
Create a South sword light attack using anticipation, contact and recovery.
Author, bake all required layers, clean pixels and validate final playback.
```

```text
Use character-production-qa from unity-character-creation-plugin.
Diagnose missing parts during Southwest walking.
Compare source pose, raw bake, cleaned frame and runtime composite at the same time/index.
Preserve my master and record the confirmed cause with rendered evidence.
```

ตรวจ source joints อย่างต่อเนื่อง และตรวจทุกเฟรมที่ export แล้ว รวมถึงตอนเปลี่ยนทิศ/อุปกรณ์ในเกม ดู [workflow ทั้งหมด](../plugins/unity-character-creation-plugin/README.md) และ [วิธีใช้ Unity tooling](../plugins/unity-character-creation-plugin/skills/character-production-orchestrator/references/unity-plugin.md)
