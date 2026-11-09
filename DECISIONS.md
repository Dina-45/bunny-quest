# DECISIONS.md - Bunny Quest

Assignment 10 (three enemy types). I lead, the AI helps. AI chat used: Claude. I test everything in Unity myself.

## Week 11 (Assignment 10)

- 2026-11-09  Shared my EnemyBase, PatrollerEnemy and ShooterEnemy for review. Two problems found: Obstacle Mask was "Nothing", so walls did not block the Raycast, and ShooterEnemy had no player null check. Created a Ground layer, set it on the ground and on every enemy, added the check.
- 2026-11-09  Shooter only stood still and turned red, which felt boring next to the Patroller. Asked for movement. Got a keep-distance version (minDistance, maxDistance, leash). I checked that maxDistance stays below sightRange so he can still see the player.
- 2026-11-09  Fireball flew tail first and passed through platforms, so hiding behind a wall did not work. Rotated the sprite along its direction and added a groundMask so walls destroy it.
- TODO  Charger: what I asked, what was wrong, what I changed.
- TODO  Playtest: names, tries, and the 2+ numbers I changed (old value -> new value).