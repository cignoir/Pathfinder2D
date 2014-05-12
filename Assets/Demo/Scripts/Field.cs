using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour {
	public const int Width = 3;
	public const int Height = 5;

	public static Cell[,] Cells { get; set; }

	void Start () {
		Cells = new Cell[Width, Height];
		foreach(Cell cell in FindObjectsOfType<Cell>()){
			Cells[cell.x, cell.y] = cell;
		}
	}
	
	void Update () {

	}
}
