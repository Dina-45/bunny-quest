# DECISIONS.md - Bunny Quest

Assignment 10 (three enemy types). I lead, the AI helps. AI chat used: Claude. I test everything in Unity myself.

## Week 11 (Assignment 10)

- 2026-11-09  Shared my EnemyBase, PatrollerEnemy and ShooterEnemy for review. Two problems found: Obstacle Mask was "Nothing", so walls did not block the Raycast, and ShooterEnemy had no player null check. Created a Ground layer, set it on the ground and on every enemy, added the check.
- 2026-11-09  Shooter only stood still and turned red, which felt boring next to the Patroller. Asked for movement. Got a keep-distance version (minDistance, maxDistance, leash). I checked that maxDistance stays below sightRange so he can still see the player.
- 2026-11-09  Fireball flew tail first and passed through platforms, so hiding behind a wall did not work. Rotated the sprite along its direction and added a groundMask so walls destroy it.
- TODO  Charger: what I asked, what was wrong, what I changed.
- TODO  Playtest: names, tries, and the 2+ numbers I changed (old value -> new value).
- 2026-11-09  Charger hung in the air and could not reach the player. Asked for a fix: it now locks the player's position during the 0.7 s windup, dives there, then flies back to its place.
- 2026-11-10  Claude suggested a yellow warning flash for the Patroller. It did not work, because the tint multiplies the red sprite. Switched to a dark grey flash.
- 2026-11-10  Player bullets left the screen too fast. Speed 10 -> 5, lifetime 2 -> 3.
- 2026-11-10  Playtest 1 (before tuning), iPhone: Daniyar needed 6 tries, the target is 2-4. After it I enlarged the joystick background (StickBG) from 300 to 350, because it was too small to use with a thumb, and raised the player speed 4 -> 5.
- 2026-11-12  Started moving balance numbers into data. Created PlayerStats.asset (speed, jump, health, shooting). My first copy had jump force 10, but the player really had 9. Fixed to 9, then Shooter and Bullet read their numbers from the asset.
- 2026-11-12  Moved enemy numbers from three scripts into PatrollerStats, ShooterStats and ChargerStats assets. I also found values hidden in code (hit height, fireball spawn offset, wall check distance, fireball lifetime) and made them fields.
## Week 12 (Assignment 11)
- 2026-11-16: Level exit calls ProgressTracker, which updates the best score, unlocks the next level and saves right away, before the win sound plays.
- 2026-11-16: Level1 Exit still pointed to the Win scene from the playtest, so Level2 was skipped. I cleared the field.
- 2026-11-16: After copying the pause UI between scenes, the Coin Text and Hp Text references were empty and the HUD did not update. I reassigned them from the scene Hierarchy.
- Boss Die() calls ProgressTracker.CompleteLevel(0) before the death delay, so the final score is saved even if the app closes during the death sound.
- Level numbers (coins, unlocked level, next scene) moved from scene objects into LevelData assets Level_01..03, so one script can load any level from data.
- 2026-11-16  Assignment requires saving in OnApplicationPause(true), my code only saved after a level. Added AppSaveHook: it creates itself before the first scene with RuntimeInitializeOnLoadMethod, survives scene changes and saves on pause and on quit.
- 2026-11-16  musicVol was in SaveData but nothing used it, so the settings test was impossible. Added SettingsPanel with a volume slider in MainMenu: it saves on every change and sets AudioListener.volume.
- 2026-11-18  Added a Reset progress button in the settings window with an in-game YES/NO confirm panel. Changed SaveSystem.ResetProgress to keep musicVol, because reset should clear progress, not the player's settings.