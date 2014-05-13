using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
	PathfinderCell pathfinderCell;

	void Start () {
		pathfinderCell = GetComponent<PathfinderCell>();
	}

	void Update () {
		if(pathfinderCell.IsPath)
		{
			renderer.material.color = Color.yellow;
		} else if(pathfinderCell.IsWall) {
			renderer.material.color = Color.gray;
		} else {
			renderer.material.color = Color.green;
		}
	}
}
