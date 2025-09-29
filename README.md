# GAME TEST

A Unity-based game project.

## Getting Started

1. **Requirements**
   - Unity Editor 6000.1.8f1 or compatible (see [ProjectSettings/ProjectVersion.txt](ProjectSettings/ProjectVersion.txt))
   - .NET Framework (managed by Unity)
   - Visual Studio or another C# IDE (recommended)

2. **Setup**
   - Clone or download this repository.
   - Open the project folder in Unity Hub and launch the specified Unity version.
   - Open the solution file ([game_test.sln](game_test.sln)) in Visual Studio for code editing.

3. **Running the Game**
   - Open the main scene in `Assets/_Game/Scenes/`.
   - Press the Play button in the Unity Editor.

## Code Overview

- **Scripts**
  - `Assets/_Game/Scripts/` contains all gameplay scripts, including:
    - Character logic
    - Enemy AI and spawning
    - Weapons and shooting
    - UI management
    - Utilities (object pooling, initialization, etc.)
- **Input System**
  - Uses Unity's Input System ([Assets/InputSystem_Actions.inputactions](Assets/InputSystem_Actions.inputactions))
  - Input actions are auto-generated in [Assets/InputSystem_Actions.cs](Assets/InputSystem_Actions.cs)

## Controls

- **Movement:** WASD / Arrow Keys
- **Attack:** Mouse Left Click / Touch Screen
- **Switch Weapon:** Q key

## Overview Gameplay

[Watch gameplay video](Extras~/overview_gampley_game_test.mp4)