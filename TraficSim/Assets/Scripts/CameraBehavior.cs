using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class 	   : CameraBehavior
 * Descirption : Manages all the main camera behaviors (left-right, zoom)
 */
public class CameraBehavior : MonoBehaviour {

	// Variables declaration
	public Camera _caCamera1;
	public Camera _caCamera2;
	public Camera _caCamera3;
	public Camera _caCamera4;
	public GameObject _goSun;

	// Camera 1 variables
	private float _fltCamera1LeftLimit;
	private float _fltCamera1RightLimit;
	private float _fltCamera1UpLimit;
	private float _fltCamera1DownLimit;

	// Camera 2 variables
	private float _fltCamera2UpLimit;
	private float _fltCamera2DownLimit;

	// Camera 3 variables
	private float _fltCamera3UpLimit;
	private float _fltCamera3DownLimit;

	// Camera 4 variables
	private CarBehavior _cbRandomCar;

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
		_fltCamera3UpLimit	  = Config.INT_ROAD_SIZE / 2 * -1; // The third camera up limit
		_fltCamera3DownLimit  = Config.INT_ROAD_SIZE / 2;	   // The third camera down limit

		// Disables the cameras
		DisableAllCameras();
		_caCamera1.enabled = true;
	}

	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void Update () {

		// Variables update
		_fltCamera1LeftLimit  = Config.INT_ROAD_SIZE / 2 * -1; // Limit depends on the chosen size of the road
		_fltCamera1RightLimit = Config.INT_ROAD_SIZE / 2;	   // Limit depends on the chosen size of the road
		_fltCamera2UpLimit	  = Config.INT_ROAD_SIZE / 2 * -1; // The second camera up limit
		_fltCamera2DownLimit  = Config.INT_ROAD_SIZE / 2;	   // The second camera down limit
		_fltCamera3UpLimit	  = Config.INT_ROAD_SIZE / 2 * -1; // The third camera up limit
		_fltCamera3DownLimit  = Config.INT_ROAD_SIZE / 2;	   // The third camera down limit

		// Updates the sun rotation
		_goSun.transform.Rotate(-0.2f * Time.deltaTime, 0, 0, Space.World);

		// Updates the active camera if it's enabled and the menu is not active
		if (_caCamera1.enabled == true && !Config.BLN_IS_INTERFACE_ACTIVE) {

			// Updates the camera
			UpdateCamera1();
		} else if (_caCamera2.enabled == true && !Config.BLN_IS_INTERFACE_ACTIVE) {
			
			// Updates the camera
			UpdateCamera2();
		} else if (_caCamera3.enabled == true && !Config.BLN_IS_INTERFACE_ACTIVE) {

			// Updates the camera
			UpdateCamera3();
		} else if (_caCamera4.enabled == true) {

			// Updates the camera
			UpdateCamera4();
		}

		// If space key is pressed, changes camera if the menu is not active
		if (Input.GetKeyDown (KeyCode.Space) && !Config.BLN_IS_INTERFACE_ACTIVE) {

			// Changes the cameras
			if (_caCamera1.enabled == true) {

				// Changes
				DisableAllCameras();
				_caCamera2.enabled = true;
			} else if (_caCamera2.enabled == true) {

				// Changes
				DisableAllCameras();
				_caCamera3.enabled = true;
			} else if (_caCamera3.enabled == true) {

				// Changes
				DisableAllCameras();
				_caCamera4.enabled = true;
			} else {

				// Changes
				DisableAllCameras();
				_caCamera1.enabled = true;
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
		if ((_caCamera1.transform.position.y < _fltCamera1DownLimit && Input.GetAxis ("Mouse ScrollWheel") < 0) || (_caCamera1.transform.position.y > _fltCamera1UpLimit && Input.GetAxis ("Mouse ScrollWheel") > 0)) {

			// Moves the camera
			_caCamera1.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") / 5);
		}

		/// Management of the camera displacement on the x axis ///
		//Detects the left arrow key
		if (Input.GetKey (KeyCode.LeftArrow)) {
			// Checks the limit
			if (_caCamera1.transform.position.x > _fltCamera1LeftLimit) {
				// Moves the camera to the left
				_caCamera1.transform.position = new Vector3(_caCamera1.transform.position.x - 7f * Time.deltaTime, _caCamera1.transform.position.y, _caCamera1.transform.position.z);
			}
		}

		// Detects the right arrow key
		if (Input.GetKey (KeyCode.RightArrow)) {
			if (_caCamera1.transform.position.x < _fltCamera1RightLimit) {
				// Moves the camera to the right
				_caCamera1.transform.position = new Vector3(_caCamera1.transform.position.x + 7f * Time.deltaTime, _caCamera1.transform.position.y, _caCamera1.transform.position.z);
			}
		}

		/// Management of the camera displacement on the y axis ///
		// Detects the down arrow key
		if (Input.GetKey (KeyCode.DownArrow)) {
			if (_caCamera1.transform.position.y < _fltCamera1DownLimit) {
				// Moves the camera to the right
				_caCamera1.transform.position = new Vector3(_caCamera1.transform.position.x, _caCamera1.transform.position.y + 7f * Time.deltaTime, _caCamera1.transform.position.z - 7f * Time.deltaTime);
			}
		}

		// Detects the up arrow key
		if (Input.GetKey (KeyCode.UpArrow)) {
			if (_caCamera1.transform.position.y > _fltCamera1UpLimit) {
				// Moves the camera to the right
				_caCamera1.transform.position = new Vector3(_caCamera1.transform.position.x, _caCamera1.transform.position.y - 7f * Time.deltaTime, _caCamera1.transform.position.z + 7f * Time.deltaTime);
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
		if (_caCamera2.transform.position.x < _fltCamera2DownLimit + 3 && Input.GetAxis ("Mouse ScrollWheel") > 0) {

			// Moves the camera
			_caCamera2.transform.Translate(Vector3.right * Input.GetAxis("Mouse ScrollWheel") * 4 * Time.deltaTime, Space.World);
		} else if (_caCamera2.transform.position.x > _fltCamera2UpLimit + 3 && Input.GetAxis ("Mouse ScrollWheel") < 0) {

			// Moves the camera
			_caCamera2.transform.Translate(Vector3.right * Input.GetAxis("Mouse ScrollWheel") * 4 * Time.deltaTime, Space.World);
		}

		_caCamera2.transform.Translate(Vector3.left / 70, Space.World);
	}

	/*
	* Function 	: UpdateCamera3()
	* Description  : Updates the position of the third camera
	*/
	void UpdateCamera3() {

		/// Mouse camera management ///
		// Detects the up and down mouse wheel movment
		if (_caCamera3.transform.position.x < _fltCamera3DownLimit - 2 && Input.GetAxis ("Mouse ScrollWheel") > 0) {

			// Moves the camera
			_caCamera3.transform.Translate(Vector3.right * Input.GetAxis("Mouse ScrollWheel") * 4 * Time.deltaTime, Space.World);
		} else if (_caCamera3.transform.position.x > _fltCamera3UpLimit + 3 && Input.GetAxis ("Mouse ScrollWheel") < 0) {

			// Moves the camera
			_caCamera3.transform.Translate(Vector3.right * Input.GetAxis("Mouse ScrollWheel") * 4 * Time.deltaTime, Space.World);
		}
	}

	/*
	* Function 	: UpdateCamera4()
	* Description  : Updates the position of the fourth camera
	*/
	void UpdateCamera4() {

		// Gets a random car if the current has been destroyed
		if (_cbRandomCar == null || Input.GetKeyDown (KeyCode.R))
			_cbRandomCar = GetRandomCar ();
		if (Input.GetKey (KeyCode.LeftArrow))
			_cbRandomCar.gameObject.transform.Rotate (Vector3.down*3);
		if (Input.GetKey (KeyCode.RightArrow))
			_cbRandomCar.gameObject.transform.Rotate (Vector3.up*3);
		if (Input.GetKey (KeyCode.UpArrow))
			_cbRandomCar._fltCarSpeed += Config.FLT_DRIVER_ACCELERATION_SPEED;
		if (Input.GetKey (KeyCode.DownArrow))
			_cbRandomCar._fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
		if (Input.GetKey (KeyCode.W))
			_cbRandomCar.transform.Rotate(3,0,0);
		if (Input.GetKey (KeyCode.S))
			_cbRandomCar.transform.Rotate(-2,0,0);
		if (Input.GetKeyDown (KeyCode.P))
			_cbRandomCar.PROBLEM = !_cbRandomCar.PROBLEM;
		
		_caCamera4.transform.position = new Vector3(_cbRandomCar.transform.position.x, _cbRandomCar.transform.position.y + 0.05f, _cbRandomCar.transform.position.z);

		//_caCamera4.transform.rotation = _cbRandomCar._v3CarDirection == Vector3.left ? Quaternion.Euler (0, -90, 0) : Quaternion.Euler (0, 90, 0);
		_caCamera4.transform.rotation = _cbRandomCar.transform.rotation;
	}

	/*
	 * Function    : GetRandomCar()
	 * Description : Gets a random car from the scene
	 */
	CarBehavior GetRandomCar () {

		// Gets all the cars in the scene
		var _liAllCars = new List<CarBehavior>();

		foreach (Road _road in Config.LI_GAME_ROADS) {

			_liAllCars.AddRange (_road._liCars);
		}

		return _liAllCars [_liAllCars.Count - 1];
	}

	/*
	 * Function    : DisableAllCameras()
	 * Description : Disables all the cameras
	 */
	void DisableAllCameras () {

		// Disables the cameras
		_caCamera1.enabled = false;
		_caCamera2.enabled = false;
		_caCamera3.enabled = false;
		_caCamera4.enabled = false;
	}
}
