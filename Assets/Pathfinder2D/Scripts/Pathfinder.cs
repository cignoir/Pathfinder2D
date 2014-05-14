using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Ways
{
	FOUR, EIGHT
}

public enum Direction
{
	C, N, E, W, S, NE, NW, SE, SW
}

public enum PathfinderContent
{
	Empty,
	Start,
	Goal,
	Wall
}

public class Pathfinder: MonoBehaviour
{
	public int size_x;
	public int size_y;
	public Ways ways;


	public List<Vector2> Movements { get; set; }
	public PathfinderCell[,] Cells { get; set; }
	public List<PathfinderCell> Route { get; set; }

	void Start(){
		InitMovements(ways);
        ClearCells();
	}
	
	public void InitMovements(Ways ways)
	{
		if (ways == Ways.FOUR)
		{
			Movements = new List<Vector2>
			{
				new Vector2(0, -1),
				new Vector2(1, 0),
				new Vector2(0, 1),
				new Vector2(-1, 0)
			};
		}
		else
		{
			Movements = new List<Vector2>
			{
				new Vector2(-1, -1),
				new Vector2(0, -1),
				new Vector2(1, -1),
				new Vector2(1, 0),
				new Vector2(1, 1),
				new Vector2(0, 1),
				new Vector2(-1, 1),
				new Vector2(-1, 0)
			};
		}
	}

	public Pathfinder SetMovingPriorities(params Direction[] directions){
		if(directions[0] == Direction.E || directions[0] == Direction.W){
			directions = directions.Where(d => d == Direction.N || d == Direction.S).Concat(directions.Where(d => d == Direction.E || d == Direction.W)).ToArray();
		} else {
			directions = directions.Where(d => d == Direction.E || d == Direction.W).Concat(directions.Where(d => d == Direction.N || d == Direction.S)).ToArray();
		}

		for(int i = 0; i < directions.Length; i++){
			switch(directions[i]){
			case Direction.N:
				Movements.Insert(i, new Vector2(0, -1));
				break;
			case Direction.E:
				Movements.Insert(i, new Vector2(1, 0));
				break;
			case Direction.W:
				Movements.Insert(i, new Vector2(-1, 0));
				break;
			case Direction.S:
				Movements.Insert(i, new Vector2(0, 1));
				break;
			case Direction.NE:
				Movements.Insert(i, new Vector2(1, -1));
				break;
			case Direction.NW:
				Movements.Insert(i, new Vector2(-1, -1));
				break;
			case Direction.SE:
				Movements.Insert(i, new Vector2(1, 1));
				break;
			case Direction.SW:
				Movements.Insert(i, new Vector2(-1, 1));
				break;
			default:
				break;
			}
		}

		Movements = Movements.Distinct().ToList<Vector2>();
		return this;
	}
	
	public void ClearCells()
	{
		Route = new List<PathfinderCell>();
		Cells = new PathfinderCell[size_x, size_y];
		foreach (PathfinderCell point in FindObjectsOfType<PathfinderCell>())
		{
			Cells[point.x, point.y] = point;
		}
	}
	
	public void ClearLogic()
	{
		for(int x = 0; x < size_x; x++){
			for(int y = 0; y < size_y; y++){
				Cells[x, y].Steps = 10000;
				Cells[x, y].IsPath = false;
				if(!Cells[x, y].IsWall){
					Cells[x, y].ContentCode = PathfinderContent.Empty;
                }
			}
		}
	}
	
	public Pathfinder Pathfind()
	{
		Vector2 startingVector2 = FindCode(PathfinderContent.Start);
		int startX = (int)startingVector2.x;
		int startY = (int)startingVector2.y;
		if (startX == -1 || startY == -1)
		{
			return this;
		}
		
		
		Cells[startX, startY].Steps = 0;
		
		while (true)
		{
			bool madeProgress = false;

			for(int x = 0; x < size_x; x++){
				for(int y = 0; y < size_y; y++){
					if (CellOpen(x, y))
					{
						int passHere = Cells[x, y].Steps;
						
						foreach (Vector2 moveVector2 in ValidMoves(x, y))
						{
							int newX = (int)moveVector2.x;
							int newY = (int)moveVector2.y;
							int newPass = passHere + 1;
                            
                            if (Cells[newX, newY].Steps > newPass)
                            {
                                Cells[newX, newY].Steps = newPass;
                                madeProgress = true;
                            }
						}
					}
				}
			}

			if (!madeProgress)
			{
				break;
			}
		}
		
		HighlightPath();
		
		return this;
	}
	
	public Pathfinder From(int x, int y){
		Cells[x, y].ContentCode = PathfinderContent.Start;
		return this;
	}
	
	public Pathfinder To(int x, int y){
		if(!Cells[x, y].IsWall){
			Cells[x, y].ContentCode = PathfinderContent.Goal;
		}
		return this;
	}
	
	public Pathfinder Between(Vector2 vec1, Vector2 vec2){
		return From ((int)vec1.x, (int)vec1.y).To((int)vec2.x, (int)vec2.y);
	}
	
	private bool ValidCoordinates(int x, int y)
	{
		
		if (x < 0)
		{
			return false;
		}
		if (y < 0)
		{
			return false;
		}
		if (x > size_x - 1)
		{
			return false;
		}
		if (y > size_y - 1)
		{
			return false;
		}
		return true;
	}
	
	private bool CellOpen(int x, int y)
	{
		switch (Cells[x, y].ContentCode)
		{
		case PathfinderContent.Empty:
			return true;
		case PathfinderContent.Start:
			return true;
		case PathfinderContent.Goal:
			return true;
		case PathfinderContent.Wall:
		default:
			return false;
		}
	}
	
	private Vector2 FindCode(PathfinderContent contentIn)
	{
		for(int x = 0; x < size_x; x++){
			for(int y = 0; y < size_y; y++){
				if (Cells[x, y].ContentCode == contentIn)
				{
					return new Vector2(x, y);
                }
			}
		}
		return new Vector2(-1, -1);
	}
	
	private void HighlightPath()
	{
		Vector2 startingVector2 = FindCode(PathfinderContent.Goal);
		int pointX = (int)startingVector2.x;
		int pointY = (int)startingVector2.y;
		if (pointX == -1 && pointY == -1)
		{
			return;
		}
		
		Cells[pointX, pointY].IsPath = true;
		Route.Add(Cells[pointX, pointY]);
		
		while (true)
		{
			Vector2 lowestVector2 = new Vector2();
			int lowest = 10000;
			
			foreach (Vector2 moveVector2 in ValidMoves(pointX, pointY))
			{
				int count = Cells[(int)moveVector2.x, (int)moveVector2.y].Steps;
				if (count < lowest)
				{
					lowest = count;
					lowestVector2.x = moveVector2.x;
					lowestVector2.y = moveVector2.y;
				}
			}

			if (lowest != 10000)
			{
				Cells[(int)lowestVector2.x, (int)lowestVector2.y].IsPath = true;
				pointX = (int)lowestVector2.x;
				pointY = (int)lowestVector2.y;
				Route.Add(Cells[pointX, pointY]);
			}
			else
			{
				Cells[(int)lowestVector2.x, (int)lowestVector2.y].IsPath = false;
				break;
			}
			
			if (Cells[pointX, pointY].ContentCode == PathfinderContent.Start)
			{
				Cells[pointX, pointY].IsPath = true;
				Route.Add(Cells[pointX, pointY]);
				break;
			}
		}
		
		Route.Reverse();
		
		for(int i = 0; i < Route.Count - 1; i++){
			if(Route[i + 1].x < Route[i].x){
				Cells[Route[i].x, Route[i].y].Direction = Direction.E;
			} else if(Route[i + 1].x > Route[i].x){
				Cells[Route[i].x, Route[i].y].Direction = Direction.W;
			} else {
				if(Route[i + 1].y < Route[i].y){
					Cells[Route[i].x, Route[i].y].Direction = Direction.N;
				} else if(Route[i + 1].y > Route[i].y){
					Cells[Route[i].x, Route[i].y].Direction = Direction.S;
				} else {
					Cells[Route[i].x, Route[i].y].Direction = Direction.C;
				}
			}
		}
	}
	
	private List<Vector2> ValidMoves(int x, int y)
	{
		var list = new List<Vector2>();
		foreach (Vector2 moveVector2 in Movements)
		{
			int newX = (int)(x + moveVector2.x);
			int newY = (int)(y + moveVector2.y);
			
			if (ValidCoordinates(newX, newY) &&
			    CellOpen(newX, newY))
			{
				list.Add(new Vector2(newX, newY));
			}
		}

		return list;
	}

	public static Direction GetDirectionFromVector2(Vector2 v){
		Direction direction = Direction.C;
		if(v.x == 0){
			if(v.y == 0){
			} else if(v.y == -1){
				direction = Direction.N;
			} else if(v.y == 1){
				direction = Direction.S;
			}
		} else if(v.x == -1){
			if(v.y == 0){
				direction = Direction.W;
			} else if(v.y == -1){
				direction = Direction.NW;
			} else if(v.y == 1){
				direction = Direction.SW;
			}
		} else if(v.x == 1){
			if(v.y == 0){
				direction = Direction.E;
			} else if(v.y == -1){
				direction = Direction.NE;
			} else if(v.y == 1){
				direction = Direction.SE;
			}
		}
		return direction;
	}
	
}
