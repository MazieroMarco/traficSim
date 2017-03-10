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
	public Road _rdCarRoad;								// The current car road
	public bool _blnCarProblem = false;					// Used to stop the car on the road
	public float _fltCarInitialSpeed;					// The initial speed before detections (used during the detections)
	public bool _blnIsChangingLane = false;				// Used when changing lane to avoid collision with other cars

	// Private variables declaration
	private int _intRandomRoad;							// The random road index to generate the car
	private int _intSlowCarsCounter = 0;				// The slow cars counter

	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Variables declaration
		bool _blnPossibleSpawn = false;

		// Initializes the random speed factor
		_fltRandomSpeed = Random.Range(Config.FLT_DRIVERS_SPEED_FACTOR_KMH * -1, Config.FLT_DRIVERS_SPEED_FACTOR_KMH);

		// Intializes the total car/truck speed
		if (this.tag == "Truck" && Config.INT_SPEED_LIMIT_KMH > 80) {

			// Updates the speed for the truck
			_fltCarInitialSpeed = ((_fltRandomSpeed + Config.INT_SPEED_LIMIT_KMH_TRUCK) * 100 / 60 / 60);
		} else {

			// Updates the speed for the car
			_fltCarInitialSpeed = ((_fltRandomSpeed + Config.INT_SPEED_LIMIT_KMH) * 100 / 60 / 60);
		}

		// Sets the initial speed
		_fltCarSpeed = _fltCarInitialSpeed;

		// Random index list
		List<int> _intIndexList = new List<int>();

		// Gets a random road and checks if there's a place to spawn
		for (int i = 0; i < Config.LI_GAME_ROADS.Count; i++) {

			do {
			// Generate random value
			_intRandomRoad = Random.Range(0, Config.LI_GAME_ROADS.Count);
			
			} while (_intIndexList.Contains(_intRandomRoad));

			// Adds the value to the list
			_intIndexList.Add (_intRandomRoad);

			// Gets the temp car direction
			_v3CarDirection = Config.LI_GAME_ROADS[_intRandomRoad]._blnDirectionRight ? Vector3.right : Vector3.left;

			// Stores the initial Y axis (To avoid spawn on the ground)
			float _carYValue = transform.position.y;

			// Places the car to the start position
			transform.position = Config.LI_GAME_ROADS[_intRandomRoad].GetSpawnOrigin() - _v3CarDirection;

			// Sets the car Y axis value (To avoid spawn on the ground)
			transform.Translate(0, _carYValue, 0);

			// Rotates the car
			transform.rotation = Config.LI_GAME_ROADS[_intRandomRoad]._blnDirectionRight ? Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z) : Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);

			// Car in the range
			RaycastHit _rhCarInRange;	// This is the car detected in the hit range

			// Checks the presence of other cars with a raycast
			Ray _rRangeDetection = new Ray () {direction = _v3CarDirection, origin = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z)};

			// Executes the raycast
			if (Physics.Raycast ( _rRangeDetection, out _rhCarInRange, 3f)) {

				if (_rhCarInRange.distance > 1.5f) {

					// No car detected in 3m, spawn possible
					_blnPossibleSpawn = true;

					// Sets the car speed
					_fltCarSpeed = _rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed;
					_fltCarSpeed = _fltCarSpeed > _fltCarInitialSpeed ? _fltCarInitialSpeed : _fltCarSpeed;
					break;
				}
			} else {

				// No car detected, spawn possible
				_blnPossibleSpawn = true;
				break;
			}
		}

		// Destroys the car if the spawn is false
		if (!_blnPossibleSpawn) {
			DestroyImmediate (this.gameObject);
			return;
		} else {

			// Places the car to the start point
			transform.position = new Vector3 (transform.position.x + _v3CarDirection.x / 1.5f, transform.position.y, transform.position.z);
		}

		// Adds the car to the road
		Config.LI_GAME_ROADS[_intRandomRoad]._liCars.Add(this);

		// Sets the car roaa variable
		_rdCarRoad = Config.LI_GAME_ROADS[_intRandomRoad];

		// Starts the breakdown check
		InvokeRepeating("BreakdownCheck", 0, 1f);
	}
	
	/*
	 * Function 	: FixedUpdate()
	 * Description  : Called every frame
	 */
	void FixedUpdate () {
		
		// Stops the car if it has a problem and activates the global car accidents bool
		if (_blnCarProblem) {
			
			_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
			Config.BLN_CAR_PROBLEM_ICON = true;
		}
		
		// Moves the car
		CarMoveForward();

		// If the car is not changing lane
		if (!_blnIsChangingLane) {

			// Checks for collisions
			CarCollisionsCheck ();

			// Check for destroy
			CarDestroyOnLimit();
		}

		// Updates the initial speed (in case of menu change), Limits the speed to 80 if the vehicle is a truck
		if (this.tag == "Truck" && Config.INT_SPEED_LIMIT_KMH > Config.INT_SPEED_LIMIT_KMH_TRUCK) {

			// Updates the speed for the truck
			_fltCarInitialSpeed = ((_fltRandomSpeed + Config.INT_SPEED_LIMIT_KMH_TRUCK) * 100 / 60 / 60);
		} else {

			// Updates the speed for the car
			_fltCarInitialSpeed = ((_fltRandomSpeed + Config.INT_SPEED_LIMIT_KMH) * 100 / 60 / 60);
		}
	}

	/*
	 * Function 	: OnTriggerEnter()
	 * Description  : Is called each time the car collider collides with something.
	 */
	void OnTriggerEnter(Collider _coCollider)
	{
		// The car collided something, checks if it's a car or a truck and generates an accident
		if (_coCollider.gameObject.tag == "Car" || _coCollider.gameObject.tag == "Truck" || _coCollider.gameObject.tag == "Obstacle")
			CarAccident (_coCollider);
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

		// If the user has the control returns immediately
		if (Config.BLN_CAR_CONTROL && this == GameObject.Find("SceneScripts").GetComponent<CameraBehavior>()._cbRandomCar)
			return;
		
		// If the car finishes the road
		if ((_rdCarRoad._blnDirectionRight && transform.position.x > _rdCarRoad.GetEndOfTheRoad().x) || (!_rdCarRoad._blnDirectionRight && transform.position.x < _rdCarRoad.GetEndOfTheRoad().x)) {

			// Updates the car output counter
			if (_rdCarRoad._blnDirectionRight)
				Config.INT_CARS_OUTPUT_RIGHT += 1;
			else
				Config.INT_CARS_OUTPUT_LEFT += 1;

			// Removes the car from the list
			_rdCarRoad._liCars.Remove(_rdCarRoad._liCars.FirstOrDefault(a=>a==this));

			// Destroys the current car
			Destroy(gameObject);
		}
	}

	/*
	 * Function 	: BreakdownCheck()
	 * Description  : Calculates the chances of having a breakdown
	 */
	void BreakdownCheck () {

		// Checks the chances
		var rand = Random.Range (0f, 100f) ;

		if (rand < Config.FLT_BREAKDOWN_CHANCES) {
			_blnCarProblem = true;
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
		float _fltDetectionDist = (3 * (_fltCarSpeed * 60 * 60 / 100)) / 10 / 10 * Config.FLT_SECURITY_DIST_FACTOR; //Config.FLT_AHEAD_CAR_DETECTION_DIST;
		Ray _rRangeDetection = new Ray (){direction = _v3CarDirection, origin = transform.position};	// This is the car deteciont range

		// Sets the raycast y value for the trucks
		if (this.tag == "Truck")
			_rRangeDetection = new Ray (){direction = _v3CarDirection, origin = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z)};
		
		////////////////////////////////////////////////////////////
		/// Accidents and slow cars detection
		////////////////////////////////////////////////////////////
		Collider[] _coHitColliders = Physics.OverlapSphere(transform.position, 6f);

		for (int i = 0; i < _coHitColliders.Length; i++) {

			// Checks if the collider is a car or a truck
			if (_coHitColliders [i].gameObject.tag == "Car" || _coHitColliders [i].gameObject.tag == "Truck") {

				// Checks the car / truck speed
				if (_coHitColliders [i].gameObject.GetComponent<CarBehavior> ()._fltCarSpeed <= 0 && _coHitColliders [i].gameObject.GetComponent<CarBehavior> ()._v3CarDirection == _v3CarDirection)
					_intSlowCarsCounter++;
				else
					_intSlowCarsCounter--;

				// Checks if the collider has an accident
				if (_coHitColliders [i].gameObject.GetComponent<CarBehavior> ()._blnCarProblem && _coHitColliders [i].gameObject.GetComponent<CarBehavior> ()._v3CarDirection == _v3CarDirection || _intSlowCarsCounter >= 5) {

					// Slows down the current car
					if (_fltCarSpeed > _fltCarInitialSpeed / 2)
						_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
				}
			}
		}

		////////////////////////////////////////////////////////////
		/// Detection Raycast
		////////////////////////////////////////////////////////////
		// Casts the detection zone to find cars ahead. It returns trus if detection
		if (Physics.Raycast (_rRangeDetection, out _rhCarInRange, _fltDetectionDist + 0.5f)) {

			// Checks if the collision is a car
			if (_rhCarInRange.transform.gameObject.GetComponent<CarBehavior> () == null) {

				// Checks if it's a obstacle
				if (_rhCarInRange.transform.gameObject.tag == "Obstacle") {

					// Slows down the car
					_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;

					// Tries to change lane
					if (_fltCarSpeed < _fltCarInitialSpeed / 1.5)
						ChangeLane (Config.FLT_SECURITY_DIST_CHANGE_LANE / 1.8f);

					// Returns
					return;

				} else {

					// Return
					return;
				}
			}

			// Tries to change lane if the car is stopped
			if (_fltCarSpeed <= _fltCarInitialSpeed / 3) ChangeLane (Config.FLT_SECURITY_DIST_CHANGE_LANE / 1.8f);

			////////////////////////////////////////////////////////////
			/// Distance detections
			////////////////////////////////////////////////////////////
			if (_rhCarInRange.distance < _fltDetectionDist / 3) { // Between 0 and 3 meters

				// Slows down the car to avoid collision
				_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED * 2;

			} else if (_rhCarInRange.distance > _fltDetectionDist / 3 && _rhCarInRange.distance < _fltDetectionDist / 1.875f) { // Between 3 and 6 meters

				// Slows down the car to the detected car speed to avoid collision
				if (_fltCarSpeed > _rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed) {

					// Slows down the car
					_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
				}

			} else if (_rhCarInRange.distance > _fltDetectionDist / 1.5f && _rhCarInRange.distance < _fltDetectionDist) { // Between 8 and 10 meters
				
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
			if (_rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed < _fltCarSpeed / _fltDetectionDist && _fltCarSpeed > _fltDetectionDist / 5) {

				// Already slows down the car
				_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
			}

			// Checks to change the lane
			if (_rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed < _fltCarInitialSpeed)
				ChangeLane (Config.FLT_SECURITY_DIST_CHANGE_LANE);

		} else {

			// If the car speed is lower thant the initial, speeds it up
			if (_fltCarSpeed < _fltCarInitialSpeed) {

				// Speeds up the car
				_fltCarSpeed += Config.FLT_DRIVER_ACCELERATION_SPEED;
			}
		}
	}

	/*
	 * Function 	: CarAccident()
	 * Description  : Called when a car collides with another. It creates an accident.
	 */
	void CarAccident (Collider _coVehicleCollided) {

		// Stops the vehicle if the vehicle is not curently changing lane
		_fltCarSpeed = 0;
		_blnCarProblem = true;

		// Places the smoke on the impact
		if(_coVehicleCollided.gameObject.tag != "Obstacle" && _coVehicleCollided.gameObject.GetComponent<CarBehavior>()._blnCarProblem) return;
		GameObject _goSmoke = GameObject.Instantiate(Resources.Load<GameObject>("accident_smoke"));
		_goSmoke.transform.position = new Vector3(_coVehicleCollided.ClosestPointOnBounds(transform.position).x, transform.position.y, _coVehicleCollided.ClosestPointOnBounds(transform.position).z) ;
	}

	/*
	 * Function 	: ChangeLane()
	 * Description  : When called, the car will try to change his lane on the road.
	 * 
	 * Parameters   : float _fltSecurityDistance - This is the security distance to respect on the other lane before changing lane.
	 * Return		: Returns a boolean, true whan the change has been successful and false when the car cannot change lane.
	 */
	bool ChangeLane (float _fltSecurityDistance) {

		// If the car has an accident or breakdown, doesn't change
		if (_blnCarProblem)
			return false;

		// Checks if another car is changing lane around
		Collider[] _coHitColliders = Physics.OverlapSphere(transform.position, 0.8f);

		for (int i = 0; i < _coHitColliders.Length; i++) {

			// Checks if the collider is a car or a truck
			if (_coHitColliders [i].gameObject.tag == "Car" || _coHitColliders [i].gameObject.tag == "Truck") {

				// Checks if the collider is already changing lane or has an accident
				if (_coHitColliders [i].gameObject.GetComponent<CarBehavior> ()._blnIsChangingLane)
					return false;
			}
		}

		// Checks if the car is far enough from the spawn point
		if (_v3CarDirection == Vector3.left) {

			// Left lane
			if (transform.position.x + _fltSecurityDistance >= _rdCarRoad.GetSpawnOrigin().x)
				return false;
		} else {

			// Right lane
			if (transform.position.x - _fltSecurityDistance <= _rdCarRoad.GetSpawnOrigin().x)
				return false;
		}

		// Variables declaration
		bool _blnTurnPos = false;	// If true, it's possible to translate on the Z+ axis
		bool _blnTurnNeg = false;	// If true, it's possible to translate on the Z- axis
		int _intCurrentRoadIndex;	// The current road index

		// Gets all the roads that go to the same direction
		var _liRoads = Config.LI_GAME_ROADS.Where(a=>a._blnDirectionRight==_rdCarRoad._blnDirectionRight).ToList();

		// Gets the index of the current car road
		_intCurrentRoadIndex = _liRoads.FindIndex(a=> a._intRoadID==_rdCarRoad._intRoadID);

		// Sets the possibilities booleans
		if (_intCurrentRoadIndex + 1 < _liRoads.Count)
			_blnTurnNeg = true;

		if (_intCurrentRoadIndex - 1 >= 0)
			_blnTurnPos = true;


		// Checks to turn on Z-
		if (_blnTurnNeg) {

			// The road where to change
			Road _rdRoadToChange = _liRoads [_intCurrentRoadIndex + 1];

			// Checks if there are cars on the adjacent road
			if (!(_rdRoadToChange._liCars.Where (a =>IsBetweenLimits(transform.position.x, a.transform.position.x, _fltSecurityDistance))).Any()) {

				// Checks if there're no obstacles
				if (!(GameObject.FindGameObjectsWithTag ("Obstacle").Where (a => a.transform.position.z == _rdRoadToChange.GetEndOfTheRoad().z && IsBetweenLimits (transform.position.x, a.transform.position.x, _fltSecurityDistance)).Any ())) {

					// Initiates the lane change
					StartCoroutine (ChangeLaneTransition (false, _rdRoadToChange));

					// Returns true
					return true;
				}
			}
		}

		// Checks to turn on Z+
		if (_blnTurnPos) {

			// The road where to change
			Road _rdRoadToChange = _liRoads [_intCurrentRoadIndex - 1];

			// Checks if there are cars on the adjacent road
			if (!(_rdRoadToChange._liCars.Where (a =>IsBetweenLimits(transform.position.x, a.transform.position.x, _fltSecurityDistance))).Any()) {

				// Checks if there're no obstacles
				if (!(GameObject.FindGameObjectsWithTag ("Obstacle").Where (a => a.transform.position.z == _rdRoadToChange.GetEndOfTheRoad().z && IsBetweenLimits (transform.position.x, a.transform.position.x, _fltSecurityDistance)).Any ())) {
					
					// Initiates the lane change
					StartCoroutine (ChangeLaneTransition (true, _rdRoadToChange));

					// Returns true
					return true;
				}
			}
		}

		// Returns false if this code is reached
		return false;
	}

	/*
	 * Function 	: IsBetweenLimits()
	 * Description  : Checks if a position is between the two global limits.
	 */
	bool IsBetweenLimits(float _fltCurrentXPosition, float _fltSecondXposition, float _fltDistance) {

		// Checks if the position is in the range
		return _fltCurrentXPosition + _fltDistance > _fltSecondXposition && _fltCurrentXPosition - _fltDistance < _fltSecondXposition;
	}


	/*
	 * Function 	: ChangeLaneTransition()
	 * Description  : Moves the car from a lane to another
	 */
	IEnumerator ChangeLaneTransition(bool _blnPositiveTranslate, Road _rdToChange) {

		// Starts the graphic lane change
		_blnIsChangingLane = true;

		// Gets the final Z position
		float _fltEndZPos = _rdToChange.GetSpawnOrigin().z;

		// If the car has no forward speed, adds just a little bit of it
		if (_fltCarSpeed <= 0) _fltCarSpeed = 1f;

		// Variables declaration
		float _fltmoveSpeed = 0.029f;

		// If changing on Z+
		if (_blnPositiveTranslate) {

			// Moves the car on Z+
			while (transform.position.z < _fltEndZPos) {

				// Checks if breakdown
				if (_blnCarProblem) break;

				// Speeds the car during the change
				_fltCarSpeed += Config.FLT_DRIVER_ACCELERATION_SPEED * 3f;
				_fltCarSpeed = _fltCarSpeed > _fltCarInitialSpeed ? _fltCarInitialSpeed : _fltCarSpeed;

				// Translates
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + _fltmoveSpeed);

				// Wait end of frame
				yield return new WaitForEndOfFrame();
			}

		} else { // If changing on Z-

			// Moves the car on Z-
			while (transform.position.z > _fltEndZPos) {

				// Checks if breakdown
				if (_blnCarProblem) break;

				// Speeds the car during the change
				_fltCarSpeed += Config.FLT_DRIVER_ACCELERATION_SPEED * 3f;
				_fltCarSpeed = _fltCarSpeed > _fltCarInitialSpeed ? _fltCarInitialSpeed : _fltCarSpeed;

				// Translates
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - _fltmoveSpeed);

				// Wait end of frame
				yield return new WaitForEndOfFrame();
			}
		}

		// Puts the car to its final Z location
		if (!_blnCarProblem) transform.position = new Vector3(transform.position.x, transform.position.y, _fltEndZPos);

		// Changes the car in the car road list
		_rdCarRoad._liCars.Remove(this);
		_rdToChange._liCars.Add (this);
		_rdCarRoad = _rdToChange;

		// Ends the graphic lane changing
		_blnIsChangingLane = false;
	}
}
