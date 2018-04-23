using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public int startingLives = 10;
	private int lives;

	public int money = 0;

	public Text livesText;
	public GameObject gameOverPanel;
	public Text moneyText;

	[HideInInspector]
	public bool gameOver = false;

	[HideInInspector]
	public static GameManager S;

	[HideInInspector]
	public GameObject[,] levelMap;

	[HideInInspector]
	public Vector3 bottomLeft;

	[HideInInspector]
	public Vector3[] waypoints;

	// Use this for initialization
	void Start () {
		S = this;

		float camHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
		float camHalfHeight = Camera.main.orthographicSize;
		bottomLeft = Camera.main.transform.position - new Vector3 (camHalfWidth, camHalfHeight, 0);
		bottomLeft.z = 0;
		bottomLeft.x += 0.5f;
		bottomLeft.y += 0.5f;

		InitializeGame ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitializeGame() {
		lives = startingLives;
		livesText.text = "Lives: " + lives;

		moneyText.text = "Money: $" + money;

		gameOver = false;

		gameOverPanel.SetActive (false);

		Vector3[] path = new Vector3[] {
			new Vector3(0, 5),
			new Vector3(1, 5),
			new Vector3(2, 5),
			new Vector3(3, 5),
			new Vector3(3, 4),
			new Vector3(3, 3),
			new Vector3(3, 2),
			new Vector3(3, 1),
			new Vector3(4, 1),
			new Vector3(5, 1),
			new Vector3(5, 2),
			new Vector3(5, 3),
			new Vector3(5, 4),
			new Vector3(5, 5),
			new Vector3(6, 5),
			new Vector3(7, 5),
			new Vector3(8, 5),
			new Vector3(9, 5),
			new Vector3(10, 5),
			new Vector3(11, 5),
			new Vector3(12, 5),
			new Vector3(12, 6),
			new Vector3(12, 7),
			new Vector3(12, 8),
			new Vector3(11, 8),
			new Vector3(10, 8),
			new Vector3(9, 8),
			new Vector3(9, 7)
		};

		levelMap = gameObject.GetComponent<LevelGenerator> ().Generate (14, 10, bottomLeft, path);

		List<Vector3> tempWaypoints = new List<Vector3> ();
		foreach (Vector3 point in path) {
			tempWaypoints.Add (levelMap [Mathf.RoundToInt(point.y), Mathf.RoundToInt(point.x)].transform.position);
		}
		waypoints = tempWaypoints.ToArray ();
	}

	public void ResetGame() {
		// Wipe out the map
		for (int row = 0; row < levelMap.GetLength (0); row++) {
			for (int col = 0; col < levelMap.GetLength (1); col++) {
				Destroy (levelMap [row, col]);
			}
		}

		Enemy[] enemies = FindObjectsOfType<Enemy> ();

		for (int i = 0; i < enemies.Length; i++) {
			Destroy (enemies [i].gameObject);
		}

		Tower[] towers = FindObjectsOfType<Tower> ();

		for (int i = 0; i < towers.Length; i++) {
			Destroy (towers [i].gameObject);
		}

		InitializeGame ();

		Time.timeScale = 1.0f;
	}

	public void ModifyLives(int amount) {
		lives += amount;
		livesText.text = "Lives: " + lives;
		if (lives <= 0) {
			gameOver = true;
			gameOverPanel.SetActive (true);
			Time.timeScale = 0.0f;
		}
	}

	public void ModifyMoney(int amount) {
		money += amount;
		moneyText.text = "Money: $" + money;
	}
}
