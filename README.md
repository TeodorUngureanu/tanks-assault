# Tanks' Assault

A 2D top-down WWII tank combat game built in Unity (C#) as my Bachelor's degree thesis project.

## About

You pick a tank, fight your way through a story campaign split into chapters, and face off against AI-controlled enemy tanks of increasing difficulty. The project was built to explore and apply classic software design patterns (State, Observer, Factory, Object Pool, Singleton) inside a real-time game context, alongside custom AI and pathfinding logic.

## Gameplay

- **Tank selection** — choose between multiple tanks (T-44, Tiger, Ausf.B), each with its own health and speed stats, purchasable with in-game gold.
- **Campaign** — a story mode split into chapters (intro, main menu/hub, and combat chapters), with new game / continue / load game flows.
- **Combat** — top-down tank battles: the turret tracks the mouse cursor, and you fire projectiles at enemies while managing health.
- **Enemy AI** — three difficulty tiers (Easy, Intermediate, Advanced) with progressively smarter targeting and movement behaviour.
- **Pathfinding** — enemy tanks navigate the battlefield using Bezier-curve-based path following.
- **Training mode** — a sandbox scene to practice movement and aiming outside the campaign.
- **Save system** — player profile (username, gold, owned tanks, chosen difficulty) and game progress (score, XP, health) are persisted to disk between sessions.

## Tech Stack

- **Engine:** Unity 5.5
- **Language:** C#

## Architecture & Design Patterns

The codebase applies several classic design patterns, reflecting the academic focus of the thesis:

- **State** — enemy AI behaviour (`EasyAI`, `IntermediateAI`, `AdvancedAI`) driven by per-difficulty state classes.
- **Observer** — enemy spawning/notifications via `Subject` / `IObserver` / `ObserverManager`.
- **Factory** — player creation via `UserFactory`.
- **Object Pool** — bullet/projectile reuse via `ObjectPoolerScript`.
- **Singleton** — central game state via `MainGameManager`.

## Project Structure

```
Assets/
├── Audio/              # Sound effects and music
├── Background Image/   # UI textures and menu art
├── Chapters/            # Unity scenes for each campaign chapter
├── Materials/           # Unity materials
├── Prefabs/             # Reusable game object prefabs
├── Scripts/
│   ├── AI/               # Enemy AI difficulty logic
│   ├── Chapter-1/        # Intro/name-entry logic
│   ├── Chapter0/         # Main menu logic
│   ├── Chapter2/          # Combat chapter logic
│   ├── Player/            # Movement, shooting, health, turret control
│   └── State/              # Design-pattern implementations (AI states, observer, pathfinding)
├── Tank/                # Tank models (T-44, Tiger, Ausf.B) and training scene
├── Terrain/             # Terrain assets
└── Textures/             # Environment textures
```

## Background

This project was developed as my Bachelor's thesis in Computer Science, focusing on the design and implementation of a small real-time strategy/action game with rule-based enemy AI.


## Demo

Videofinal

[<img src="https://img.youtube.com/vi/zaAYcWIDMF0/0.jpg" width="240">](https://youtu.be/zaAYcWIDMF0)

PathBezier

[<img src="https://img.youtube.com/vi/x612XGUFyhA/0.jpg" width="240">](https://youtu.be/x612XGUFyhA)

Tanks' Assault

[<img src="https://img.youtube.com/vi/0Z783r5BICU/0.jpg" width="240">](https://youtu.be/0Z783r5BICU)
