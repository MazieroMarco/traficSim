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
		GenerateRoads();

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
			GenerateCars();

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

		// Gets a random car model
		_intCarModel = Random.Range(1, 5);

		// Instanciates a car on the scene
		GameObject _goCar = (GameObject)Instantiate(Resources.Load("Car_0" + _intCarModel));
	}
}
