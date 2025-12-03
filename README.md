# Advent of Code 2025

This repository contains my solutions for the Advent of Code 2025 programming puzzles.

## Project Structure

The project is organized as follows:

- `Solvers/`: Contains the C# source code for each day's solution.
  - `Day01.cs`, `Day02.cs`, etc. - Each file corresponds to a specific day's puzzle.
  - `BaseSolver.cs`: A base class inherited by each daily solver, likely containing common functionality.
- `Inputs/`: Contains the input data for each day's puzzle.
  - `01.txt`, `02.txt`, etc. - Each file contains the input for the corresponding day.
- `Program.cs`: The main entry point of the application, which is probably responsible for running the solvers.
- `AoC2025.csproj`: The C# project file.

## Running the Solutions

To run the solutions, you will need to have the .NET SDK installed. You can then run the project from the command line.

By default, the application runs the latest available solver.

```bash
dotnet run
```

You can also specify which day and part to run using command-line arguments.

- To run both parts of a specific day (e.g., Day 5):
  ```bash
  dotnet run 5
  ```

- To run a single part of a specific day (e.g., Day 5, Part 1):
  ```bash
  dotnet run 5 1
  ```
