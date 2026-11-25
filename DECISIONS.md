# DECISIONS.md - Bunny Quest

I lead, the AI helps. AI chat used: Claude. I test everything in Unity and on my iPhone myself.

## Week 11 (Assignment 10)

- 2026-11-09  Shared my EnemyBase, PatrollerEnemy and ShooterEnemy for review. Two problems found: Obstacle Mask was "Nothing", so walls did not block the Raycast, and ShooterEnemy had no player null check. Created a Ground layer, set it on the ground and on every enemy, added the check.
- 2026-11-09  Shooter only stood still and turned red, which felt boring next to the Patroller. Asked for movement. Got a keep-distance version (minDistance, maxDistance, leash). I checked that maxDistance stays below sightRange so he can still see the player.
- 2026-11-09  Fireball flew tail first and passed through platforms, so hiding behind a wall did not work. Rotated the sprite along its direction and added a groundMask so walls destroy it.
- 2026-11-09  Charger hung in the air and could not reach the player. Asked for a fix: it now locks the player's position during the 0.7 s windup, dives there, then flies back to its place.
- 2026-11-10  Claude suggested a yellow warning flash for the Patroller. It did not work, because the tint multiplies the red sprite. Switched to a dark grey flash.
- 2026-11-10  Player bullets left the screen too fast. Speed 10 -> 5, lifetime 2 -> 3.
- 2026-11-10  Playtest 1 (before tuning), iPhone: Daniyar needed 6 tries, the target is 2-4. After it I enlarged the joystick background (StickBG) from 300 to 350, because it was too small to use with a thumb, and raised the player speed 4 -> 5.
- 2026-11-12  Started moving balance numbers into data. Created PlayerStats.asset (speed, jump, health, shooting). My first copy had jump force 10, but the player really had 9. Fixed to 9, then Shooter and Bullet read their numbers from the asset.
- 2026-11-12  Moved enemy numbers from three scripts into PatrollerStats, ShooterStats and ChargerStats assets. I also found values hidden in code (hit height, fireball spawn offset, wall check distance, fireball lifetime) and made them fields.

## Week 12 (Assignment 11)

- 2026-11-16  Level exit calls ProgressTracker, which updates the best score, unlocks the next level and saves right away, before the win sound plays.
- 2026-11-16  Level1 Exit still pointed to the Win scene from the playtest, so Level2 was skipped. I cleared the field.
- 2026-11-16  After copying the pause UI between scenes, the Coin Text and Hp Text references were empty and the HUD did not update. I reassigned them from the scene Hierarchy.
- 2026-11-16  Boss Die() calls ProgressTracker.CompleteLevel(0) before the death delay, so the final score is saved even if the app closes during the death sound.
- 2026-11-16  Level numbers (coins, unlocked level, next scene) moved from scene objects into LevelData assets Level_01..03, so one script can load any level from data.
- 2026-11-16  Assignment requires saving in OnApplicationPause(true), my code only saved after a level. Added AppSaveHook: it creates itself before the first scene with RuntimeInitializeOnLoadMethod, survives scene changes and saves on pause and on quit.
- 2026-11-16  musicVol was in SaveData but nothing used it, so the settings test was impossible. Added SettingsPanel with a volume slider in MainMenu that saves on every change.
- 2026-11-18  Added a Reset progress button in the settings window with an in-game YES/NO confirm panel. Changed SaveSystem.ResetProgress to keep the volume, because reset should clear progress, not the player's settings.
- 2026-11-18  PLAY loaded Level1 by build index and ignored the save. Added a level select window: buttons read levelNumber and sceneName from LevelData, locks come from the unlocked list in save.json.
- 2026-11-18  Win scene had a GameManager with an empty LevelData, so the console showed an error. Removed it, the Win scene does not need it.

## Week 13 (Assignment 12)

- 2026-11-23  I could not find win and lose jingles with a clear license. Asked Claude to synthesize all SFX and both music loops in one 8-bit style with Python scripts (Tools/). License is clear (own work, CC0) and the style matches the game.
- 2026-11-23  Sounds played with PlayClipAtPoint could not be routed to the mixer. Made one SoundManager (8 AudioSources in the SFX group) and a SoundLibrary asset: clips, volume and pitch range 0.92-1.08 are data, not code. Win and lose have no random pitch, it made the melody sound out of tune.
- 2026-11-23  Enemy attacks had no warning sound. Added the wind-up sound at the start of the wind-up (Patroller, Charger), the aim (Shooter) and before the boss rain, so the player hears the attack before it lands.
- 2026-11-23  Music had to survive scene loads. Put MusicManager on the DontDestroyOnLoad AudioManager with two AudioSources and a 1 s crossfade; the same clip is not restarted, so Level1 -> Level2 and a death restart keep the music going.
- 2026-11-23  One slider changed AudioListener.volume for everything. Made two sliders (Music, SFX) that set the mixer exposed parameters in dB with Log10 x 20; a linear value made 50% sound almost silent. Added sfxVol and save version 2 so old saves get a default.
- 2026-11-23  The agent put Start() between [RuntimeInitializeOnLoadMethod] and Create() when I pasted the code, so the attribute moved to the wrong method. I put Create() back right under the attribute.
- 2026-11-23  Pause only stopped time. Added Normal and Paused snapshots (MusicBed -10 dB, lowpass 800 Hz). MusicBed is a child of Music, so the Music slider and the snapshot do not fight over one volume. Mixer Update Mode is Unscaled Time, otherwise the transition never finishes with timeScale = 0.
## Week 14 (Assignment 13)
- 2026-11-25  Landing code threw a NullReferenceException before isGrounded = true, so the bunny could not jump. Moved isGrounded = true to the top and made JuiceSettings.Get null-safe.
- 2026-11-25  With hit stop 0.05 s and shake 0.08 I could not see if they work. Tested with 0.3 s / 0.5, then returned to small values so the effect is felt, not annoying.
- 2026-11-25  Win confetti was hard to see: small and flew off screen. Size 0.1-0.2 -> 0.2-0.35, speed 4-8 -> 3-6, gravity 1 -> 0.6, and it now spawns one unit above the bunny instead of at the exit.