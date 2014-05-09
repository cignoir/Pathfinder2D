Pathfinder2D
============

This is an implementation of A* pathfinding algorithm for Unity using 2D Arrays.

## Download
coming soon

## Features
* Unity only.
* Easy to simulate A* pathfinding using 2D Arrays.
* Support 4-ways and 8-ways.
* No dependencies except UnityEngine.
* Written in C#.
* Easy to customize.
* Bundled a demo project.

## Usage
```c#
// Specify width, height and count of ways to walk.
var pathfinder = new Pathfinder(3, 5, Ways.FOUR);

// Specify the start-point and end-point.
pathfinder = pathfinder.From(1, 0).To(3, 3);

// Then start calculation.
pathfinder = pathfinder.Pathfind();

// Write more simply.
// var cells = new Pathfinder(3, 5, Ways.FOUR).From(1, 0).To(3, 3).Pathfind().Cells;

// After calculation, you can access the result like this.
Debug.Log(pathfinder.Cells[2, 4].IsPath); // Is this cell walked?
Debug.Log(pathfinder.Cells[2, 4].DistanceSteps);  // Showing count of steps.
```

## License
Pathfinder2D is released under the MIT license. 
