using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	public float fireRate = 1.0f;
	public float damage = 1.0f;
	public int cost = 10;
	public GameObject projectile;
	public GameObject projectileRoot;

	public float range = 5.0f;
	public int lineCount = 50;

	private float fireDelay;

	private LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
		fireDelay = 1.0f / fireRate;

		// Render range indicator
		lineRenderer = gameObject.AddComponent<LineRenderer> ();
		lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer.startWidth = 0.05f;
		lineRenderer.endWidth = 0.05f;
		lineRenderer.positionCount = lineCount;
		lineRenderer.sortingLayerName = "Indicators";
		lineRenderer.loop = true;

		Vector3 point;
		float angle = 0f;
		float angleInc = (2.0f * Mathf.PI) / lineCount;
		for (int i = 0; i < lineCount; i++) {
			angle += angleInc;
			float x = range * Mathf.Cos (angle);
			float y = range * Mathf.Sin (angle);
			x += gameObject.transform.position.x;
			y += gameObject.transform.position.y;
			point = new Vector3 (x, y);
			lineRenderer.SetPosition (i, point);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fireDelay <= 0) {
			Enemy[] enemies = FindObjectsOfType<Enemy> ();

			GameObject target = null;

			foreach (Enemy e in enemies) {
				Vector3 pos = e.gameObject.transform.position;

				float distanceToNewTarget = Vector3.Distance (transform.position, pos);

				if (distanceToNewTarget <= range && (target == null ||
					distanceToNewTarget < Vector3.Distance (transform.position, target.transform.position))) {
					target = e.gameObject;
				}
			}

			if (target != null) {
				GameObject p = Instantiate (projectile, projectileRoot.transform);
				Projectile proj = p.GetComponent<Projectile> ();
				proj.target = target;
				proj.damage = damage;
			}
				
			fireDelay = 1.0f / fireRate;
		} else {
			fireDelay -= Time.deltaTime;
		}



		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;
		if (Vector3.Distance (transform.position, mousePos) <= 0.5f) {
			lineRenderer.enabled = true;
		} else {
			lineRenderer.enabled = false;
		}
	}

	void OnGUI() {
		
	}
}
