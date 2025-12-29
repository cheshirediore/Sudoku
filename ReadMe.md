# Roadmap

## Phase 1: Sudoku Puzzle [Complete]

Phase 1 focuses on the internal integrity of the sudoku puzzle. Any UI at this point is scaffolding. Puzzles are not generated at this phase; puzzle values must be manually entered.

Milestones (unordered) are:
- Represent a sudoku puzzle. [Complete]
    - Sudoku puzzle with standard 9x9 dimensions [Complete]
    - Each cell can contain a value in the range 1-9 [Complete]
    - Columns, Rows, and Blocks can be accessed as a unit [Complete]
    - Each cell contains a value, which can be hidden or visible. [Complete]
- Internal validation of a sudoku puzzle. Given a filled puzzle, determine if it is a valid solution. [Complete]


## Phase 2: Puzzle Generation [Complete]

Phase 2 focuses entirely on the puzzle generation.

Milestones are:
- Generate valid sudoku puzzle solution. [Complete]
- Remove values until a minimal number of visible values remain to constitute a valid puzzle. [Complete]
- Extend Generator to allow more variety in generated puzzles. [Complete]

## Phase 2.5: Preliminary Optimization [Complete]

This intermediary phase focuses on getting runtimes down to a level acceptable for iterative development and testing. Proper optimization will occur at the end of the project. [Complete]

~~## Phase 3: Difficulty~~

~~Phase 3 focuses on refining the "minimal" puzzle from Puzzle Generation into categories of Easy, Medium, Hard, Expert. Milestones include:~~
~~- Formally define each category~~
~~- Implement difficulty selection method(s)~~


## Phase 4: Clean-up and Generalize API
Phase 4 focuses on refactoring and restructuring the codebase, and cleaning up the accessors and public methods. This phase prepares the backend code to play well with others, with emphasis on preparation for the GUI.

Milestones include:
- Determine what needs to be exposed to the front-end, and what should not be.
- Add documentation comments to methods. [Complete]
- Verify that all methods and properties have the correct access levels.
- Add public methods, as needed, to allow communication with a front-end GUI.
- Remove unused methods.

## Phase 5: GUI

Phase 5 focuses on adding a front-end GUI to the backend created in the previous phases. Engine/Framework TBD, but it'll probably be Godot.

Milestones TBD.


## Phase 6: Interaction

Phase 6 focuses on updating cells. UI is still scaffolding at this point, but the user should be able to enter values into the grid and have it validated.

Milestones are:
- Sudoku puzzle takes input for cells and records them for future validation.
- Differentiate between "pencil marks" and "pen marks"
