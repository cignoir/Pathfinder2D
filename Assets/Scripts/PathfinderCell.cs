using UnityEngine;
using System.Collections;

public class PathfinderCell: MonoBehaviour
{
	const int DEFAULT_STEPS = 10000;

	public int x;
	public int y;
	public PathfinderContent ContentCode { get; set; }

	public int Steps { get; set; }
	public bool IsPath { get; set; }
	public bool IsWall;
	public Direction Direction { get; set; }

	void Start(){
		ContentCode = IsWall? PathfinderContent.Wall : PathfinderContent.Empty;
		Steps = DEFAULT_STEPS;
	}
}