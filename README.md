# AmazeNet — Code Showcase

Selected C# source code from **AmazeNet**, a procedurally generated maze game developed in Unity.

This repository is a code showcase and does not contain the complete Unity project, game assets, scenes, or build files. The scripts are included as they were used in the project.

## Maze Generation

- `Mazegenerator.cs` — Generates the maze using a randomized backtracking system. It also identifies dead ends, direction changes, the starting point, and the endpoint.

- `Mazeblock.cs` — Represents an individual maze cell and manages its walls, visited state, endpoint visual, and navigation markers.

## Rush Mode

- `Sessionmode.cs` — Controls the Rush Mode gameplay loop, including the timer, maze progression, sessions, time-dot placement, score, and game-over state.

- `Timedotsetup.cs` — Handles collectible time dots. When collected, a time dot adds additional time to the current Rush Mode session.

## System Connection

The maze generator produces more than the maze geometry. Information generated during maze creation, such as dead-end positions, is reused by the gameplay system.

Rush Mode requests unused dead-end positions from the maze generator and uses them to place time dots throughout the maze.

## Full Project

AmazeNet was designed and developed in Unity using C#.

For gameplay, project details, and my other work, visit:
**https://milan1512.itch.io/amazenet**

**https://milanchandegara.com**
