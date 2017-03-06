using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Linq;

/*
 * Class 	   : UserInterface
 * Descirption : This is all the user configuration interface class
 */
public class UserInterface : MonoBehaviour {

	// Variables declaration
	private int _intOutputRefreshTimer30min; // Incremented each time the output is calculated
	private int _intOutputRefreshTimer5min;  // Incremented each time the output is calculated
	private int _intOutputRefreshTimer30sec; // Incremented each time the output is calculated
	private float _fltOutputTimer;			 // Timer for the output update
	private Text _txtCarOutputLeft;			 // The left output
	private Text _txtCarOutputRight;		 // The right output
	private Text _txtMinMaxLeft;			 // The left min / max values
	private Text _txtMinMaxRight;			 // The right min / max values
	private int[] _intAverageOutputsLeft;	 // Array containing the average outputs
	private int _intArrayCountLeft;			 // The current place in the average output array
	private int[] _intAverageOutputsRight;	 // Array containing the average outputs
	private int _intArrayCountRight;		 // The current place in the average output array
	private bool _blnIsGraphsActive;		 // To show if the graphs interface is active

	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Initializes the timers
		_fltOutputTimer = 0;

		// Initializes the refresh timer
		_intOutputRefreshTimer30min = 0;
		_intOutputRefreshTimer5min = 0;
		_intOutputRefreshTimer30sec = 0;

		// Initializes the outputs text
		_txtCarOutputLeft 		= GameObject.Find("carOutputLeft").GetComponent<Text>();
		_txtCarOutputRight 		= GameObject.Find("carOutputRight").GetComponent<Text>();
		_txtMinMaxLeft			= GameObject.Find("leftMinMaxValues").GetComponent<Text>();
		_txtMinMaxRight			= GameObject.Find("rightMinMaxValues").GetComponent<Text>();
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
		InvokeRepeating("UpdateGraphs", 0, 1f);

	}

	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void Update () {

		/// MENU INTERFACE ///
		// If the ESC key is pressed, enables / disables the menu interface
		if (Input.GetKeyDown (KeyCode.Escape) && (!Config.BLN_IS_INTERFACE_ACTIVE || GameObject.Find ("CanvasInterface").GetComponent<Canvas> ().enabled)) {

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

		/// CONSOLE INTERFACE ///
		// If the Ctrl + X keys are pressed, enables / disables the console interface
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.X) && (!Config.BLN_IS_INTERFACE_ACTIVE || GameObject.Find ("CanvasConsole").GetComponent<Canvas> ().enabled)) {

			// Enables / Disables the canvas and all the console interface
			GameObject.Find ("CanvasConsole").GetComponent<Canvas> ().enabled = !GameObject.Find ("CanvasConsole").GetComponent<Canvas> ().enabled;

			// Changes the menu active boolean
			Config.BLN_IS_INTERFACE_ACTIVE = !Config.BLN_IS_INTERFACE_ACTIVE;

			// Focuses on the input field
			GameObject.Find ("commandLine").GetComponent<InputField>().Select();
			GameObject.Find ("commandLine").GetComponent<InputField>().ActivateInputField();
		}

		/// CONSOLE MANAGEMENT ///
		// If the enter key is pressed
		if (Input.GetKeyDown (KeyCode.Return) && GameObject.Find ("CanvasConsole").GetComponent<Canvas> ().enabled) {

			// Gets the input field text
			string _strInput = GameObject.Find("commandLine").GetComponent<InputField>().text.ToLower();

			// Checks the text in the input field through the conditions
			if (_strInput == "set time to morning") {
				GameObject.Find ("SceneScripts").GetComponent<DayAndNightControl> ().currentTime = 0.24f;

			} else if (_strInput == "set time to midday") {
				GameObject.Find ("SceneScripts").GetComponent<DayAndNightControl> ().currentTime = 0.50f;

			} else if (_strInput == "set time to evening") {
				GameObject.Find ("SceneScripts").GetComponent<DayAndNightControl> ().currentTime = 0.75f;

			} else if (_strInput == "set time to night") {
				GameObject.Find ("SceneScripts").GetComponent<DayAndNightControl> ().currentTime = 0f;

			} else if (_strInput == "enablecarcontrol") {
				Config.BLN_CAR_CONTROL = true;

			} else if (_strInput == "disablecarcontrol") {
				Config.BLN_CAR_CONTROL = false;

			} else if (_strInput == "set timescale to 1") {
				UnityEngine.Time.timeScale = 1;

			} else if (_strInput == "set timescale to 5") {
				UnityEngine.Time.timeScale = 5;

			} else if (_strInput == "set timescale to 8") {
				UnityEngine.Time.timeScale = 8;

			} else if (_strInput == "set timescale to 10") {
				UnityEngine.Time.timeScale = 10;

			} else if (_strInput == "changegravitystate") {
				UnityEngine.Physics.gravity = UnityEngine.Physics.gravity == Vector3.zero ? new Vector3 (0, -9.81f, 0) : Vector3.zero;

			} else if (_strInput == "enablefreecamera") {
				Config.BLN_FREE_CAMERA = true;

				// Sets camera position and rotation
				GameObject.Find("Camera_03").GetComponent<Camera>().transform.position = new Vector3(0f, 2f, 0f);
				GameObject.Find("Camera_03").GetComponent<Camera>().transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			} else if (_strInput == "disablefreecamera") {
				Config.BLN_FREE_CAMERA = false;

				// Resets camera position and rotation
				GameObject.Find("Camera_03").GetComponent<Camera>().transform.position = new Vector3(1.5f, 0.25f, -1.7f);
				GameObject.Find("Camera_03").GetComponent<Camera>().transform.rotation = Quaternion.Euler(6.378f, -48.112f, 0f);
			}

			// Deletes the text in the input
			GameObject.Find("commandLine").GetComponent<InputField>().text = "";

			// Closes the console
			GameObject.Find ("CanvasConsole").GetComponent<Canvas> ().enabled = !GameObject.Find ("CanvasConsole").GetComponent<Canvas> ().enabled;
			Config.BLN_IS_INTERFACE_ACTIVE = !Config.BLN_IS_INTERFACE_ACTIVE;
		}
		
		/// OUTPUT MANAGEMENT ///
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

			// Updates the left outputs average arrays
			if (Config.LI_LEFT_OUTPUTS.Count < Config.INT_GRAPHS_NB_DATA) {

				// Adds the new value
				Config.LI_LEFT_OUTPUTS.Add (_fltCarsAverageLeft);
			} else {

				// Deletes the oldest value
				Config.LI_LEFT_OUTPUTS.RemoveAt(0);

				// Adds the new value
				Config.LI_LEFT_OUTPUTS.Add (_fltCarsAverageLeft);
			}

			// Updates the right outputs average arrays
			if (Config.LI_RIGHT_OUTPUTS.Count < Config.INT_GRAPHS_NB_DATA) {

				// Adds the new value
				Config.LI_RIGHT_OUTPUTS.Add (_fltCarsAverageRight);
			} else {

				// Deletes the oldest value
				Config.LI_RIGHT_OUTPUTS.RemoveAt(0);

				// Adds the new value
				Config.LI_RIGHT_OUTPUTS.Add (_fltCarsAverageRight);
			}
				

			// Updates the output text
			_txtCarOutputLeft.text  = "<< Débit : " + _fltCarsAverageLeft + " /min";
			_txtCarOutputRight.text = ">> Débit : " + _fltCarsAverageRight + " /min";

			// Checks the max / min values
			Config.INT_OUTPUT_LEFT_MAX[0]  = Config.INT_OUTPUT_LEFT_MAX[0] < Mathf.RoundToInt(_fltCarsAverageLeft) ? Mathf.RoundToInt(_fltCarsAverageLeft) : Config.INT_OUTPUT_LEFT_MAX[0];
			Config.INT_OUTPUT_LEFT_MIN[0]  = Config.INT_OUTPUT_LEFT_MIN[0] > Mathf.RoundToInt(_fltCarsAverageLeft) ? Mathf.RoundToInt(_fltCarsAverageLeft) : Config.INT_OUTPUT_LEFT_MIN[0];
			Config.INT_OUTPUT_RIGHT_MAX[0] = Config.INT_OUTPUT_RIGHT_MAX[0] < Mathf.RoundToInt(_fltCarsAverageRight) ? Mathf.RoundToInt(_fltCarsAverageRight) : Config.INT_OUTPUT_RIGHT_MAX[0];
			Config.INT_OUTPUT_RIGHT_MIN[0] = Config.INT_OUTPUT_RIGHT_MIN[0] > Mathf.RoundToInt(_fltCarsAverageRight) ? Mathf.RoundToInt(_fltCarsAverageRight) : Config.INT_OUTPUT_RIGHT_MIN[0];

			Config.INT_OUTPUT_LEFT_MAX[1]  = Config.INT_OUTPUT_LEFT_MAX[1] < Mathf.RoundToInt(_fltCarsAverageLeft) ? Mathf.RoundToInt(_fltCarsAverageLeft) : Config.INT_OUTPUT_LEFT_MAX[1];
			Config.INT_OUTPUT_LEFT_MIN[1]  = Config.INT_OUTPUT_LEFT_MIN[1] > Mathf.RoundToInt(_fltCarsAverageLeft) ? Mathf.RoundToInt(_fltCarsAverageLeft) : Config.INT_OUTPUT_LEFT_MIN[1];
			Config.INT_OUTPUT_RIGHT_MAX[1] = Config.INT_OUTPUT_RIGHT_MAX[1] < Mathf.RoundToInt(_fltCarsAverageRight) ? Mathf.RoundToInt(_fltCarsAverageRight) : Config.INT_OUTPUT_RIGHT_MAX[1];
			Config.INT_OUTPUT_RIGHT_MIN[1] = Config.INT_OUTPUT_RIGHT_MIN[1] > Mathf.RoundToInt(_fltCarsAverageRight) ? Mathf.RoundToInt(_fltCarsAverageRight) : Config.INT_OUTPUT_RIGHT_MIN[1];

			Config.INT_OUTPUT_LEFT_MAX[2]  = Config.INT_OUTPUT_LEFT_MAX[2] < Mathf.RoundToInt(_fltCarsAverageLeft) ? Mathf.RoundToInt(_fltCarsAverageLeft) : Config.INT_OUTPUT_LEFT_MAX[2];
			Config.INT_OUTPUT_LEFT_MIN[2]  = Config.INT_OUTPUT_LEFT_MIN[2] > Mathf.RoundToInt(_fltCarsAverageLeft) ? Mathf.RoundToInt(_fltCarsAverageLeft) : Config.INT_OUTPUT_LEFT_MIN[2];
			Config.INT_OUTPUT_RIGHT_MAX[2] = Config.INT_OUTPUT_RIGHT_MAX[2] < Mathf.RoundToInt(_fltCarsAverageRight) ? Mathf.RoundToInt(_fltCarsAverageRight) : Config.INT_OUTPUT_RIGHT_MAX[2];
			Config.INT_OUTPUT_RIGHT_MIN[2] = Config.INT_OUTPUT_RIGHT_MIN[2] > Mathf.RoundToInt(_fltCarsAverageRight) ? Mathf.RoundToInt(_fltCarsAverageRight) : Config.INT_OUTPUT_RIGHT_MIN[2];

			// Updates the min / max values
			_txtMinMaxLeft.text  = Config.INT_OUTPUT_LEFT_MIN[0].ToString("000") + "   |   " + Config.INT_OUTPUT_LEFT_MIN[1].ToString("000") + "   |   " + Config.INT_OUTPUT_LEFT_MIN[2].ToString("000") + "\n" +
								   Config.INT_OUTPUT_LEFT_MAX[0].ToString("000") + "   |   " + Config.INT_OUTPUT_LEFT_MAX[1].ToString("000") + "   |   " + Config.INT_OUTPUT_LEFT_MAX[2].ToString("000");
			
			_txtMinMaxRight.text  = Config.INT_OUTPUT_RIGHT_MIN[0].ToString("000") + "   |   " + Config.INT_OUTPUT_RIGHT_MIN[1].ToString("000") + "   |   " + Config.INT_OUTPUT_RIGHT_MIN[2].ToString("000") + "\n" +
								    Config.INT_OUTPUT_RIGHT_MAX[0].ToString("000") + "   |   " + Config.INT_OUTPUT_RIGHT_MAX[1].ToString("000") + "   |   " + Config.INT_OUTPUT_RIGHT_MAX[2].ToString("000");

			// Resets the timer
			_fltOutputTimer = 0;
		}

		/// TIME OF DAY MANAGEMENT ///
		// Updates the time of the day text
		GameObject.Find("timeOfDay").GetComponent<Text>().text = Mathf.Floor((Config.FLT_TIME_OF_DAY * 24)).ToString("00") + ":" + (((Config.FLT_TIME_OF_DAY * 24) - Mathf.Floor(Config.FLT_TIME_OF_DAY * 24)) * 60).ToString("00");

		/// CAR PROBLEM ICONS ///
		// Updates the icon
		if (Config.BLN_CAR_PROBLEM_ICON) {

			// Activates the image
			GameObject.Find("CarProblemIcon").GetComponent<Image>().enabled = true;
			GameObject.Find("CarProblemBack").GetComponent<Image>().enabled = true;
		} else {

			// Deactivates the image
			GameObject.Find("CarProblemIcon").GetComponent<Image>().enabled = false;
			GameObject.Find("CarProblemBack").GetComponent<Image>().enabled = false;
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

		/// INCREMENTS THE MIN / MAX TIMERS AND UPDATES THE MIN / MAX VALUES
		_intOutputRefreshTimer30min++;
		_intOutputRefreshTimer5min++;
		_intOutputRefreshTimer30sec++;

		// If 30 seconds passed
		if (_intOutputRefreshTimer30sec >= 30) {

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

			// Updates the 30 seconds values
			Config.INT_OUTPUT_LEFT_MAX[0] = Mathf.RoundToInt(_fltCarsAverageLeft);
			Config.INT_OUTPUT_LEFT_MIN[0] = Mathf.RoundToInt(_fltCarsAverageLeft);
			Config.INT_OUTPUT_RIGHT_MAX[0] = Mathf.RoundToInt(_fltCarsAverageRight);
			Config.INT_OUTPUT_RIGHT_MIN[0] = Mathf.RoundToInt(_fltCarsAverageRight);

			// Resets the timer
			_intOutputRefreshTimer30sec = 0;
		}

		// If 5 minutes passed
		if (_intOutputRefreshTimer5min >= 300) {

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

			// Updates the 30 seconds values
			Config.INT_OUTPUT_LEFT_MAX[1] = Mathf.RoundToInt(_fltCarsAverageLeft);
			Config.INT_OUTPUT_LEFT_MIN[1] = Mathf.RoundToInt(_fltCarsAverageLeft);
			Config.INT_OUTPUT_RIGHT_MAX[1] = Mathf.RoundToInt(_fltCarsAverageRight);
			Config.INT_OUTPUT_RIGHT_MIN[1] = Mathf.RoundToInt(_fltCarsAverageRight);

			// Resets the timer
			_intOutputRefreshTimer5min = 0;
		}

		// If 30 minutes passed
		if (_intOutputRefreshTimer30min >= 1800) {

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

			// Updates the 30 seconds values
			Config.INT_OUTPUT_LEFT_MAX[2] = Mathf.RoundToInt(_fltCarsAverageLeft);
			Config.INT_OUTPUT_LEFT_MIN[2] = Mathf.RoundToInt(_fltCarsAverageLeft);
			Config.INT_OUTPUT_RIGHT_MAX[2] = Mathf.RoundToInt(_fltCarsAverageRight);
			Config.INT_OUTPUT_RIGHT_MIN[2] = Mathf.RoundToInt(_fltCarsAverageRight);

			// Resets the timer
			_intOutputRefreshTimer30min = 0;
		}
	}

	/*
	 * Function 	: StartSimulation()
	 * Description  : Starts the simulation when the start button is pressed
	 */
	public void StartSimulation () {

		// Updates the blur of the camera 1
		GameObject.Find ("Camera_01").GetComponent<Blur>().iterations = 3;
		GameObject.Find ("Camera_01").GetComponent<Blur> ().enabled   = false;

		// Hides the start menu
		GameObject.Find ("CanvasStartMenu").GetComponent<Canvas> ().enabled = false;

		// Displays the outputs menu
		GameObject.Find ("CanvasOutput").GetComponent<Canvas> ().enabled = true;

		// Disables the interface mode
		Config.BLN_IS_INTERFACE_ACTIVE = false;
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

		// Changes the roads number global
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

		// Changes value
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

		// Changes value
		Config.INT_SPEED_LIMIT_KMH = _intNewSpeedLimit;
	}

	/*
	 * Function 	: ChangeSpawnDensity()
	 * Description  : Changes the amount of car spawns
	 */
	public void ChangeSpawnDensity () {

		// Variables declaration
		float _fltNewSpawnDensity = Mathf.Round(GameObject.Find("TraficDensitySlider").GetComponent<Slider>().value * 1000) / 1000;	// The current slider value
		string _strSpawnDensityMenuValue = _fltNewSpawnDensity.ToString ();															// The value displayed next to the slider

		// Updates the dropdown value
		GameObject.Find("TraficDensityValue").GetComponent<Text>().text = _strSpawnDensityMenuValue + " s";

		// Changes value
		Config.FLT_CARS_DENSITY_SEC = _fltNewSpawnDensity;
	}

	/*
	 * Function 	: ChangeBreakdownChance()
	 * Description  : Changes the chances for a car of having a breakdown
	 */
	public void ChangeBreakdownChance () {

		// Variables declaration
		float _fltNewProblemValue = Mathf.Round(GameObject.Find("ProblemSlider").GetComponent<Slider>().value * 1000) / 1000;	// The current slider value
		string _strProblemMenuValue = _fltNewProblemValue.ToString ();															// The value displayed next to the slider

		// Updates the dropdown value
		GameObject.Find("ProblemValue").GetComponent<Text>().text = _strProblemMenuValue + "%";

		// Changes value
		Config.FLT_BREAKDOWN_CHANCES = _fltNewProblemValue;
	}

	/*
	 * Function 	: ChangeTruckSpeedLimit()
	 * Description  : Changes the speed limit for the trucks
	 */
	public void ChangeTruckSpeedLimit () {

		// Variables declaration
		int _fltNewSpeedValue = Mathf.RoundToInt(GameObject.Find("TruckLimitSlider").GetComponent<Slider>().value);		// The current slider value
		string _strSpeedMenuValue = _fltNewSpeedValue.ToString ();														// The value displayed next to the slider

		// Updates the dropdown value
		GameObject.Find("TruckLimitValue").GetComponent<Text>().text = _strSpeedMenuValue + " km/h";

		// Changes value
		Config.INT_SPEED_LIMIT_KMH_TRUCK = _fltNewSpeedValue;
	}

	/*
	 * Function 	: ChangeTruckDensity()
	 * Description  : Changes the trucks density
	 */
	public void ChangeTruckDensity () {

		// Variables declaration
		int _fltNewDensityValue = Mathf.RoundToInt(GameObject.Find("TruckSlider").GetComponent<Slider>().value);		// The current slider value
		string _strDensityMenuValue = _fltNewDensityValue.ToString ();														// The value displayed next to the slider

		// Updates the dropdown value
		GameObject.Find("TruckValue").GetComponent<Text>().text = _strDensityMenuValue + "%";

		// Changes value
		Config.INT_TRUCK_DENSITY = _fltNewDensityValue;
	}

	/*
	 * Function 	: ChangeWeather()
	 * Description  : Changes the weather
	 */
	public void ChangeWeather () {

		// Variables declaration
		int _intNewWeatherValue = Mathf.RoundToInt(GameObject.Find("WeatherSlider").GetComponent<Slider>().value) - 1;		// The current slider value

		// Changes value and weather
		GameObject.Find("SceneScripts").GetComponent<WeatherManager>().ChangeWeather(_intNewWeatherValue);

		// Updates the weather value and icon
		WeatherManager _wmGameWeather = GameObject.Find("SceneScripts").GetComponent<WeatherManager>();
		GameObject.Find("WeatherValue").GetComponent<Text>().text = _wmGameWeather._strWeatherName;
		GameObject.Find("WeatherIcon").GetComponent<Image>().material = _wmGameWeather._matWeatherIcon;
	}

	/*
	 * Function 	: UpdateGraphs()
	 * Description  : Updates all the data lines in the left and right graphs
	 */
	public void UpdateGraphs () {

		// Variables declaration
		var _v3LeftGraphPositions = new Vector3[31];
		var _v3RightGraphPositions = new Vector3[31];

		// Defines the values for the left graph
		for (int i = 0; i < _v3LeftGraphPositions.Length; i++) {

			// Sets the values
			if (i < Config.LI_LEFT_OUTPUTS.Count)
				_v3LeftGraphPositions [i] = new Vector3 ((i * 600 / 30) - 600, Config.LI_LEFT_OUTPUTS [i] - 300f, -1f);
			else if(i > 0)
				_v3LeftGraphPositions [i] = _v3LeftGraphPositions [i - 1];	
		}

		// Defines the values for the right graph
		for (int i = 0; i < _v3RightGraphPositions.Length; i++) {

			// Sets the values
			if (i < Config.LI_RIGHT_OUTPUTS.Count)
				_v3RightGraphPositions [i] = new Vector3 ((-1 * (i * 600 / 30)) + 600, Config.LI_RIGHT_OUTPUTS [i] - 300f, -1f);
			else if(i > 0)
				_v3RightGraphPositions [i] = _v3RightGraphPositions [i - 1];	
		}

		// Applies the values to the graphical view
		GameObject.Find ("LeftData").GetComponent<LineRenderer> ().SetPositions (_v3LeftGraphPositions);
		GameObject.Find ("RightData").GetComponent<LineRenderer> ().SetPositions (_v3RightGraphPositions);
	}

	/*
	 * Function 	: ActivateGraphsInterface()
	 * Description  : Activates the graphs interface
	 */
	public void ActivateGraphsInterface () {

		// Activates only for the two first cameras
		if (GameObject.Find ("SceneScripts").GetComponent<CameraBehavior> ()._caCamera1.enabled || GameObject.Find ("SceneScripts").GetComponent<CameraBehavior> ()._caCamera2.enabled) {

			// Sets the canvas to the current camera
			if (GameObject.Find("SceneScripts").GetComponent<CameraBehavior>()._caCamera1.enabled)
				GameObject.Find ("CanvasOutputGraphs").GetComponent<Canvas> ().worldCamera = GameObject.Find("Camera_01").GetComponent<Camera>();
			else if (GameObject.Find("SceneScripts").GetComponent<CameraBehavior>()._caCamera2.enabled)
				GameObject.Find ("CanvasOutputGraphs").GetComponent<Canvas> ().worldCamera = GameObject.Find("Camera_02").GetComponent<Camera>();

			// Enables / Disables the canvas and all the configuration interface
			GameObject.Find ("CanvasOutputGraphs").GetComponent<Canvas> ().enabled = !GameObject.Find ("CanvasOutputGraphs").GetComponent<Canvas> ().enabled;
			GameObject.Find ("Graphs").GetComponent<LineRenderer> ().enabled = !GameObject.Find ("Graphs").GetComponent<LineRenderer> ().enabled;
			GameObject.Find ("LeftData").GetComponent<LineRenderer> ().enabled = !GameObject.Find ("LeftData").GetComponent<LineRenderer> ().enabled;
			GameObject.Find ("RightData").GetComponent<LineRenderer> ().enabled = !GameObject.Find ("RightData").GetComponent<LineRenderer> ().enabled;

			// Changes the menu active boolean
			Config.BLN_IS_INTERFACE_ACTIVE = !Config.BLN_IS_INTERFACE_ACTIVE;
		}
	}

	/*
	 * Function 	: ShowMinMaxInterface()
	 * Description  : Activates the min / max values interface
	 */
	public void ShowMinMaxInterface () {

		// Changes the display state of the canvas
		GameObject.Find ("CanvasDetails").GetComponent<Canvas> ().enabled = !GameObject.Find ("CanvasDetails").GetComponent<Canvas> ().enabled;

		// Rotates the button
		GameObject.Find("DetailsIcon").GetComponent<RectTransform>().transform.Rotate(0, 0, 180);
	}

	/*
	 * Function 	: SetTimeToMorning()
	 * Description  : Sets the time to morning
	 */
	public void SetTimeToMorning () {GameObject.Find ("SceneScripts").GetComponent<DayAndNightControl> ().currentTime = 0.24f;}

	/*
	 * Function 	: SetTimeToMidday()
	 * Description  : Sets the time to midday
	 */
	public void SetTimeToMidday () {GameObject.Find ("SceneScripts").GetComponent<DayAndNightControl> ().currentTime = 0.50f;}

	/*
	 * Function 	: SetTimeToEvening()
	 * Description  : Sets the time to evening
	 */
	public void SetTimeToEvening () {GameObject.Find ("SceneScripts").GetComponent<DayAndNightControl> ().currentTime = 0.75f;}

	/*
	 * Function 	: SetTimeToNight()
	 * Description  : Sets the time to night
	 */
	public void SetTimeToNight () {GameObject.Find ("SceneScripts").GetComponent<DayAndNightControl> ().currentTime = 0f;}

	/*
	 * Function 	: ChangeBiomeToMountain()
	 * Description  : Changes the biome to the mountain
	 */
	public void ChangeBiomeToMountain () {
		GameObject.Find ("MountainTerrain").GetComponent<Terrain> ().enabled = true;
		GameObject.Find ("MountainTerrain").GetComponent<TerrainCollider> ().enabled = true;
		GameObject.Find ("DesertTerrain").GetComponent<Terrain> ().enabled = false;
		GameObject.Find ("DesertTerrain").GetComponent<TerrainCollider> ().enabled = false;
	}

	/*
	 * Function 	: ChangeBiomeToDesert()
	 * Description  : Changes the biome to the mountain
	 */
	public void ChangeBiomeToDesert () {
		GameObject.Find ("MountainTerrain").GetComponent<Terrain> ().enabled = false;
		GameObject.Find ("MountainTerrain").GetComponent<TerrainCollider> ().enabled = false;
		GameObject.Find ("DesertTerrain").GetComponent<Terrain> ().enabled = true;
		GameObject.Find ("DesertTerrain").GetComponent<TerrainCollider> ().enabled = true;
	}

	/*
	 * Function 	: ChangeScrollSensitivity()
	 * Description  : Changes the scroll sensitivity
	 */
	public void ChangeScrollSensitivity() {

		// Variables declaration
		float _fltNewSensitivity = Mathf.Round(GameObject.Find("ScrollSensitivitySlider").GetComponent<Slider>().value * 10) / 10;	// The current slider value
		string _strNewSensitivityMenuValue = _fltNewSensitivity.ToString ();														// The value displayed next to the slider

		// Updates the dropdown value
		GameObject.Find("ScrollSensitivityValue").GetComponent<Text>().text = _strNewSensitivityMenuValue;

		// Changes value
		Config.FLT_SCROLL_SENSITIVITY = _fltNewSensitivity;
	}

	/*
	 * Function 	: ExitSimulation()
	 * Description  : Exits the current simulation
	 */
	public void ExitSimulation () {

		// Exits
		Application.Quit();
	}
}
