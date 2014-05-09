using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Ways
{
	FOUR, EIGHT
}

public class Pathfinder
{
    static int size_x;
	static int size_y;

    public enum PathfinderContent
	{
		Empty,
		Start,
		Goal,
		Wall
	}

	public class PathfinderCell
	{
		public PathfinderContent ContentCode { get; set; }
		public int DistanceSteps { get; set; }
		public bool IsPath { get; set; }

		public PathfinderCell(){
			ContentCode = PathfinderContent.Empty;
			DistanceSteps = 10000;
		}
	}

	public Vector2[] Movements { get; set; }
	public PathfinderCell[,] Cells { get; set; }

	public Pathfinder(int size_x, int size_y, Ways ways)
	{
		Pathfinder.size_x = size_x;
		Pathfinder.size_y = size_y;

		Cells = new PathfinderCell[size_x, size_y];

		InitMovements(ways);
		ClearCells();
	}
	
	public void InitMovements(Ways ways)
	{
		if (ways == Ways.FOUR)
		{
			Movements = new Vector2[]
			{
				new Vector2(0, -1),
				new Vector2(1, 0),
				new Vector2(0, 1),
				new Vector2(-1, 0)
			};
		}
		else
		{
			Movements = new Vector2[]
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
	
	public void ClearCells()
	{
		foreach (Vector2 point in AllCells())
		{
			Cells[(int)point.x, (int)point.y] = new PathfinderCell();
		}
	}
	
	public void ClearLogic()
	{
		foreach (Vector2 point in AllCells())
		{
			int x = (int)point.x;
			int y = (int)point.y;
			Cells[x, y].DistanceSteps = 10000;
			Cells[x, y].IsPath = false;
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


		Cells[startX, startY].DistanceSteps = 0;
		
		while (true)
		{
			bool madeProgress = false;
			

			foreach (Vector2 mainVector2 in AllCells())
			{
				int x = (int)mainVector2.x;
				int y = (int)mainVector2.y;
				

				if (CellOpen(x, y))
				{
					int passHere = Cells[x, y].DistanceSteps;
					
					foreach (Vector2 moveVector2 in ValidMoves(x, y))
					{
						int newX = (int)moveVector2.x;
						int newY = (int)moveVector2.y;
						int newPass = passHere + 1;
						
						if (Cells[newX, newY].DistanceSteps > newPass)
						{
							Cells[newX, newY].DistanceSteps = newPass;
							madeProgress = true;
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
		Cells[x, y].ContentCode = Pathfinder.PathfinderContent.Start;
		return this;
	}

	public Pathfinder To(int x, int y){
		Cells[x, y].ContentCode = Pathfinder.PathfinderContent.Goal;
		return this;
	}
	
	static private bool ValidCoordinates(int x, int y)
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
		foreach (Vector2 point in AllCells())
		{
			if (Cells[(int)point.x, (int)point.y].ContentCode == contentIn)
			{
				return new Vector2(point.x, point.y);
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
		
		while (true)
		{
			Vector2 lowestVector2 = new Vector2();
			int lowest = 10000;
			
			foreach (Vector2 moveVector2 in ValidMoves(pointX, pointY))
			{
				int count = Cells[(int)moveVector2.x, (int)moveVector2.y].DistanceSteps;
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
			}
			else
			{
				break;
			}
			
			if (Cells[pointX, pointY].ContentCode == PathfinderContent.Start)
			{
				break;
			}
		}
	}
	
	private static System.Collections.Generic.IEnumerable<Vector2> AllCells()
	{
		for (int x = 0; x < size_x; x++)
		{
			for (int y = 0; y < size_y; y++)
			{
				yield return new Vector2(x, y);
			}
		}
	}
	
	private System.Collections.Generic.IEnumerable<Vector2> ValidMoves(int x, int y)
	{
		foreach (Vector2 moveVector2 in Movements)
		{
			int newX = (int)(x + moveVector2.x);
			int newY = (int)(y + moveVector2.y);
			
			if (ValidCoordinates(newX, newY) &&
			    CellOpen(newX, newY))
			{
				yield return new Vector2(newX, newY);
			}
		}
	}

}
