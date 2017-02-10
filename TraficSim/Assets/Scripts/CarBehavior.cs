using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Class 	   : CarBehavior
 * Descirption : Manages all the diferent cars behaviors
 */
public class CarBehavior : MonoBehaviour {

	// Public variables declaration
	public float _fltRandomSpeed;						// The custom speed factor of the car
	public float _fltCarSpeed;							// The current total speed of the car
	public Vector3 _v3CarDirection = Vector3.right;		// The current direction of the car

	public bool PROBLEM = false;

	// Private variables declaration
	private float _fltCarInitialSpeed;					// The initial speed before detections (used during the detections)
	private int _intRandomRoad;							// The random road index to generate the car
	private Road _rdCarRoad;							// The current car road

	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Variables declaration
		bool _blnPossibleSpawn = false;

		// Sets the car tag
		this.tag = "Car";

		// Gets a random road and checks if there's a place to spawn
		do {

			// Generate random value
			_intRandomRoad = Random.Range(0, Config.LI_GAME_ROADS.Count);

			// Gets the temp car direction
			_v3CarDirection = Config.LI_GAME_ROADS[_intRandomRoad]._blnDirectionRight ? Vector3.right : Vector3.left;

			// Checks the presence of other cars with a raycast
			Ray _rRangeDetection = new Ray () {direction = _v3CarDirection, origin = Config.LI_GAME_ROADS[_intRandomRoad].GetSpawnOrigin()};
			Debug.DrawRay(Config.LI_GAME_ROADS[_intRandomRoad].GetSpawnOrigin(), _v3CarDirection);
			// Executes the raycast
			if (!(Physics.Raycast (_rRangeDetection, 0.3f))) {

				// No car detected, spawn possible
				_blnPossibleSpawn = true;
			}

		} while (!_blnPossibleSpawn);


		// Stores the initial Y axis (To avoid spawn on the ground)
		float _carYValue = transform.position.y;

		// Places the car to the start position
		transform.position = Config.LI_GAME_ROADS[_intRandomRoad].GetSpawnOrigin();

		// Sets the car Y axis value (To avoid spawn on the ground)
		transform.Translate(0, _carYValue, 0);

		// Rotates the car
		transform.rotation = Config.LI_GAME_ROADS[_intRandomRoad]._blnDirectionRight ? Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z) : Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);

		// Sets the car direction
		_v3CarDirection = Config.LI_GAME_ROADS[_intRandomRoad]._blnDirectionRight ? Vector3.right : Vector3.left;

		// Adds the car to the road
		Config.LI_GAME_ROADS[_intRandomRoad]._liCars.Add(this);

		// Sets the car roaa variable
		_rdCarRoad = Config.LI_GAME_ROADS[_intRandomRoad];

		// Initializes the random speed factor
		_fltRandomSpeed = Random.Range(Config.FLT_DRIVERS_SPEED_FACTOR_KMH * -1, Config.FLT_DRIVERS_SPEED_FACTOR_KMH);

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

		// Updates the initial speed (in case of menu change)
		_fltCarInitialSpeed = ((_fltRandomSpeed + Config.INT_SPEED_LIMIT_KMH) * 100 / 60 / 60);
	}

	/*
	 * Function 	: CarMoveForward()
	 * Description  : Moves the car forward on the road lane
	 */
	void CarMoveForward () {

		// Puts the car speed to 0 if under 0 and tries to change lane
		if (_fltCarSpeed <= 0) {

			// Puts to 0
			_fltCarSpeed = 0;

			if (!PROBLEM && Random.Range(1, 50) == 2)
				ChangeLane (Config.FLT_SECURITY_DIST_CHANGE_LANE);
		}
			

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

		// If the car finishes the road
		if ((_rdCarRoad._blnDirectionRight && transform.position.x > _rdCarRoad.GetEndOfTheRoad().x) || (!_rdCarRoad._blnDirectionRight && transform.position.x < _rdCarRoad.GetEndOfTheRoad().x)) {

			// Removes the car from the list
			_rdCarRoad._liCars.Remove(_rdCarRoad._liCars.FirstOrDefault(a=>a==this));

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
			if (_rhCarInRange.distance < 0.3f) { // Between 0 and 3 meters

				// Slows down the car to avoid collision
				_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED * 2;
				//_fltCarSpeed -= _fltCarSpeed >_rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed ? Config.FLT_DRIVER_DECELERATION_SPEED * 2 :0 ;

			} else if (_rhCarInRange.distance > 0.3f && _rhCarInRange.distance < 0.6f) { // Between 3 and 6 meters

				// Slows down the car to the detected car speed to avoid collision
				if (_fltCarSpeed > _rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed) {

					// Slows down the car
					_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
				}

			} else if (_rhCarInRange.distance > 0.6f && _rhCarInRange.distance < 0.8f) { // Between 6 and 8 meters

				// Tries to change lane if the front car is slower than the current car original speed
				if (_rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed < _fltCarSpeed)
					ChangeLane (Config.FLT_SECURITY_DIST_CHANGE_LANE);

			} else if (_rhCarInRange.distance > 0.8f && _rhCarInRange.distance < 1f) { // Between 8 and 10 meters

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
			if (_rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed < _fltCarSpeed / 1.5 && _fltCarSpeed > 0.3f) {

				// Already slows down the car
				_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
			}
			//_fltCarSpeed -= (2f - _rhCarInRange.distance) / 25; //Mathf.(_rhCarInRange.distance,1.1);

		} else {

			// If the car speed is lower thant the initial, speeds it up
			if (_fltCarSpeed < _fltCarInitialSpeed) {

				// Speeds up the car
				_fltCarSpeed += Config.FLT_DRIVER_ACCELERATION_SPEED;
			}
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

		// If the car is already moving doesn't try to change lane again
		if (_rdCarRoad.GetSpawnOrigin ().z != transform.position.z)
			return false;

		// Variables declaration
		bool _blnTurnPos = false;	// If true, it's possible to translate on the Z+ axis
		bool _blnTurnNeg = false;	// If true, it's possible to translate on the Z- axis
		int _intCurrentRoadIndex;	// The current road index

		// Gets all the roads that go to the same direction
		var _liRoads = Config.LI_GAME_ROADS.Where(a=>a._blnDirectionRight==_rdCarRoad._blnDirectionRight).ToList();

		// Gets the index of the current car road
		_intCurrentRoadIndex = _liRoads.FindIndex(a=> a._intRoadID==_rdCarRoad._intRoadID);

		//Debug.Log (_intCurrentRoadIndex);
		// Sets the possibilities booleans
		if (_intCurrentRoadIndex + 1 < _liRoads.Count)
			_blnTurnNeg = true;

		if (_intCurrentRoadIndex - 1 >= 0)
			_blnTurnPos = true;

		// Checks the Z- transltion possibility
		if (_blnTurnNeg) {

			// Gets the adjacent road Z axis value
			float _fltAdjZ = _liRoads[_intCurrentRoadIndex + 1].GetSpawnOrigin().z;

			// Initiates the raycast on Z-
			float _fltRayXPos = _v3CarDirection == Vector3.left ? transform.position.x + _fltSecurityDistance - 0.5f: transform.position.x - _fltSecurityDistance + 0.5f; // The X position of the ray
			RaycastHit _rhCarInRange;	// This is the car detected in the hit range
			Ray _rRangeDetection = new Ray () {direction = Vector3.right, origin = new Vector3 (_fltRayXPos, transform.position.y, _fltAdjZ)};

			// Executes the raycast
			if (!(Physics.Raycast (_rRangeDetection, out _rhCarInRange, _fltSecurityDistance))) {

				// Initiates the lane change
				StartCoroutine (ChangeLaneTransition (false, _fltAdjZ));

				// Changes the car in the car road list
				_rdCarRoad._liCars.Remove(this);
				_liRoads [_intCurrentRoadIndex + 1]._liCars.Add (this);
				_rdCarRoad = _liRoads [_intCurrentRoadIndex + 1];

				// Returns true
				return true;
			}
		}

		// Checks the Z+ transltion possibility
		if (_blnTurnPos) {

			// Gets the adjacent road Z axis value
			float _fltAdjZ = _liRoads[_intCurrentRoadIndex - 1].GetSpawnOrigin().z;

			// Initiates the raycast on Z-
			float _fltRayXPos = _v3CarDirection == Vector3.left ? transform.position.x + _fltSecurityDistance - 0.5f: transform.position.x - _fltSecurityDistance + 0.5f; // The X position of the ray
			RaycastHit _rhCarInRange;	// This is the car detected in the hit range
			Ray _rRangeDetection = new Ray () {direction = Vector3.right, origin = new Vector3 (_fltRayXPos, transform.position.y, _fltAdjZ)};

			// Executes the raycast
			if (!(Physics.Raycast (_rRangeDetection, out _rhCarInRange, _fltSecurityDistance))) {

				// Initiates the lane change
				StartCoroutine (ChangeLaneTransition (true, _fltAdjZ));

				// Changes the car in the car road list
				_rdCarRoad._liCars.Remove(this);
				_liRoads [_intCurrentRoadIndex - 1]._liCars.Add (this);
				_rdCarRoad = _liRoads [_intCurrentRoadIndex - 1];

				// Returns true
				return true;
			}
		}

		// If no change possibilities
		if (!_blnTurnNeg && !_blnTurnPos) {

			// No lane change, returns false
			return false;
		}

		// Returns false if this code is reached
		return false;
	}

	/*
	 * Function 	: ChangeLaneTransition()
	 * Description  : Moves the car from a lane to another
	 */
	IEnumerator ChangeLaneTransition(bool _blnPositiveTranslate, float _fltEndZPos) {

		// If the car has no forward speed, adds just a little bit of it
		if (_fltCarSpeed <= 0)
			_fltCarSpeed = 1.5f;
		
		// Variables declaration
		float _fltmoveSpeed = 0.01f;

		// If changing on Z+
		if (_blnPositiveTranslate) {

			// Moves the car on Z+
			while (transform.position.z < _fltEndZPos) {

				// Translates
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + _fltmoveSpeed);

				// Wait end of frame
				yield return new WaitForEndOfFrame();
			}

		} else { // If changing on Z-

			// Moves the car on Z-
			while (transform.position.z > _fltEndZPos) {

				// Translates
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - _fltmoveSpeed);

				// Wait end of frame
				yield return new WaitForEndOfFrame();
			}
		}

		// Puts the car to its final Z location
		transform.position = new Vector3(transform.position.x, transform.position.y, _fltEndZPos);

	}
}
