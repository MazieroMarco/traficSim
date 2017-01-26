using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class 	   : CameraBehavior
 * Descirption : Manages all the main camera behaviors (left-right, zoom)
 */
public class CameraBehavior : MonoBehaviour {

	// Variables declaration
	public GameObject _goMainCamera;

	private Rigidbody _rbMainCamera;
	private float _fltCameraLeftLimit;
	private float _fltCameraRightLimit;

	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Variables initialization
		_fltCameraLeftLimit  = -25;
		_fltCameraRightLimit = 25;
	}
	
	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void Update () {

		/// Management of the camera displacement on the x axis ///
		//Detects the left arrow key
		if (Input.GetKey (KeyCode.LeftArrow)) {
			// Checks the limit
			if (_goMainCamera.transform.position.x > _fltCameraLeftLimit) {
				// Moves the camera to the left
				_goMainCamera.transform.position = new Vector3(_goMainCamera.transform.position.x - 0.1f, _goMainCamera.transform.position.y, _goMainCamera.transform.position.z);
			}
		}

		// Detects the right arrow key
		if (Input.GetKey (KeyCode.RightArrow)) {
			if (_goMainCamera.transform.position.x < _fltCameraRightLimit) {
				// Moves the camera to the right
				_goMainCamera.transform.position = new Vector3(_goMainCamera.transform.position.x + 0.1f, _goMainCamera.transform.position.y, _goMainCamera.transform.position.z);
			}
		}

	}
}
