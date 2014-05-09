using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
	public static Cell[,] Cells { get; set; }

	public int x;
	public int y;

	public bool IsPath { get; set; }
	
	void Start () {
	}

	void OnPostRender(){
		foreach(Cell cell in FindObjectsOfType<Cell>()){
			Cells[cell.x, cell.y] = cell;
		}
	}
	
	void Update () {
		if(IsPath)
		{
			renderer.material.color = Color.yellow;
		}
	}
}
