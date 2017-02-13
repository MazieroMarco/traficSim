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
	private float _fltCamera2HighUpLimit;
	private float _fltCamera2HighDownLimit;

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
		_fltCamera1LeftLimit     = Config.INT_ROAD_SIZE / 2 * -1; // Limit depends on the chosen size of the road
		_fltCamera1RightLimit    = Config.INT_ROAD_SIZE / 2;	   // Limit depends on the chosen size of the road
		_fltCamera1UpLimit	     = 3;							   // The higher value limit
		_fltCamera1DownLimit     = 10;							   // The lower value limit
		_fltCamera2UpLimit	     = Config.INT_ROAD_SIZE / 2 * -1; // The second camera up limit
		_fltCamera2DownLimit     = Config.INT_ROAD_SIZE / 2;	   // The second camera down limit
		_fltCamera2HighUpLimit   = 3;
		_fltCamera2HighDownLimit = 1;
		_fltCamera3UpLimit	     = Config.INT_ROAD_SIZE / 2 * -1; // The third camera up limit
		_fltCamera3DownLimit     = Config.INT_ROAD_SIZE / 2;	   // The third camera down limit

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

		/// Management of the camera displacement on the y axis ///
		// Detects the down arrow key
		if (Input.GetKey (KeyCode.DownArrow)) {
			if (_caCamera2.transform.position.y > _fltCamera2HighDownLimit) {
				// Moves the camera to the right
				_caCamera2.transform.position = new Vector3(_caCamera2.transform.position.x, _caCamera2.transform.position.y - 1.5f * Time.deltaTime, _caCamera2.transform.position.z);
				_caCamera2.transform.Rotate (-20 * Time.deltaTime, 0, 0);
			}
		}

		// Detects the up arrow key
		if (Input.GetKey (KeyCode.UpArrow)) {
			if (_caCamera2.transform.position.y < _fltCamera2HighUpLimit) {
				// Moves the camera to the right
				_caCamera2.transform.position = new Vector3(_caCamera2.transform.position.x, _caCamera2.transform.position.y + 1.5f * Time.deltaTime, _caCamera2.transform.position.z);
				_caCamera2.transform.Rotate (20 * Time.deltaTime, 0, 0);
			}
		}
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

		// (DEBUG ONLY) Allows to stop the car on the road
		if (Input.GetKeyDown (KeyCode.P))
			_cbRandomCar.PROBLEM = !_cbRandomCar.PROBLEM;

		// Gets a random car if the current has been destroyed or if the left / right key is pressed
		if (_cbRandomCar == null || Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow))
			_cbRandomCar = GetRandomCar ();
		
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

		return _liAllCars [Random.Range(0, _liAllCars.Count - 1)];
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
