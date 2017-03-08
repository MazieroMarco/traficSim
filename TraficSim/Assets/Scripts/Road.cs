using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Class 	   : Road
 * Descirption : Represents a road
 */
public class Road : Component {

	// Private variables declaration
	private int _intRoadSize;											// The size of the road in meters / 10
	private List<GameObject> _liRoadPieces = new List<GameObject>();	// The list of all the pieces in the road

	// Public variables declaration
	public float _fltRoadZPos;											// The Z axis position of the road on the scene
	public List<CarBehavior> _liCars = new List<CarBehavior>();			// The list of all the cars acctually on the road
	public bool _blnDirectionRight;										// The direction of the cars on the road
	public int _intRoadID;												// The unique id of the road

	/*
	 * Function 	: Road()
	 * Description  : Class constructor, called at the beginning
	 */
	public Road (bool _blnDirectionIsRight)
	{
		// Sets the values
		_blnDirectionRight = _blnDirectionIsRight;
		_intRoadSize = Config.INT_ROAD_SIZE;
		_fltRoadZPos = 0;
		_intRoadID = Config.LI_GAME_ROADS.Count + 1;

		// Draws the road for the first time
		DrawRoad ();
	}

	/*
	 * Function 	: DestroyRoad()
	 * Description  : Destroys the road and all its components
	 */
	public void DestroyRoad() {

		// Destroys every road piece
		foreach (GameObject _goPiece in _liRoadPieces) {  

			// Destroys the piece
			Destroy(_goPiece);
		}

		// Destroys all the obstacles
		foreach (GameObject _goObstacle in GameObject.FindGameObjectsWithTag("Obstacle")) {  

			// Destroys the piece
			Destroy(_goObstacle);
		}
			
		// Destroys every car on the current road
		foreach (CarBehavior _goCar in _liCars) {

			// Destroys the car
			Destroy(_goCar.gameObject);
		}

		// Destroys the accidents impacts
		foreach (GameObject _goSmoke in GameObject.FindGameObjectsWithTag("Smoke")) {

			// Destroys the smoke
			Destroy (_goSmoke);
		}

		// Disable the accidents icon
		Config.BLN_CAR_PROBLEM_ICON = false;

		Config.LI_GAME_ROADS.Remove(Config.LI_GAME_ROADS.FirstOrDefault(a=>a._intRoadID==this._intRoadID));
	}

	/*
	 * Function 	: GetSpawnOrigin()
	 * Description  : Returns the spawn origin vector for the cars
	 */
	public Vector3 GetSpawnOrigin () {

		return _blnDirectionRight ? _liRoadPieces.FirstOrDefault ().transform.position : _liRoadPieces.LastOrDefault ().transform.position;
	}

	/*
	 * Function 	: GetEndOfTheRoad()
	 * Description  : Returns the position of the end of the road
	 */
	public Vector3 GetEndOfTheRoad () {
		if (_liRoadPieces == null)
			return Vector3.zero;
		return _blnDirectionRight ? _liRoadPieces.LastOrDefault ().transform.position : _liRoadPieces.FirstOrDefault ().transform.position;
	}

	/*
	 * Function 	: DrawRoad()
	 * Description  : Draws the road on the scene
	 */
	public void DrawRoad() {

		// Variables declaration
		int i;					// Loop variable

		// Destroys the entire road if it already exists on the scene
		for (i = 0; i < Config.LI_GAME_ROADS.Count; i++) {

			// Checks if the road exists
			if (Config.LI_GAME_ROADS[i]._intRoadID == _intRoadID)
				DestroyRoad();
		}

		// Sets the new road Z axis
		if (Config.LI_GAME_ROADS.Count > 0)
			_fltRoadZPos = Config.LI_GAME_ROADS.LastOrDefault ()._fltRoadZPos - 0.2f;

		// Road separation direction
		if (Config.LI_GAME_ROADS.Count == Config.INT_NB_ROADS)
			_fltRoadZPos -= 0.1f;

		// Creates the new road by generating road pieces
		for (i = _intRoadSize / 2 * -1; i < _intRoadSize / 2; i++) {

			// Generates the new road piece
			GameObject _goRoadPiece = Instantiate(Resources.Load<GameObject>("Road_01"));

			// Sets the road position and parent object
			_goRoadPiece.transform.position = new Vector3 (i, 0, _fltRoadZPos);
			_goRoadPiece.transform.parent	= GameObject.Find ("Roads").transform;

			// Adds the road piece to the list
			_liRoadPieces.Add(_goRoadPiece);
		}

		// Adds the road to the roads list to the list
		Config.LI_GAME_ROADS.Add(this);
	}
}
