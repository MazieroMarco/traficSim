using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class 	   : CarBehavior
 * Descirption : Manages all the diferent cars behaviors
 */
public class CarBehavior : MonoBehaviour {

	// Variables initialisation
	public Vector3 _v3CarDirection = Vector3.right;	// The current direction of the car
	public float _fltRandomSpeed;					// The custom speed factor of the car
	public float _fltCarSpeed;						// The current total speed of the car

	public bool PROBLEM = false;
	private float _fltCarInitialSpeed;				// The initial speed before detections (used during the detections)


	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Initializes the random speed factor
		_fltRandomSpeed = Random.Range(0, Config.FLT_DRIVERS_SPEED_FACTOR_KMH);

		// Intializes the total car speed
		_fltCarSpeed = ((_fltRandomSpeed + Config.INT_SPEED_LIMIT_KMH) * 100 / 60 / 60);

		// Sets the initial speed
		_fltCarInitialSpeed = _fltCarSpeed;
	}
	
	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void FixedUpdate () {

		if (PROBLEM)
			_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
		
		// Moves the car
		CarMoveForward();

		// Checks for collisions
		CarCollisionsCheck ();

		// Check for destroy
		CarDestroyOnLimit();
	}

	/*
	 * Function 	: CarMoveForward()
	 * Description  : Moves the car forward on the road lane
	 */
	void CarMoveForward () {

		// Puts the car speed to 0 if under 0
		if (_fltCarSpeed < 0)
			_fltCarSpeed = 0;

		// Moves the car forward if the speed is not under 0
		if (_fltCarSpeed >= 0) {
			transform.Translate(new Vector3(0, 0, _fltCarSpeed) * Time.deltaTime);
		}
	}

	/*
	 * Function 	: CarDestroy()
	 * Description  : Destroys the current car instance when it reaches the end of the road
	 */
	void CarDestroyOnLimit () {

		// Variables declaration
		int _intRoadLength = Config.INT_ROAD_SIZE;

		// Detects the end of the road
		if (transform.position.x > (_intRoadLength / 2) + 1 || transform.position.x < (_intRoadLength / 2 * -1) - 1) {

			// Destroys the current car
			Destroy(gameObject);

			// Updates the car output counter
			Config.INT_CARS_OUTPUT += 1;
		}
	}

	/*
	 * Function 	: CarCollisionsCheck()
	 * Description  : Checks for collisions with the other front cars.
	 * 				  When a car is detected and is in the safety distance,
	 * 				  the current car starts to slow down.
	 * 				  When no car is detected, the car speeds up to his initial speed
	 */
	void CarCollisionsCheck () {

		// Variables declaration
		RaycastHit _rhCarInRange;	// This is the car detected in the hit range
		Ray _rRangeDetection = new Ray (){direction = _v3CarDirection, origin = transform.position};	// This is the car deteciont range

		// Casts the detection zone to find cars ahead. It returns trus if detection
		if (Physics.Raycast (_rRangeDetection, out _rhCarInRange, Config.FLT_AHEAD_CAR_DETECTION_DIST)) {

			////////////////////////////////////////////////////////////
			/// Distance detections
			////////////////////////////////////////////////////////////
			if (_rhCarInRange.distance < 0.3f) { // Between 0 and 2 meters

				// Slows down the car to avoid collision
				_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED * 2;

			} else if (_rhCarInRange.distance > 0.3f && _rhCarInRange.distance < 0.6f) { // Between 2 and 5 meters

				// Slows down the car to the detected car speed to avoid collision
				if (_fltCarSpeed > _rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed) {

					// Slows down the car
					_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
				}

			} else if (_rhCarInRange.distance > 0.6f && _rhCarInRange.distance < 0.8f) { // Between 5 and 7 meters

				// Tries to change lane
				ChangeLane(0.5f);

			} else if (_rhCarInRange.distance > 0.8f && _rhCarInRange.distance < 1f) { // Between 7 and 10 meters

				// Speeds up the car to reach the detected car speed
				if (_fltCarSpeed < _rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed) {

					// Speeds up the car
					_fltCarSpeed += Config.FLT_DRIVER_ACCELERATION_SPEED;
				}
			}
			////////////////////////////////////////////////////////////

			////////////////////////////////////////////////////////////
			/// Anomalies / stange behaviors
			////////////////////////////////////////////////////////////

			// If there's a very slow object on the road
			if (_rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed < (Config.INT_SPEED_LIMIT_KMH * 100 / 60 / 60 / 2)) {

				// Already slows down the car
				_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
			}
		}

		// If the car speed is lower thant the initial, speeds it up
		if (_fltCarSpeed < _fltCarInitialSpeed) {
			
			// Speeds up the car
			_fltCarSpeed += Config.FLT_DRIVER_ACCELERATION_SPEED;
		}
	}

	/*
	 * Function 	: ChangeLane()
	 * Description  : When called, the car will try to change his lane on the road.
	 * 
	 * Parameters   : float _fltSecurityDistance - This is the security distance to respect on the other lane before changing lane.
	 * Return		: Returns a boolean, true whan the change has been successful and false when the car cannot change lane.
	 */
	bool ChangeLane (float _fltSecurityDistance) {

		// Variables declaration
		Vector3[] _v3LanesPositions = Config.V3_SPAWN_COORDINATES;		// All the lanes positions
		Vector3 _v3CurrentLane		= new Vector3();					// The current lane of the car
		bool _blnChangeZPos			= false;							// When true, allows the car to initiate the check on the right to change lane
		bool _blnChangeZNeg			= false;							// When true, allows the car to initiate the check on the left to change lane
		int _intLaneId				= 0;								// The index of the current lane in the vector 3 array


		// Loop variables
		int i;	// Loop variable

		// Gets the current lane of the car
		for (i = 0; i < _v3LanesPositions.Length; i++) {

			// If the lane is the same as the car z position
			if (_v3LanesPositions[i].z == transform.position.z) {

				// Stores the current lane values
				_v3CurrentLane = _v3LanesPositions[i];
				_intLaneId	   = i;
			}
		}

		// Identificates the avaliable lanes (left, right or both)
		int _intCTempLane = 0; // The current lane refactored for the conditions purpose

		// Refactors the temp lane value
		if (_intLaneId + 1 > _v3LanesPositions.Length / 2)
			_intCTempLane = (_intLaneId + 1) - (_v3LanesPositions.Length / 2);
		else
			_intCTempLane = (_intLaneId + 1);
		

		// If on the higher lane, only allows to translate down (Z-)
		if (_intCTempLane - 1 <= 0 && _intCTempLane + 1 <= _v3LanesPositions.Length / 2) {

			// Sets the lanes possibilities
			_blnChangeZNeg = true;
			_blnChangeZPos = false;
		} else if (_intCTempLane + 1 <= _v3LanesPositions.Length / 2 && _intCTempLane - 1 >= 1) { // If the lane is the middle lane, allows to go left and right (Z+, Z-)

			// Sets the lanes possibilities
			_blnChangeZNeg = true;
			_blnChangeZPos = true;
		} else if (_intCTempLane >= _v3LanesPositions.Length / 2 && _intCTempLane - 1 >= 1) { // On the lowest lane, allows to go only up (Z+)

			// Sets the lanes possibilities
			_blnChangeZNeg = false;
			_blnChangeZPos = true;
		} else { // No possibilities
			
			_blnChangeZNeg = false;
			_blnChangeZPos = false;
		}

		// If the negative option is avaliable, starts the security occupation verification
		if (_blnChangeZNeg) {

			// Initiates the raycast on Z-
			RaycastHit _rhCarInRange;	// This is the car detected in the hit range
			Ray _rRangeDetection = new Ray () {
				direction = Vector3.back,
				origin = new Vector3 (transform.position.x - (_fltSecurityDistance / 2), transform.position.y, _v3CurrentLane.z - 0.2f)
			};	// This is the car deteciont range

			// Executes the raycast
			if (!(Physics.Raycast (_rRangeDetection, out _rhCarInRange, _fltSecurityDistance))) {

				// Initiates the lane change
				StartCoroutine (ChangeLaneTransition (false, transform.position.z, transform.position.z - 0.2f));

				// Returns true
				return true;
			}
		} 

		// If the positive option is avaliable, starts the security occupation verification
		if (_blnChangeZPos) { 

			// Initiates the raycast on Z+
			RaycastHit _rhCarInRange;	// This is the car detected in the hit range
			Ray _rRangeDetection = new Ray () {
				direction = Vector3.back,
				origin = new Vector3 (transform.position.x - (_fltSecurityDistance / 2), transform.position.y, _v3CurrentLane.z + 0.2f)
			};	// This is the car deteciont range

			// Executes the raycast
			if (!(Physics.Raycast (_rRangeDetection, out _rhCarInRange, _fltSecurityDistance))) {

				// Initiates the lane change
				StartCoroutine (ChangeLaneTransition (true, transform.position.z, transform.position.z + 0.2f));

				// Returns true
				return true;
			}
		}

		// If no change possibilities
		if (!(_blnChangeZNeg) && !(_blnChangeZPos)) {

			// No lane change, returns false
			return false;
		}

		// Returns false if this code is reached
		return false;
	}

	/*
	 * Function 	: ChangeLane()
	 * Description  : Moves the car from a lane to another
	 */
	IEnumerator ChangeLaneTransition(bool _blnPositiveTranslate, float _fltActualZPos, float _fltEndZPos) {

		// Variables declaration
		float _fltProgress = _fltActualZPos; // The progress of the translation	

		// Moves positively on the Z 
		if (_blnPositiveTranslate) {

			// Starts the translation
			while (_fltProgress < _fltEndZPos) {

				// Moves the car on the z axis
				transform.Translate(0, 0, +0.0001f);
				_fltProgress += 0.01f;
				yield return new WaitForEndOfFrame();
			}
		} else {

			// Starts the translation
			while (_fltProgress < _fltEndZPos) {

				// Moves the car on the z axis
				transform.Translate(0, 0, -0.00001f);
				_fltProgress -= 0.01f;
				yield return new WaitForEndOfFrame();
			}
		}

		// Puts the car to its final Z location
		//transform.position = new Vector3(transform.position.x, transform.position.y, _fltEndZPos);
	}
}
