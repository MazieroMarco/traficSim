using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Class 	   : Road
 * Descirption : Represents a road
 */
public class Road : Component {

	// Variables declaration
	private bool _blnDirectionRight;
	private int _intRoadSize;
	public float _fltRoadZPos;
	private List<GameObject> _liRoadPieces = new List<GameObject>();

	public List<CarBehavior> _liCars = new List<CarBehavior>();

	public Road(bool _blnDirectionIsRight)
	{
		// Sets the values
		_blnDirectionRight = _blnDirectionIsRight;
		_intRoadSize = Config.INT_ROAD_SIZE;
		_fltRoadZPos = 0;

		DrawRoad ();

		// Adds to the list
		Config.LI_GAME_ROADS.Add(this);
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
	}

	/*
	 * Function 	: DrawRoad()
	 * Description  : Draws the road on the scene
	 */
	public void DrawRoad() {

		// Destroys the entire road
		DestroyRoad();

		// Variables declaration
		if (Config.LI_GAME_ROADS.Count > 0)
			_fltRoadZPos = Config.LI_GAME_ROADS.LastOrDefault ()._fltRoadZPos - 0.2f;

		// Road separation direction
		if (Config.LI_GAME_ROADS.Count == Config.INT_NB_ROADS)
			_fltRoadZPos -= 0.1f;

		// Creates the new road by generating road pieces
		for (int i = _intRoadSize / 2 * -1; i < _intRoadSize / 2; i++) {

			// Generates the new road piece
			GameObject _goRoadPiece = (GameObject)Instantiate(Resources.Load("Road_01"));

			// Sets the road position an parent object
			_goRoadPiece.transform.position = new Vector3 (i, 0, _fltRoadZPos);
			_goRoadPiece.transform.parent	= GameObject.Find ("Roads").transform;

			// Adds the road piece to the list
			_liRoadPieces.Add(_goRoadPiece);
		}
	}
}
