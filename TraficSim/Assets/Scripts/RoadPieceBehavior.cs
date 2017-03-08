using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class 	   : RoadPieceBehavior
 * Descirption : Defines the behavior of a road piece
 */
public class RoadPieceBehavior : MonoBehaviour {

	// Variables declaration
	private bool _blnHaveObstacle = false;		// Used to know if there's already an obstacle on the road

	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

	}

	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void Update () {

		// If the interface mode is activated removes the focus
		if (Config.BLN_IS_INTERFACE_ACTIVE && GetComponent<Renderer> ().material != Resources.Load("Road_01", typeof(Material)) as Material)
			GetComponent<Renderer> ().material = Resources.Load("Road_01", typeof(Material)) as Material;
	}

	/*
	 * Function 	: OnMouseEnter()
	 * Description  : Sets the focus to the road piece when the mouse cursor enters the road piece
	 */
	void OnMouseEnter() {

		// Checks if not in interface mode
		if (Config.BLN_IS_INTERFACE_ACTIVE) return;

		// Sets the focus
		GetComponent<Renderer>().material = Resources.Load("Road_01_focus", typeof(Material)) as Material;
	}

	/*
	 * Function 	: OnMouseExit()
	 * Description  : Disables the focus on the road piece when the mouse cursor exits
	 */
	void OnMouseExit() {

		// Checks if not in interface mode
		if (Config.BLN_IS_INTERFACE_ACTIVE) return;

		// Removes the focus
		GetComponent<Renderer> ().material = Resources.Load("Road_01", typeof(Material)) as Material;
	}

	/*
	 * Function 	: OnMouseDown()
	 * Description  : Puts or removes the obstacle on the road when it's clicked
	 */
	void OnMouseDown() {

		// Checks if not in interface mode
		if (Config.BLN_IS_INTERFACE_ACTIVE) return;

		// Checks if there's already an obstacle on the road
		if (_blnHaveObstacle) {

			// Removes the obstacle
			foreach (GameObject _goObst in GameObject.FindGameObjectsWithTag("Obstacle")) {

				// If the obstacle position is right, deletes it
				if (_goObst.transform.position == transform.position) {

					// Deletes the obstacle
					Destroy(_goObst);
				}
			}

			// Sets the boolean
			_blnHaveObstacle = false;

		} else {

			// instanciates the obstacle on the road
			GameObject _goObstacle = GameObject.Instantiate(Resources.Load<GameObject>("road_obstacle"));
			_goObstacle.transform.position = transform.position;

			// Ses the boolean
			_blnHaveObstacle = true;
		}
	}
}
