using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

			// Enables / Disables the canvas and all the configuration interface
			GameObject.Find ("CanvasInterface").GetComponent<Canvas> ().enabled = !GameObject.Find ("CanvasInterface").GetComponent<Canvas> ().enabled;

			// Changes the menu active boolean
			Config.BLN_IS_INTERFACE_ACTIVE = !Config.BLN_IS_INTERFACE_ACTIVE;
		}
	}
}
