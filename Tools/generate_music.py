"""
Bunny Quest — music generator.
Makes two looping 8-bit music tracks in the same style as the sound effects:
  menu_music.wav  - calm, slow (menu)
  level_music.wav - happy, fast, with drums (levels)
Own work: synthesized by this script, no third-party audio. License: CC0.

Run:  python3 generate_music.py
The end of each track joins the start without a gap, so it loops cleanly.
"""
import os
import wave
import numpy as np

RATE = 44100
OUT = "music"
rng = np.random.default_rng(11)


def freq(name):
    steps = {"C": -9, "C#": -8, "D": -7, "D#": -6, "E": -5, "F": -4,
             "F#": -3, "G": -2, "G#": -1, "A": 0, "A#": 1, "B": 2}
    return 440.0 * 2 ** ((steps[name[:-1]] + (int(name[-1]) - 4) * 12) / 12)


def voice(f, length, kind="square", duty=0.5, decay=None, f_end=None):
    n = int(RATE * length)
    fr = np.linspace(f, f_end or f, n)
    phase = np.cumsum(fr) / RATE % 1.0
    if kind == "square":
        x = np.where(phase < duty, 1.0, -1.0)
    elif kind == "triangle":
        x = 4 * np.abs(phase - 0.5) - 1
    elif kind == "noise":
        x = rng.uniform(-1, 1, n)
    env = np.exp(-np.arange(n) / (RATE * decay)) if decay else np.ones(n)
    a, r = int(0.004 * RATE), min(int(0.03 * RATE), n // 2)   # no clicks
    env[:a] *= np.linspace(0, 1, a)
    env[-r:] *= np.linspace(1, 0, r)
    return x * env


class Track:
    def __init__(self, bpm, bars):
        self.beat = 60 / bpm
        self.buf = np.zeros(int(RATE * self.beat * 4 * bars))

    def add(self, start_beat, samples, vol):
        """Add a sound; anything past the end wraps to the start (clean loop)."""
        i = int(start_beat * self.beat * RATE) % len(self.buf)
        end = i + len(samples)
        if end <= len(self.buf):
            self.buf[i:end] += samples * vol
        else:
            cut = len(self.buf) - i
            self.buf[i:] += samples[:cut] * vol
            self.buf[:end - len(self.buf)] += samples[cut:] * vol

    def line(self, notes, vol, kind="square", duty=0.5, gap=0.9):
        """notes: list of (note name or None, length in beats)."""
        t = 0.0
        for name, beats in notes:
            if name:
                self.add(t, voice(freq(name), beats * self.beat * gap,
                                  kind, duty), vol)
            t += beats

    def save(self, name, volume=0.6):
        x = self.buf / (np.max(np.abs(self.buf)) or 1) * volume
        os.makedirs(OUT, exist_ok=True)
        with wave.open(os.path.join(OUT, name), "wb") as w:
            w.setnchannels(1)
            w.setsampwidth(2)
            w.setframerate(RATE)
            w.writeframes((x * 32767).astype(np.int16).tobytes())
        print(f"{name:16s} {len(x) / RATE:.1f} s")


CHORDS = {
    "C": ["C", "E", "G"], "Am": ["A", "C", "E"], "F": ["F", "A", "C"],
    "G": ["G", "B", "D"], "Em": ["E", "G", "B"], "D": ["D", "F#", "A"],
}

# ---------------- MENU: calm, 92 BPM, C - Am - F - G ----------------
menu = Track(bpm=92, bars=8)
menu_chords = ["C", "Am", "F", "G"] * 2

menu.line([  # melody
    ("E5", 1), ("G5", 1), ("E5", 1), ("D5", 1),
    ("C5", 2), ("A4", 2),
    ("A4", 1), ("C5", 1), ("F5", 1), ("E5", 1),
    ("D5", 3), (None, 1),
    ("E5", 1), ("G5", 1), ("C6", 1), ("B5", 1),
    ("A5", 2), ("E5", 2),
    ("F5", 1), ("E5", 1), ("D5", 1), ("C5", 1),
    ("D5", 2), ("G4", 1), (None, 1),
], vol=0.30, kind="triangle")

for bar, ch in enumerate(menu_chords):
    root = CHORDS[ch][0]
    menu.line([(None, bar * 4), (root + "3", 2), (root + "3", 2)],
              vol=0.35, kind="triangle")
    arp = [CHORDS[ch][i % 3] + "4" for i in (0, 1, 2, 1, 0, 1, 2, 1)]
    menu.line([(None, bar * 4)] + [(n, 0.5) for n in arp],
              vol=0.07, duty=0.125, gap=0.6)

menu.save("menu_music.wav")

# ---------------- LEVEL: happy, 140 BPM, G - Em - C - D + drums ----------------
level = Track(bpm=140, bars=8)
level_chords = ["G", "Em", "C", "D"] * 2

level.line([  # melody
    ("D5", .5), ("G5", .5), ("B5", .5), ("G5", .5), ("D5", .5), ("G5", .5), ("B5", 1),
    ("E5", .5), ("G5", .5), ("B5", .5), ("G5", .5), ("E5", 1), (None, 1),
    ("E5", .5), ("G5", .5), ("C6", .5), ("B5", .5), ("A5", .5), ("G5", .5), ("E5", 1),
    ("F#5", .5), ("A5", .5), ("D6", 1), ("A5", .5), ("F#5", .5), ("D5", 1),
    ("B5", .5), ("A5", .5), ("G5", .5), ("A5", .5), ("B5", 1), ("D6", 1),
    ("B5", .5), ("A5", .5), ("G5", .5), ("E5", .5), ("G5", 2),
    ("C6", .5), ("B5", .5), ("A5", .5), ("G5", .5), ("E5", .5), ("G5", .5), ("A5", 1),
    ("F#5", 1), ("A5", 1), ("D5", 1), (None, 1),
], vol=0.22, duty=0.25)

for bar, ch in enumerate(level_chords):
    root = CHORDS[ch][0]
    bass = [root + ("2" if i % 2 == 0 else "3") for i in range(8)]
    level.line([(None, bar * 4)] + [(n, 0.5) for n in bass],
               vol=0.22, duty=0.5, gap=0.7)

    for beat in range(4):                       # drums
        t = bar * 4 + beat
        if beat in (0, 2):                      # kick
            level.add(t, voice(150, 0.15, "square", 0.5, decay=0.04, f_end=45), 0.35)
        else:                                   # snare
            level.add(t, voice(0, 0.12, "noise", decay=0.04), 0.25)
        for half in (0, 0.5):                   # hi-hat
            level.add(t + half, voice(0, 0.04, "noise", decay=0.01), 0.08)

level.save("level_music.wav")
