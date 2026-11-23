# Bunny Quest

Mobile 2D platformer made in Unity 6 for Game Development Fundamentals.
Screens: main menu with level select and settings, HUD, pause menu.

## Sound sources

All sounds and music are synthesized by my own Python scripts (written with AI help). No third-party audio is used.

| Sound | Files | Source | License |
|---|---|---|---|
| UI click, jump, hit (2 clips), pickup, heal, enemy wind-up (2 clips), shoot, win, lose | Assets/Audio/SFX/*.wav | Synthesized by Tools/generate_sounds.py | CC0 |
| Menu music, level music | Assets/Audio/music/*.wav | Synthesized by Tools/generate_music.py | CC0 |

To make the files again: `python3 Tools/generate_sounds.py` and `python3 Tools/generate_music.py` (needs numpy).