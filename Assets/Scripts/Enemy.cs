using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float speed = 0.5f;

	public float maxHealth = 10f;

	[HideInInspector]
	public float health;

	public int bounty = 10;

	[SerializeField]
	private Texture2D hitPointsBarTexture;

	private float healthBarWidth = 0.5f;
	private float healthBarOffset = 0.25f;

	private Vector3[] path;
	private int pathIndex = 0;

	private float pixelsPerMeter;

	// Use this for initialization
	void Start () {
		path = GameManager.S.waypoints;
		pixelsPerMeter = Camera.main.pixelHeight / Camera.main.orthographicSize;
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (pathIndex < path.Length) {
			transform.position = Vector3.MoveTowards (transform.position, path [pathIndex], speed * Time.deltaTime);

			if (transform.position == path [pathIndex]) {
				pathIndex++;
			}
		}
	}

	void OnGUI() {
		if (health < maxHealth) {
			Vector3 worldPos = transform.position;
			worldPos.y *= -1;
			Vector3 screenPos = Camera.main.WorldToScreenPoint (worldPos);
			GUI.DrawTexture (new Rect (screenPos.x - (healthBarWidth / 2 * pixelsPerMeter), screenPos.y - healthBarOffset * pixelsPerMeter, healthBarWidth * pixelsPerMeter * health / maxHealth, 2), hitPointsBarTexture);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Bullet") {
			TakeDamage (other.GetComponent<Projectile> ().damage);
		} else if (other.tag == "Goal") {
			Destroy (gameObject);
		}
	}

	void TakeDamage(float damage) {
		health -= damage;
		if (health <= 0) {
			GameManager.S.ModifyMoney (bounty);
			Destroy (gameObject);
		}
	}
}
