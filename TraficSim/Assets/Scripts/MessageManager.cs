using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class 	   : MessageManager
 * Descirption : The messages displays on the cars
 */
public class MessageManager : MonoBehaviour {

	// Variables declaration
	private string[] _strGoodMessages = {"Good Message\nRetourLigne"};
	private string[] _strBadMessages = {"Bad Message\nRetourLigne"};

	/*
	 * Function 	: Start()
	 * Description  : Executed at the begining of the class initialization
	 */
	void Start () {
		
	}
	
	/*
	 * Function 	: Update()
	 * Description  : Called every frame
	 */
	void Update () {

		if (Random.Range (0, 100) <50) {

			DisplayMessage ();
		}

		// Gets the ctive camera
		Camera _caActiveCamera = GameObject.Find ("SceneScripts").GetComponent<CameraBehavior> ().CA_ACTIVE_CAMERA;

		foreach (GameObject _goMessage in GameObject.FindGameObjectsWithTag("Message")) {

			_goMessage.transform.LookAt(transform.position + _caActiveCamera.transform.rotation * Vector3.forward,
				_caActiveCamera.transform.rotation * Vector3.up);
		}
	}

	/*
	 * Function 	: DisplayMessage()
	 * Description  : Displays a random message
	 */
	void DisplayMessage () {

		// Gets a random car
		CarBehavior _cbRandomCar = GetRandomCar ();

		// Instanciates a message on a random car
		GameObject _goCurrentMessage = GameObject.Instantiate(Resources.Load<GameObject>("MessageBack"));

		// Adds the message to the car
		_goCurrentMessage.gameObject.transform.parent = _cbRandomCar.gameObject.transform;
		//_goCurrentMessage.gameObject.transform.position = _cbRandomCar.ga

		// Sets the message position
		_goCurrentMessage.gameObject.transform.localPosition = new Vector3(0, 0.5f, 0);

		// Gets the message text
		TextMesh _goText = _goCurrentMessage.GetComponentInChildren<TextMesh>();

		// Message conditions
		if (_cbRandomCar._fltCarSpeed < _cbRandomCar._fltCarInitialSpeed / 2) {

			_goText.text = _strBadMessages [Random.Range (0, _strBadMessages.Length)];

		} else {

			_goText.text = _strGoodMessages [Random.Range (0, _strGoodMessages.Length)];
		}
	}

	/*
	 * Function    : GetRandomCar()
	 * Description : Gets a random car from the scene
	 */
	CarBehavior GetRandomCar () {

		// Gets all the cars in the scene
		var _liAllCars = new List<CarBehavior>();

		foreach (Road _road in Config.LI_GAME_ROADS) {

			_liAllCars.AddRange (_road._liCars);
		}

		return _liAllCars [Random.Range(0, _liAllCars.Count - 1)];
	}
}
