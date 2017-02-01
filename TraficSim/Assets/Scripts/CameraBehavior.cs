using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class 	   : CameraBehavior
 * Descirption : Manages all the main camera behaviors (left-right, zoom)
 */
public class CameraBehavior : MonoBehaviour {

	// Variables declaration
	public Camera caCamera1;
	public Camera _caCamera2;
	public GameObject _goSun;

	// Camera 1 variables
	private float _fltCamera1LeftLimit;
	private float _fltCamera1RightLimit;
	private float _fltCamera1UpLimit;
	private float _fltCamera1DownLimit;

	// Camera 2 variables
	private float _fltCamera2UpLimit;
	private float _fltCamera2DownLimit;

	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Variables initialization
		_fltCamera1LeftLimit  = Config.INT_ROAD_SIZE / 2 * -1; // Limit depends on the chosen size of the road
		_fltCamera1RightLimit = Config.INT_ROAD_SIZE / 2;	   // Limit depends on the chosen size of the road
		_fltCamera1UpLimit	  = 3;							   // The higher value limit
		_fltCamera1DownLimit  = 10;							   // The lower value limit
		_fltCamera2UpLimit	  = Config.INT_ROAD_SIZE / 2 * -1; // The second camera up limit
		_fltCamera2DownLimit  = Config.INT_ROAD_SIZE / 2;	   // The second camera down limit
	}

	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void Update () {

		// Updates the sun rotation
		_goSun.transform.Rotate(-0.005f, 0, 0);

		// Updates the active camera
		if (caCamera1.enabled == true) {

			// Updates the camera
			UpdateCamera1();
		} else if (_caCamera2.enabled == true) {
			
			// Updates the camera
			UpdateCamera2();
		}

		// If space key is pressed, changes camera
		if (Input.GetKeyDown (KeyCode.Space)) {

			// Changes the cameras
			if (caCamera1.enabled == true) {

				caCamera1.enabled = false;
				_caCamera2.enabled = true;
			} else {

				caCamera1.enabled = true;
				_caCamera2.enabled = false;
			}
		}
	}

	/*
	 * Function 	: UpdateCamera1()
	 * Description  : Updates the position of the first camera
	 */
	void UpdateCamera1 () {

		/// Mouse camera management ///
		// Detects the up and down mouse wheel movment
		if (caCamera1.transform.position.y < _fltCamera1DownLimit && Input.GetAxis ("Mouse ScrollWheel") < 0) {

			// Moves the camera
			caCamera1.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") / 5);
		} else if (caCamera1.transform.position.y > _fltCamera1UpLimit && Input.GetAxis ("Mouse ScrollWheel") > 0) {

			// Moves the camera
			caCamera1.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") / 5);
		}

		/// Management of the camera displacement on the x axis ///
		//Detects the left arrow key
		if (Input.GetKey (KeyCode.LeftArrow)) {
			// Checks the limit
			if (caCamera1.transform.position.x > _fltCamera1LeftLimit) {
				// Moves the camera to the left
				caCamera1.transform.position = new Vector3(caCamera1.transform.position.x - 0.2f, caCamera1.transform.position.y, caCamera1.transform.position.z);
			}
		}

		// Detects the right arrow key
		if (Input.GetKey (KeyCode.RightArrow)) {
			if (caCamera1.transform.position.x < _fltCamera1RightLimit) {
				// Moves the camera to the right
				caCamera1.transform.position = new Vector3(caCamera1.transform.position.x + 0.2f, caCamera1.transform.position.y, caCamera1.transform.position.z);
			}
		}

		/// Management of the camera displacement on the y axis ///
		// Detects the down arrow key
		if (Input.GetKey (KeyCode.DownArrow)) {
			if (caCamera1.transform.position.y < _fltCamera1DownLimit) {
				// Moves the camera to the right
				caCamera1.transform.position = new Vector3(caCamera1.transform.position.x, caCamera1.transform.position.y + 0.2f, caCamera1.transform.position.z - 0.2f);
			}
		}

		// Detects the up arrow key
		if (Input.GetKey (KeyCode.UpArrow)) {
			if (caCamera1.transform.position.y > _fltCamera1UpLimit) {
				// Moves the camera to the right
				caCamera1.transform.position = new Vector3(caCamera1.transform.position.x, caCamera1.transform.position.y - 0.2f, caCamera1.transform.position.z + 0.2f);
			}
		}
	}

	/*
	 * Function 	: UpdateCamera2()
	 * Description  : Updates the position of the second camera
	 */
	void UpdateCamera2() {

		/// Mouse camera management ///
		// Detects the up and down mouse wheel movment
		Debug.Log (Input.GetAxis ("Mouse ScrollWheel"));
		if (_caCamera2.transform.position.x < _fltCamera2DownLimit + 3 && Input.GetAxis ("Mouse ScrollWheel") > 0) {

			// Moves the camera
			_caCamera2.transform.Translate(Vector3.right * Input.GetAxis("Mouse ScrollWheel") / 5, Space.World);
		} else if (_caCamera2.transform.position.x > _fltCamera2UpLimit + 3 && Input.GetAxis ("Mouse ScrollWheel") < 0) {

			// Moves the camera
			_caCamera2.transform.Translate(Vector3.right * Input.GetAxis("Mouse ScrollWheel") / 5, Space.World);
		}

		_caCamera2.transform.Translate(Vector3.left / 70, Space.World);
	}
}
