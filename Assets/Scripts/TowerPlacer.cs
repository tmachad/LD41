using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour {

	[SerializeField]
	private GameObject towerRoot;

	[SerializeField]
	private GameObject tower;

	private bool mouseIsDown = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0) && !mouseIsDown && !GameManager.S.gameOver) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0;

			pos.x = (pos.x / Mathf.Abs (pos.x)) * (Mathf.Floor (Mathf.Abs (pos.x)) + 0.5f);	// Round to nearest half
			pos.y = (pos.y / Mathf.Abs (pos.y)) * (Mathf.Floor (Mathf.Abs (pos.y)) + 0.5f);	// Round to nearest half

			int cost = tower.GetComponent<Tower> ().cost;
			Tile tile;
			if (CheckValidBuildAtPosition(pos, out tile) && GameManager.S.money >= cost) {
				GameManager.S.ModifyMoney (-cost);
				GameObject.Instantiate (tower, pos, Quaternion.identity, towerRoot.transform);
				tile.occupied = true;
			}
			mouseIsDown = true;
		} else if (!Input.GetMouseButton (0)) {
			mouseIsDown = false;
		}
	}

	private bool CheckValidBuildAtPosition(Vector3 pos, out Tile tile) {
		Vector3 diff = pos - GameManager.S.bottomLeft;
		int col = Mathf.RoundToInt(diff.x);
		int row = Mathf.RoundToInt(diff.y);
		if (row >= GameManager.S.levelMap.GetLength (0) ||
		    col >= GameManager.S.levelMap.GetLength (1)) {
			tile = null;
			return false;
		} else {
			tile = GameManager.S.levelMap [row, col].GetComponent<Tile> ();
			return tile.buildable && !tile.occupied;
		}
	}
}
