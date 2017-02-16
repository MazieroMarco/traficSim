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

	private Vector3[] _v3Coordinates;	// The car spawn coordinates
	private float _fltGenerationTimer;	// Timer for the cars spawn
	private int _intArrayCount;			// The current place in the average output array


	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Initializes the roads
		GenerateRoads();

		// Initializes the timers
		_fltGenerationTimer = 0;
	}
	
	/*
	 * Function 	: FixedUpdate()
	 * Description  : Called every frame
	 */
	void FixedUpdate () {

		// Updates the timers
		_fltGenerationTimer += Time.deltaTime;

		// If the time passed, spawns new cars
		if (_fltGenerationTimer >= Config.FLT_CARS_DENSITY_SEC) {

			// Generates the cars
			GenerateCars();

			// Resets the timer
			_fltGenerationTimer = 0;
		}
	}

	/*
	 * Function 	: GenerateRoads()
	 * Description  : Generates the roads depending on the user preferences
	 */
	void GenerateRoads () {

		// Generates the left direction roads
		for (int i = 0; i < Config.INT_NB_ROADS; i++)
			new Road (false);

		// Generates the right direction roads
		for (int i = 0; i < Config.INT_NB_ROADS; i++)
			new Road (true);
	}

	/*
	 * Function 	: GenerateCars()
	 * Description  : Generates the cars on the roads
	 */
	void GenerateCars() {

		// Variables declaration
		int _intCarModel	 = 0;	// The random number of the car model
		int _intVehicleType  = 0;	// The type of the vehicle (Car, truck, ...)

		// Gets the vehicle type
		_intVehicleType = Random.Range(0, 100);

		// Generates a car or a truck
		if (_intVehicleType < Config.INT_TRUCK_DENSITY) {

			// Gets a random truck model
			_intCarModel = Random.Range(1, 5);

			// Instanciates a car on the scene
			GameObject.Instantiate(Resources.Load("Truck_0" + _intCarModel));
		} else {

			// Gets a random car model
			_intCarModel = Random.Range(1, 5);

			// Instanciates a car on the scene
			GameObject.Instantiate(Resources.Load("Car_0" + _intCarModel));
		}
	}
}
