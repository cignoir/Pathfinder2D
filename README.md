Pathfinder2D
============

![logo](https://dl.dropboxusercontent.com/u/40069781/Pathfinder2D/7708b0f1e1ed639c0972977cd1fa09cb.png)

This is an implementation of A* pathfinding algorithm for Unity using 2D Arrays.

## Web Demo
https://dl.dropboxusercontent.com/u/40069781/Pathfinder2D/Pathfinder2D.html

## Installation
Import Pathfinder2D.unitypackage into your unity project.

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

// Place walls.
pathfinder = pathfinder.Wall(1, 3);

// Then start calculation.
pathfinder = pathfinder.Pathfind();

// Write more simply.
// var cells = new Pathfinder(3, 5, Ways.FOUR).From(1, 0).To(3, 3).Pathfind().Cells;

// After calculation, you can access the result like this.
Debug.Log(pathfinder.Cells[2, 4].IsPath); // Is this cell the path?
Debug.Log(pathfinder.Cells[2, 4].Steps);  // Showing count of distance steps.
```

## License
Pathfinder2D is released under the MIT license. 
