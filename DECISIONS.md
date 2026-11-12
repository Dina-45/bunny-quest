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