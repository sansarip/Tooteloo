using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour {
	public int damage;
	private Vector3 midPoint;
	private bool invoke;

	// Use this for initialization
	void Start () {
		Vector3 midPoint;
		midPoint = new Vector3 ();
		invoke = false;
		Destroy (this.gameObject, 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (invoke) {
			rotateAxis ();
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (!other.isTrigger) {
			if (other.gameObject.tag.Equals ("Enemy")) {
				int health = other.GetComponent<EnemyController> ().health;
				other.GetComponent<EnemyController> ().health = health - damage;
				invoke = false;
				Destroy (this.gameObject);
			}
		}
	}

	public void rotateAxis() {
		Vector3 diff = midPoint - transform.position;
		diff.Normalize ();
		float rot_z = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;
		//this.gameObject.GetComponent<Rigidbody2D> ().AddTorque (transform.up.y * 2 * Time.deltaTime);
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z-90);
	}

	public void invokeRotation (Vector3 mp) {
		invoke = true;
		midPoint = mp;
	}
}
