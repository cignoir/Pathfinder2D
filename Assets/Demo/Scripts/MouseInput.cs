using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour {
	public Pathfinder pathfinder;

	void Start () {
	}
	
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 100)){
			var other = hit.collider.gameObject;
			if(other.CompareTag("Cell")){
				var colision = other.GetComponent<PathfinderCell>();

				pathfinder.ClearLogic();
				pathfinder = pathfinder.From(1, 0).To(colision.x, colision.y);
				pathfinder = pathfinder.Pathfind();
			}
		}
	}
}
