using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

	// Variables declaration
	public ParticleSystem _peRain;
	public ParticleSystem _peSnow;
	public string _strWeatherName;
	public Material _matWeatherIcon;

	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {

		// Disables all the weathers ans particle systems
		ResetWeather();
	}
	
	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void Update () {

		// Updates the emitters positions
		Camera _caActiveGameCamera = GameObject.Find("SceneScripts").GetComponent<CameraBehavior>().CA_ACTIVE_CAMERA;
		_peRain.transform.position = new Vector3 (_caActiveGameCamera.transform.position.x, _caActiveGameCamera.transform.position.y + 5, _caActiveGameCamera.transform.position.z);
		_peSnow.transform.position = new Vector3 (_caActiveGameCamera.transform.position.x, _caActiveGameCamera.transform.position.y + 5, _caActiveGameCamera.transform.position.z);
	}

	/*
	 * Function 	: ChangeWeather()
	 * Description  : Changes the weather
	 */
	public void ChangeWeather(int _intWeatherValue) {

		// Updates the current weather
		switch (_intWeatherValue) {

		case 0:
			// Updates the weather name and icon
			_strWeatherName = "Clair";
			_matWeatherIcon = Resources.Load("weather_sun", typeof(Material)) as Material;

			// Updates the weather
			StartSun ();
			break;

		case 1:
			// Updates the weather name and icon
			_strWeatherName = "Pluie";
			_matWeatherIcon = Resources.Load("weather_rain", typeof(Material)) as Material;

			// Updates the weather
			StartRain ();
			break;

		case 2:
			// Updates the weather name and icon
			_strWeatherName = "Snow";
			_matWeatherIcon = Resources.Load("weather_snow", typeof(Material)) as Material;

			// Updates the weather
			StartSnow ();
			break;
		}
	}

	/*
	 * Function 	: StartSun()
	 * Description  : Starts the sunny weather
	 */
	public void StartSun() {

		// Resets the constraints
		ResetWeather();
	}

	/*
	 * Function 	: StartRain()
	 * Description  : Starts the rain weather
	 */
	public void StartRain() {

		// Resets the constraints
		ResetWeather();

		// Enables the rain emitter
		ParticleSystem.EmissionModule _rainEmission = _peRain.emission;
		_rainEmission.enabled = true;
	}

	/*
	 * Function 	: StartSnow()
	 * Description  : Starts the snow weather
	 */
	public void StartSnow() {

		// Resets the constraints
		ResetWeather();

		// Enables the snow emitter
		ParticleSystem.EmissionModule _snowEmission = _peSnow.emission;
		_snowEmission.enabled = true;
	}

	/*
	 * Function 	: ResetWeather()
	 * Description  : Resets all the constrains to default
	 */
	private void ResetWeather() {

		// Disables all the weathers ans particle systems
		ParticleSystem.EmissionModule _rainEmission = _peRain.emission;
		ParticleSystem.EmissionModule _snowEmission = _peSnow.emission;
		_rainEmission.enabled = false;
		_snowEmission.enabled = false;
	}
}
