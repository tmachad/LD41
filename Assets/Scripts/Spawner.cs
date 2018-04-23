using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject enemy;
	public int amount;
	public float spawnRate = 0.1f;

	private float spawnDelay;
	private bool unlimited = false;
	private int count = 0;
	private int nextUpgrade = 10;

	private float healthUpgrade = 0.0f;
	private float speedUpgrade = 0.0f;
	private float bountyUpgrade = 0.0f;

	// Use this for initialization
	void Start () {
		spawnDelay = 1.0f / spawnRate;
		if (amount <= 0) {
			unlimited = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (spawnDelay <= 0 && (unlimited || amount > 0)) {
			GameObject e = Instantiate (enemy) as GameObject;
			e.transform.position = gameObject.transform.position;
			Enemy en = e.GetComponent<Enemy> ();
			en.maxHealth += healthUpgrade;
			en.health = en.maxHealth;
			en.speed += speedUpgrade;
			en.bounty += Mathf.RoundToInt (bountyUpgrade);

			if (!unlimited) {
				amount--;
			}

			count++;
			if (count >= nextUpgrade) {
				nextUpgrade = Mathf.RoundToInt(nextUpgrade * 1.1f);
				count = 0;

				Enemy prefab = enemy.GetComponent<Enemy> ();

				int rand = Random.Range (0, 5);
				switch (rand) {
				case 0:
					Debug.Log ("Upgraded health");
					healthUpgrade += prefab.maxHealth * 0.1f;
					break;
				case 1:
					Debug.Log ("Upgraded speed");
					speedUpgrade += prefab.speed * 0.1f;
					break;
				case 2:
					Debug.Log ("Upgraded spawnrate");
					spawnRate *= 1.05f;
					break;
				case 3:
					Debug.Log ("Upgraded bounty");
					bountyUpgrade += prefab.bounty * 0.1f;
					break;
				case 4:
					Debug.Log ("Upgrading nothing");
					break;
				default:
					Debug.Log ("Default case hit upgrading enemies. This shouldn't happen");
					break;
				}
			}

			spawnDelay = 1.0f / spawnRate;
		} else {
			spawnDelay -= Time.deltaTime;
		}
	}
}
