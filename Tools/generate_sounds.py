"""
Bunny Quest — sound effects generator.
Makes all game SFX in one 8-bit (chiptune) style and saves them as WAV files.
Own work: the sounds are synthesized by this script, no third-party audio is used.
License: CC0 (free to use).

Run:  python3 generate_sounds.py
Result: a folder "sfx" with WAV files.
"""
import os
import wave
import numpy as np

RATE = 44100
OUT = "sfx"
rng = np.random.default_rng(7)          # fixed seed: same result every run


# ---------- basic building blocks ----------

def note(name):
    """Note name like 'C5' -> frequency in Hz."""
    steps = {"C": -9, "C#": -8, "D": -7, "D#": -6, "E": -5, "F": -4,
             "F#": -3, "G": -2, "G#": -1, "A": 0, "A#": 1, "B": 2}
    return 440.0 * 2 ** ((steps[name[:-1]] + (int(name[-1]) - 4) * 12) / 12)


def osc(f_start, length, f_end=None, kind="square", duty=0.5, vibrato=0.0):
    """Oscillator with optional pitch slide and vibrato."""
    n = int(RATE * length)
    f = np.linspace(f_start, f_end or f_start, n)
    if vibrato:
        f = f * (1 + vibrato * np.sin(2 * np.pi * 12 * np.arange(n) / RATE))
    phase = np.cumsum(f) / RATE % 1.0
    if kind == "square":
        return np.where(phase < duty, 1.0, -1.0)
    if kind == "triangle":
        return 4 * np.abs(phase - 0.5) - 1
    raise ValueError(kind)


def noise(length):
    return rng.uniform(-1, 1, int(RATE * length))


def env(n, attack=0.004, decay=None, release=0.03, grow=False):
    """Volume envelope: soft start, optional fast decay or slow growth, soft end."""
    e = np.ones(n)
    if decay:                                   # sharp sounds: fade quickly
        e = np.exp(-np.arange(n) / (RATE * decay))
    if grow:                                    # wind-up: gets louder
        e = np.linspace(0.25, 1.0, n) ** 1.5
    a = max(1, int(attack * RATE))
    r = max(1, min(int(release * RATE), n // 2))
    e[:a] *= np.linspace(0, 1, a)
    e[-r:] *= np.linspace(1, 0, r)
    return e


def shape(x, **kw):
    return x * env(len(x), **kw)


def rest(length):
    return np.zeros(int(RATE * length))


def save(name, x, volume=0.5):
    x = np.concatenate([x, rest(0.05)])
    peak = np.max(np.abs(x)) or 1
    x = x / peak * volume                       # same loudness for every file
    data = (np.clip(x, -1, 1) * 32767).astype(np.int16)
    os.makedirs(OUT, exist_ok=True)
    with wave.open(os.path.join(OUT, name), "wb") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(RATE)
        w.writeframes(data.tobytes())
    print(f"{name:16s} {len(x) / RATE:.2f} s")


# ---------- the sounds ----------

# UI click: tiny high blip
save("ui_click.wav",
     shape(osc(1400, 0.035, 1000, kind="triangle"), decay=0.012, release=0.005),
     volume=0.45)

# Jump: quick square slide up
save("jump.wav",
     shape(osc(320, 0.16, 720, duty=0.25), decay=0.09))

# Hit (player gets damage): noise burst + low thump going down. Two variants.
for i, (f1, f2) in enumerate([(210, 70), (180, 60)], start=1):
    thump = shape(osc(f1, 0.18, f2, duty=0.5), decay=0.06)
    crack = shape(noise(0.18), decay=0.025)
    save(f"hit_{i}.wav", thump * 0.7 + crack * 0.5)

# Pickup (coin): classic two-note "ding"
save("pickup.wav", np.concatenate([
    shape(osc(note("B5"), 0.06, duty=0.25), release=0.005),
    shape(osc(note("E6"), 0.28, duty=0.25), decay=0.12),
]))

# Heal (carrot): soft rising triangle arpeggio
save("heal.wav", np.concatenate([
    shape(osc(note(n), 0.07, kind="triangle"), release=0.01)
    for n in ["C5", "E5", "G5"]
] + [shape(osc(note("C6"), 0.22, kind="triangle"), decay=0.1)]))

# Enemy wind-up: rising, wobbling, getting louder -> "it is about to hit".
# Two variants so repeated attacks do not sound the same.
save("windup_1.wav",
     shape(osc(200, 0.5, 820, duty=0.3, vibrato=0.04), grow=True, release=0.02))
save("windup_2.wav",
     shape(osc(170, 0.45, 700, duty=0.4, vibrato=0.06), grow=True, release=0.02))

# Shoot: short "pew" sliding down
save("shoot.wav",
     shape(osc(950, 0.12, 280, duty=0.125), decay=0.06),
     volume=0.4)

# Win: rising major arpeggio + long high note
save("win.wav", np.concatenate([
    shape(osc(note(n), 0.11), release=0.02) for n in ["C5", "E5", "G5", "C6"]
] + [rest(0.04),
     shape(osc(note("G5"), 0.11), release=0.02),
     shape(osc(note("C6"), 0.55, duty=0.25), release=0.15)]))

# Lose: slow falling notes, the last one slides down
save("lose.wav", np.concatenate([
    np.concatenate([shape(osc(note(n), 0.25), release=0.04), rest(0.03)])
    for n in ["G4", "F#4", "F4"]
] + [shape(osc(note("E4"), 0.7, note("C#4")), release=0.2)]))
