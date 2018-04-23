using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float speed;
	public GameObject target;
	public float damage;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// If target has gone inactive (dying, etc) find a new target
		if (target == null) {
			Enemy[] enemies = FindObjectsOfType<Enemy> ();

			target = null;
			foreach (Enemy e in enemies) {
				if (target == null ||
					Vector3.Distance (transform.position, e.gameObject.transform.position)
					< Vector3.Distance (transform.position, target.transform.position)) {
					target = e.gameObject;
				}
			}

			// If no available targets, destroy this projectile
			if (target == null) {
				Destroy (gameObject);
			}
		}

		if (target != null) {
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, speed * Time.deltaTime);
		}
	}
		
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.Equals (target)) {
			Destroy (gameObject);
		}
	}
}
