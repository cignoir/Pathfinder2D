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
				var collision = other.GetComponent<PathfinderCell>();

				pathfinder.ClearLogic();
				pathfinder = pathfinder.From(1, 0).To(collision.x, collision.y);
				pathfinder = pathfinder.Pathfind();

				var gui = GameObject.FindGameObjectWithTag("GUIText");
				gui.guiText.text = 
					"(x, y) = (" + collision.x + ", " + collision.y + ")"
					+ "\n IsPath = " + collision.IsPath.ToString()
					+ "\n IsWall = " + collision.IsWall.ToString()
					+ "\n Direction = " + collision.Direction.ToString()
					+ "\n Steps = " + collision.Steps.ToString();
			}
		}
	}
}
