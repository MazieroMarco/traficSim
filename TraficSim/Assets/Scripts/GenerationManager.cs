using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class 	   : GenerationManager
 * Descirption : Manages all car spawns and roads generation
 */
public class GenerationManager : MonoBehaviour {

	// Variables declaration
	public GameObject _goSceneRoads;	// All the roads in the scene
	public GameObject _goSceneCars;		// All the cars in the scene

	private Vector3[] _v3Coordinates;	// The car spawn coordinates
	private float _fltTimer;			// Timer for the cars spawn

	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Initializes the roads
		GenRoads();

		// Gets the car spawn coordinates
		_v3Coordinates = GetSpawnCoordinates();

		// Initializes the timer
		_fltTimer = 0;
	}
	
	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void FixedUpdate () {

		// Updates the timer
		_fltTimer += Time.deltaTime;

		// If the time passed, spawns new cars
		if (_fltTimer > Config.FLT_CARS_DENSITY_SEC) {

			// Generates the cars
			GenCars(_v3Coordinates);

			// Resets the timer
			_fltTimer = 0;
		}
	}

	/*
	 * Function 	: GenRoads()
	 * Description  : Generates the roads depending on the user preferences
	 */
	void GenRoads () {

		// Variables declaration
		int _intNbRoads  	 = Config.INT_NB_ROADS;  				// The number of roads on each side
		int _intRoadSize 	 = Config.INT_ROAD_SIZE; 				// The size of the road
		int _intNbRoadChilds = _goSceneRoads.transform.childCount;	// Gets the number of child roads

		int i; // Loop variable
		int j; // Loop variable

		// Deletes all the previous roads
		for (i = _intNbRoadChilds - 1; i > 0; i--) {

			// Deletes the road
			GameObject.Destroy(_goSceneRoads.transform.GetChild(i).gameObject);
		}
			
		// Sets the base Z position for the new pieces
		float _fltRoadPieceZ = 0;

		// Creates the new roads on each side
		for (i = 0; i < _intNbRoads * 2; i++) {

			// Defines the position of the current road piece
			int _intRoadX = _intRoadSize / 2 * -1;	// If 10 value equals -5

			// Draws the left side
			for (j = 0; j < _intRoadSize; j++) {

				// Generates the new road piece
				GameObject _goRoadPiece = (GameObject)Instantiate(Resources.Load("Road_01"));

				// Sets the road position an parent object
				_goRoadPiece.transform.position = new Vector3 (_intRoadX, 0, _fltRoadPieceZ);
				_goRoadPiece.transform.parent = _goSceneRoads.transform;

				// Increments the next road placement
				_intRoadX++;
			}

			// Decrements the Z placement for the roads (if half generation, makes the separation for both sides)
			if (i == _intNbRoads - 1) {
				_fltRoadPieceZ -= 0.3f;
			} else {
				_fltRoadPieceZ -= 0.2f;
			}
		}
	}

	/*
	 * Function 	: GetSpawnCoordinates()
	 * Description  : Gets all the spawn coordinates for the cars
	 */
	Vector3[] GetSpawnCoordinates () {

		// Variables declaration
		int _intRoadSize 	 = Config.INT_ROAD_SIZE; 	// The size of the road
		int _intNbRoads  	 = Config.INT_NB_ROADS;  	// The number of roads on each side
		int _intLeftSpawnX   = _intRoadSize / 2 * -1;	// The X coordinate for the car spawns
		int _intRightSpawnX  = _intRoadSize / 2;		// The X coordinate for the car spawns

		Vector3[] _v3Positions;							// Declares the positions array

		// Defines all the car spawn coordinates
		switch (_intNbRoads) {
		case 1:
			// Declares the coordinates array
			_v3Positions = new Vector3[2];

			// Defines the positions
			_v3Positions [0] = new Vector3 (_intRightSpawnX, 0.05f, 0);
			_v3Positions [1] = new Vector3 (_intLeftSpawnX, 0.05f, -0.3f);

			// Returns the array
			return _v3Positions;
			break;

		case 2:
			// Declares the coordinates array
			_v3Positions = new Vector3[4];

			// Defines the positions
			_v3Positions [0] = new Vector3 (_intRightSpawnX, 0.05f, 0);
			_v3Positions [1] = new Vector3 (_intRightSpawnX, 0.05f, -0.2f);

			_v3Positions [2] = new Vector3 (_intLeftSpawnX, 0.05f, -0.5f);
			_v3Positions [3] = new Vector3 (_intLeftSpawnX, 0.05f, -0.7f);

			// Returns the array
			return _v3Positions;
			break;

		case 3:
			// Declares the coordinates array
			_v3Positions = new Vector3[6];

			// Defines the positions
			_v3Positions [0] = new Vector3 (_intRightSpawnX, 0.05f, 0);
			_v3Positions [1] = new Vector3 (_intRightSpawnX, 0.05f, -0.2f);
			_v3Positions [2] = new Vector3 (_intRightSpawnX, 0.05f, -0.4f);

			_v3Positions [3] = new Vector3 (_intLeftSpawnX, 0.05f, -0.7f);
			_v3Positions [4] = new Vector3 (_intLeftSpawnX, 0.05f, -0.9f);
			_v3Positions [5] = new Vector3 (_intLeftSpawnX, 0.05f, -1.1f);

			// Returns the array
			return _v3Positions;
			break;

		default:
			return null;
			break;
		}
	}

	/*
	 * Function 	: GenCars()
	 * Description  : Generates the cars on the roads
	 */
	void GenCars(Vector3[] _v3SpawnPoints) {

		// Variables declaration
		int _intRoadSize 	 = Config.INT_ROAD_SIZE; 	// The size of the road
		int _intNbRoads  	 = Config.INT_NB_ROADS;  	// The number of roads on each side
		int _intLeftSpawnX   = _intRoadSize / 2 * -1;	// The X coordinate for the car spawns
		int _intRightSpawnX  = _intRoadSize / 2;		// The X coordinate for the car spawns

		// Spawns a car in a random lane
		int _intLaneId = Random.Range(0, _v3SpawnPoints.Length); // Gets the random lane

		// Spawns a car on coordinates and orientates it
		GameObject _goCar = (GameObject)Instantiate(Resources.Load("Car_01"));
		_goCar.transform.position = _v3SpawnPoints[_intLaneId];

		// Rotates the car
		if (_v3SpawnPoints[_intLaneId].x == _intLeftSpawnX) {
			_goCar.transform.rotation = Quaternion.Euler(0, 90, 0);

			// Set the car direction
			_goCar.GetComponent<CarBehavior>()._v3CarDirection = Vector3.right;
		} else {
			_goCar.transform.rotation = Quaternion.Euler(0, -90, 0);

			// Set the car direction
			_goCar.GetComponent<CarBehavior>()._v3CarDirection = Vector3.left;
		}
	}
}
