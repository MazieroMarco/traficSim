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

	private float _fltCarInitialSpeed;				// The initial speed before detections (used during the detections)


	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Initializes the random speed factor
		_fltRandomSpeed = Random.Range(Config.FLT_DRIVERS_SPEED_FACTOR * -1, Config.FLT_DRIVERS_SPEED_FACTOR);

		// Intializes the total car speed
		_fltCarSpeed = (Config.INT_SPEED_LIMIT_KMH * 100 / 60 / 60) + _fltRandomSpeed;

		// Sets the initial speed
		_fltCarInitialSpeed = _fltCarSpeed;
	}
	
	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void FixedUpdate () {

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

		// Moves the car forward
		transform.Translate(new Vector3(0, 0, _fltCarSpeed) * Time.deltaTime);
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
	 * 
	 */
	void CarCollisionsCheck () {

		// Variables declaration
		RaycastHit _rhCarInRange;	// This is the car detected in the hit range
		Ray _rRangeDetection = new Ray (){direction = _v3CarDirection, origin = transform.position};	// This is the car deteciont range

		// Casts the detection zone to find cars ahead. It returns trus if detection
		if (Physics.Raycast (_rRangeDetection, out _rhCarInRange, Config.FLT_AHEAD_CAR_DETECTION_DIST)) {

			// Checks if the current speed is higher than the hit car speed
			if (_fltCarSpeed > _rhCarInRange.transform.gameObject.GetComponent<CarBehavior> ()._fltCarSpeed && _rhCarInRange.distance < Config.FLT_DRIVER_SAFETY_DIST) {

				// If it's higher, slows the current speed
				_fltCarSpeed -= Config.FLT_DRIVER_DECELERATION_SPEED;
			} else if (_fltCarSpeed < _fltCarInitialSpeed && _rhCarInRange.distance > Config.FLT_DRIVER_SAFETY_DIST) {

				// Else if, speeds up the car a little bit while the speed is lower than the global
				_fltCarSpeed += Config.FLT_DRIVER_ACCELERATION_SPEED / 4;
			}
		} else if (_fltCarSpeed < _fltCarInitialSpeed) {

			// Else if, speeds up the car while the speed is lower than the global
			_fltCarSpeed += Config.FLT_DRIVER_ACCELERATION_SPEED;
		}
	}
}
