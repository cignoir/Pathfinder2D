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
				
				var cells = new Pathfinder(3, 5, Ways.FOUR).From(1, 0).To(colision.gx, colision.gz).Pathfind().Cells;
				
				for(int x = 0; x < 3; x++){
					for(int y = 0; y < 5; y++){
						Battle.cells[x, 0 , y].OnRoute = cells[x, y].IsPath;
					}
				}
				
			}
		}
	}
}
