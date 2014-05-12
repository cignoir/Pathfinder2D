using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 100)){
			var other = hit.collider.gameObject;
			if(other.CompareTag("Cell")){
				var colision = other.GetComponent<Cell>();

				var pathfinder = new Pathfinder(Field.Width, Field.Height, Ways.FOUR).From(1, 0).To(colision.x, colision.y);
				pathfinder = pathfinder.Wall(1, 3);
				var cells = pathfinder.Pathfind().Cells;

				for(int x = 0; x < Field.Width; x++){
					for(int y = 0; y < Field.Height; y++){
						Field.Cells[x, y].IsPath = cells[x, y].IsPath;
					}
				}
				
			}
		}
	}
}
