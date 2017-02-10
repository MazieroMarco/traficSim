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

	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void Update () {

		// If the ESC key is pressed, enables / disables the interface
		if (Input.GetKeyDown (KeyCode.Escape)) {

			// Enables disables the camera blur
			GameObject.Find("Camera_01").GetComponent<Blur>().enabled = !GameObject.Find("Camera_01").GetComponent<Blur>().enabled;
			GameObject.Find("Camera_02").GetComponent<Blur>().enabled = !GameObject.Find("Camera_02").GetComponent<Blur>().enabled;
			GameObject.Find("Camera_03").GetComponent<Blur>().enabled = !GameObject.Find("Camera_03").GetComponent<Blur>().enabled;
			GameObject.Find("Camera_04").GetComponent<Blur>().enabled = !GameObject.Find("Camera_04").GetComponent<Blur>().enabled;

			// Enables / Disables the canvas and all the configuration interface
			GameObject.Find ("CanvasInterface").GetComponent<Canvas> ().enabled = !GameObject.Find ("CanvasInterface").GetComponent<Canvas> ().enabled;

			// Changes the menu active boolean
			Config.BLN_IS_INTERFACE_ACTIVE = !Config.BLN_IS_INTERFACE_ACTIVE;
		}
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
	 * Function 	: ExitSimulation()
	 * Description  : Exits the current simulation
	 */
	public void ExitSimulation () {

		Application.Quit();
	}
}
