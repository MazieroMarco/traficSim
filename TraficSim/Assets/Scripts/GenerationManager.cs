using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Class 	   : GenerationManager
 * Descirption : Manages all car spawns and roads generation
 */
public class GenerationManager : MonoBehaviour {

	// Variables declaration
	public GameObject _goSceneRoads;	// All the roads in the scene
	public Text _txtCarOutput;			// The text zone for the output

	private Vector3[] _v3Coordinates;	// The car spawn coordinates
	private Vector3 _v3PrevSpawn;		// The last spawn coordinates (To not spawn twice on the same place)
	private float _fltGenerationTimer;	// Timer for the cars spawn
	private float _fltOutputTimer;		// Timer for the output update
	private int[] _intAverageOutputs;	// Array containing the average outputs
	private int _intArrayCount;			// The current place in the average output array


	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Initializes the roads
		GenRoads2();

		// Gets the car spawn coordinates
		_v3Coordinates = GetSpawnCoordinates();

		// Dtores the spawn coordinates in the global array
		Config.V3_SPAWN_COORDINATES = _v3Coordinates;

		// Initializes the timers
		_fltGenerationTimer = 0;
		_fltOutputTimer 	= 0;

		// Initializes the previous spawn point
		_v3PrevSpawn = new Vector3(0, 0, 0);

		// Initializes the output text
		_txtCarOutput.text = "Débit 0 min";

		// Initializes the average outputs array
		_intAverageOutputs = new int[20];
		_intArrayCount 	   = 0;

		// Initializes the values
		for (int i = 0; i < _intAverageOutputs.Length; i++)
			_intAverageOutputs [i] = 0;
			
		// Initializes the update average every second
		InvokeRepeating("UpdateAverageOutput", 0, 1f);

	}
	
	/*
	 * Function 	: FixedUpdate()
	 * Description  : Called every frame
	 */
	void FixedUpdate () {

		// Updates the timers
		_fltGenerationTimer += Time.deltaTime;
		_fltOutputTimer 	+= Time.deltaTime;

		// If the time passed, spawns new cars
		if (_fltGenerationTimer > Config.FLT_CARS_DENSITY_SEC) {

			// Generates the cars
			GenCars(_v3Coordinates);

			// Resets the timer
			_fltGenerationTimer = 0;
		}

		// If the time passed, updates the output text


		if (_fltOutputTimer > 2) {

			// Gets average number of cars that passed during one second
			float _fltCarsAverage = 0;

			foreach (int _nbCars in _intAverageOutputs) {
				_fltCarsAverage += _nbCars;
				//Debug.Log (_nbCars);
			}

			// Gets the average
			_fltCarsAverage = _fltCarsAverage / _intAverageOutputs.Length * 60;

			// Updates the output text
			_txtCarOutput.text = "Débit : " + _fltCarsAverage + " min";

			// Resets the timer
			_fltOutputTimer = 0;
		}
	}

	/*
	 * Function 	: UpdateAverageOutput()
	 * Description  : Updates the average output. Places the amount of cars that passed during one second in the output array
	 */
	void UpdateAverageOutput () {
		
		// Puts the value in the array
		_intAverageOutputs[_intArrayCount] = Config.INT_CARS_OUTPUT;

		// Updates the array count
		_intArrayCount += 1;

		// Resets the cars count
		Config.INT_CARS_OUTPUT = 0;

		// Resets the array count if max value
		if (_intArrayCount == _intAverageOutputs.Length)
			_intArrayCount = 0;
	}

	void GenRoads2 () {
		new Road (true);
		new Road (true);
		new Road (false);
		new Road (false);

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
			int _intRoadX = _intRoadSize / 2 * - 1;	// If 10 value equals -5

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
		int _intLaneId		 = 0;						// The lane id where the next car will spawn
		int _intCarModel	 = 0;						// The random number of the car model

		// Checks if the spawn point is not the same as the previous one
		do {
			// Spawns a car in a random lane
			_intLaneId = Random.Range (0, _v3SpawnPoints.Length); // Gets the random lane

		} while (_v3SpawnPoints [_intLaneId] == _v3PrevSpawn);

		// Gets the random car model
		_intCarModel = Random.Range(1, 5);

		// Spawns a car on coordinates and orientates it
		GameObject _goCar = (GameObject)Instantiate(Resources.Load("Car_0" + _intCarModel));

		// Gets the high position of the car and applies it
		float _fltCarHigh = _goCar.transform.position.y;
		_goCar.transform.position = _v3SpawnPoints[_intLaneId];
		_goCar.transform.position = new Vector3 (_goCar.transform.position.x, _fltCarHigh, _goCar.transform.position.z);

		// Rotates the car
		if (_v3SpawnPoints[_intLaneId].x == _intLeftSpawnX) {

			// Rotates
			_goCar.transform.rotation = Quaternion.Euler(_goCar.transform.rotation.x, 90, _goCar.transform.rotation.z);

			// Set the car direction
			_goCar.GetComponent<CarBehavior>()._v3CarDirection = Vector3.right;
		} else {

			// Rotates
			_goCar.transform.rotation = Quaternion.Euler(_goCar.transform.rotation.x, -90, _goCar.transform.rotation.z);

			// Set the car direction
			_goCar.GetComponent<CarBehavior>()._v3CarDirection = Vector3.left;
		}
	}
}
