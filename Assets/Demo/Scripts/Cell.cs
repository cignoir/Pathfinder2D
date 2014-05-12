using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
	public int x;
	public int y;

	public bool IsWall;
	public bool IsPath { get; set; }
	public bool Steps { get; set; }
	
	void Start () {

	}

	void Update () {
		if(IsPath)
		{
			renderer.material.color = Color.yellow;
		} else if(IsWall) {
			renderer.material.color = Color.gray;
		} else {
			renderer.material.color = Color.green;
		}
	}
}
