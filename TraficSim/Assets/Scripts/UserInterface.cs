using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
/*
 * Class 	   : UserInterface
 * Descirption : This is all the user configuration interface class
 */
public class UserInterface : MonoBehaviour {

	// Variables declaration
	private float _fltOutputTimer;			// Timer for the output update
	private Text _txtCarOutputLeft;			// The left output
	private Text _txtCarOutputRight;		// The right output
	private int[] _intAverageOutputsLeft;	// Array containing the average outputs
	private int _intArrayCountLeft;			// The current place in the average output array
	private int[] _intAverageOutputsRight;	// Array containing the average outputs
	private int _intArrayCountRight;		// The current place in the average output array
	private bool _blnIsGraphsActive;		// To show if the graphs interface is active

	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Initializes the timers
		_fltOutputTimer = 0;

		// Initialize the variables
		_blnIsGraphsActive = false;

		// Initializes the outputs text
		_txtCarOutputLeft 		= GameObject.Find("carOutputLeft").GetComponent<Text>();
		_txtCarOutputRight 		= GameObject.Find("carOutputRight").GetComponent<Text>();
		_txtCarOutputLeft.text  = "<< Débit : 0 /min";
		_txtCarOutputRight.text = ">> Débit : 0 /min";

		// Initializes the average outputs arrays
		_intAverageOutputsLeft  = new int[20];
		_intArrayCountLeft 	    = 0;
		_intAverageOutputsRight = new int[20];
		_intArrayCountRight 	= 0;

		// Initializes the values for the left array
		for (int i = 0; i < _intAverageOutputsLeft.Length; i++)
			_intAverageOutputsLeft [i] = 0;

		// Initializes the values for the left array
		for (int i = 0; i < _intAverageOutputsRight.Length; i++)
			_intAverageOutputsRight [i] = 0;
		
		// Initializes the update average every second
		InvokeRepeating("UpdateAverageOutputs", 0, 1f);

	}

	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void Update () {

		// If the ESC key is pressed, enables / disables the menu interface
		if (Input.GetKeyDown (KeyCode.Escape)) {

			//Enables disables the camera blur
			GameObject.Find("Camera_01").GetComponent<Blur>().enabled = !GameObject.Find("Camera_01").GetComponent<Blur>().enabled;
			GameObject.Find("Camera_02").GetComponent<Blur>().enabled = !GameObject.Find("Camera_02").GetComponent<Blur>().enabled;
			GameObject.Find("Camera_03").GetComponent<Blur>().enabled = !GameObject.Find("Camera_03").GetComponent<Blur>().enabled;
			GameObject.Find("Camera_04").GetComponent<Blur>().enabled = !GameObject.Find("Camera_04").GetComponent<Blur>().enabled;

			// Enables / Disables the canvas and all the configuration interface
			GameObject.Find ("CanvasInterface").GetComponent<Canvas> ().enabled = !GameObject.Find ("CanvasInterface").GetComponent<Canvas> ().enabled;

			// Changes the menu active boolean
			Config.BLN_IS_INTERFACE_ACTIVE = !Config.BLN_IS_INTERFACE_ACTIVE;
		}

		// If the outputs are pressed, enables / disables the graphs interface
		if (!GameObject.Find ("CanvasOutputGraphs").GetComponent<Canvas> ().enabled && _blnIsGraphsActive) {

			//Enables disables the camera blur
			GameObject.Find("Camera_01").GetComponent<Blur>().enabled = !GameObject.Find("Camera_01").GetComponent<Blur>().enabled;
			GameObject.Find("Camera_02").GetComponent<Blur>().enabled = !GameObject.Find("Camera_02").GetComponent<Blur>().enabled;
			GameObject.Find("Camera_03").GetComponent<Blur>().enabled = !GameObject.Find("Camera_03").GetComponent<Blur>().enabled;
			GameObject.Find("Camera_04").GetComponent<Blur>().enabled = !GameObject.Find("Camera_04").GetComponent<Blur>().enabled;

			// Enables / Disables the canvas and all the configuration interface
			GameObject.Find ("CanvasOutputGraphs").GetComponent<Canvas> ().enabled = !GameObject.Find ("CanvasOutputGraphs").GetComponent<Canvas> ().enabled;

			// Changes the menu active boolean
			Config.BLN_IS_INTERFACE_ACTIVE = !Config.BLN_IS_INTERFACE_ACTIVE;
		}

		// Updates the outputs
		_fltOutputTimer += Time.deltaTime;

		// If the time passed, updates the output text
		if (_fltOutputTimer > 2) {

			// Gets average number of cars that passed during one second
			float _fltCarsAverageLeft  = 0;
			float _fltCarsAverageRight = 0;

			foreach (int _nbCars in _intAverageOutputsLeft) {
				_fltCarsAverageLeft += _nbCars;
			}

			foreach (int _nbCars in _intAverageOutputsRight) {
				_fltCarsAverageRight += _nbCars;
			}

			// Gets the averages
			_fltCarsAverageLeft  = _fltCarsAverageLeft / _intAverageOutputsLeft.Length * 60;
			_fltCarsAverageRight = _fltCarsAverageRight / _intAverageOutputsRight.Length * 60;

			// Updates the output text
			_txtCarOutputLeft.text  = "<< Débit : " + _fltCarsAverageLeft + " /min";
			_txtCarOutputRight.text = ">> Débit : " + _fltCarsAverageRight + " /min";

			// Resets the timer
			_fltOutputTimer = 0;
		}
	}

	/*
	 * Function 	: UpdateAverageOutput()
	 * Description  : Updates the average output. Places the amount of cars that passed during one second in the output array
	 */
	void UpdateAverageOutputs () {
		/// UPDATES THE LEFT OUTPUT
		// Puts the value in the array
		_intAverageOutputsLeft[_intArrayCountLeft] = Config.INT_CARS_OUTPUT_LEFT;

		// Updates the array count
		_intArrayCountLeft += 1;

		// Resets the cars count
		Config.INT_CARS_OUTPUT_LEFT = 0;

		// Resets the array count if max value
		if (_intArrayCountLeft == _intAverageOutputsLeft.Length)
			_intArrayCountLeft = 0;

		/// UPDATES THE RIGHT OUTPUT
		// Puts the value in the array
		_intAverageOutputsRight[_intArrayCountRight] = Config.INT_CARS_OUTPUT_RIGHT;

		// Updates the array count
		_intArrayCountRight += 1;

		// Resets the cars count
		Config.INT_CARS_OUTPUT_RIGHT = 0;

		// Resets the array count if max value
		if (_intArrayCountRight == _intAverageOutputsRight.Length)
			_intArrayCountRight = 0;
	}

	/*
	 * Function 	: ChangeNumberRoads()
	 * Description  : Changes the number of roads depending on the chosen value
	 */
	public void ChangeNumberRoads () {

		// Variables declaration
		int _intNewRoadsNumber = Mathf.RoundToInt(GameObject.Find("NbRoadsSlider").GetComponent<Slider>().value);	// The current slider value
		string _strNbRoadsMenuValue = _intNewRoadsNumber.ToString ();												// The value displayed next to the slider

		// Updates the dropdown value
		GameObject.Find("NbRoadsValue").GetComponent<Text>().text = _strNbRoadsMenuValue;

		// Changes the roas number global
		Config.INT_NB_ROADS = _intNewRoadsNumber;

		// Number of roads to delete
		int _intRoadsToDelete = Config.LI_GAME_ROADS.Count;

		// Destroys all the roads
		for (int i = 0; i < _intRoadsToDelete; i++) {

			// For each road
			Config.LI_GAME_ROADS [0].DestroyRoad();
		}

		// Generates the left direction roads
		for (int i = 0; i < Config.INT_NB_ROADS; i++)
			new Road (false);

		// Generates the right direction roads
		for (int i = 0; i < Config.INT_NB_ROADS; i++)
			new Road (true);
	}

	/*
	 * Function 	: ChangeRoadSize()
	 * Description  : Changes the size of the road depending on the chosen value
	 */
	public void ChangeRoadSize () {

		// Variables declaration
		int _intNewRoadSize = Mathf.RoundToInt(GameObject.Find("RoadSizeSlider").GetComponent<Slider>().value);		// The current slider value
		string _strRoadsSizeMenuValue = _intNewRoadSize.ToString ();												// The value displayed next to the slider

		// Updates the dropdown value
		GameObject.Find("RoadSizeValue").GetComponent<Text>().text = _strRoadsSizeMenuValue + "m";

		// Changes the roas number global
		Config.INT_ROAD_SIZE = _intNewRoadSize / 10;

		// Number of roads to delete
		int _intRoadsToDelete = Config.LI_GAME_ROADS.Count;

		// Destroys all the roads
		for (int i = 0; i < _intRoadsToDelete; i++) {

			// For each road
			Config.LI_GAME_ROADS [0].DestroyRoad();
		}

		// Generates the left direction roads
		for (int i = 0; i < Config.INT_NB_ROADS; i++)
			new Road (false);

		// Generates the right direction roads
		for (int i = 0; i < Config.INT_NB_ROADS; i++)
			new Road (true);
	}

	/*
	 * Function 	: ChangeSpeedLimit()
	 * Description  : Changes the speed limit
	 */
	public void ChangeSpeedLimit () {

		// Variables declaration
		int _intNewSpeedLimit = Mathf.RoundToInt(GameObject.Find("SpeedLimitSlider").GetComponent<Slider>().value);	// The current slider value
		string _strSpeedLimitMenuValue = _intNewSpeedLimit.ToString ();												// The value displayed next to the slider

		// Updates the dropdown value
		GameObject.Find("SpeedLimitValue").GetComponent<Text>().text = _strSpeedLimitMenuValue + " km/h";

		// Changes the roas number global
		Config.INT_SPEED_LIMIT_KMH = _intNewSpeedLimit;
	}

	/*
	 * Function 	: ChangeSpawnDensity()
	 * Description  : Changesthe amount of car spawns
	 */
	public void ChangeSpawnDensity () {

		// Variables declaration
		float _fltNewSpawnDensity = Mathf.Round(GameObject.Find("TraficDensitySlider").GetComponent<Slider>().value * 100) / 100;	// The current slider value
		string _strSpawnDensityMenuValue = _fltNewSpawnDensity.ToString ();															// The value displayed next to the slider

		// Updates the dropdown value
		GameObject.Find("TraficDensityValue").GetComponent<Text>().text = _strSpawnDensityMenuValue + " s";

		// Changes the roas number global
		Config.FLT_CARS_DENSITY_SEC = _fltNewSpawnDensity;
	}

	/*
	 * Function 	: ActivateGraphsInterface()
	 * Description  : Activates the graphs interface
	 */
	public void ActivateGraphsInterface () {_blnIsGraphsActive = true;}

	/*
	 * Function 	: ExitSimulation()
	 * Description  : Exits the current simulation
	 */
	public void ExitSimulation () {

		Application.Quit();
	}
}
