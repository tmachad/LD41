using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	[SerializeField]
	private GameObject mapRoot;

	public GameObject pathTile;
	public GameObject buildableTile;
	public GameObject spawnerTile;
	public GameObject goalTile;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static bool[,] Generate(int width, int height, int pathLength) {
		bool[,] levelMap = new bool[height, width];	// rows * columns

		Vector2 cursor;

		int startingSide = Random.Range (0, 4);
		switch (startingSide) {
		case 0:	// left side
			cursor = new Vector2 (0, Random.Range (1, height - 1));
			break;
		case 1:	// top side
			cursor = new Vector2 (Random.Range (1, width - 1), 0);
			break;
		case 2:	// right side
			cursor = new Vector2 (width - 1, Random.Range (1, height - 1));
			break;
		case 3:	// bottom side
			cursor = new Vector2 (Random.Range (1, width - 1), height - 1);
			break;
		default:
			cursor = new Vector2 (0, Random.Range (1, height - 1));
			Debug.Log ("Hit default case when generating level! This should never happen!");
			break;
		}
			
		for (int i = 0; i < pathLength; i++) {
			levelMap [(int)cursor.y, (int)cursor.x] = true;
			int direction = Random.Range (0, 3);

		}

		return null;
	}

	public GameObject[,] Generate(int width, int height, Vector3 bottomLeft, Vector3[] path) {
		GameObject[,] levelMap = new GameObject[height, width];

		for (int i = 0; i < path.Length; i++) {
			Vector3 point = path [i];
			GameObject tile;
			if (i == 0) {
				tile = Instantiate (spawnerTile, mapRoot.transform) as GameObject;
			} else if (i == path.Length - 1) {
				tile = Instantiate (goalTile, mapRoot.transform) as GameObject;
			} else {
				tile = Instantiate (pathTile, mapRoot.transform) as GameObject;
			}

			Vector3 pos = bottomLeft + new Vector3((int)point.x, (int)point.y, 0);
			tile.transform.position = pos;
			levelMap [(int)point.y, (int)point.x] = tile;
		}

		for (int row = 0; row < height; row++) {
			for (int col = 0; col < width; col++) {
				if (levelMap [row, col] == null) {
					GameObject tile = Instantiate (buildableTile, mapRoot.transform) as GameObject;
					Vector3 pos = bottomLeft + new Vector3(col, row, 0);
					tile.transform.position = pos;
					levelMap [row, col] = tile;
				}
			}
		}

		return levelMap;
	}
}
