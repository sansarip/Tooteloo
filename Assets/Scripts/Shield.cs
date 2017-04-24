using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
	public float knockback;
	private GameObject player;
	private float shieldTime;
	private float shieldTimeLimit;
	private bool set;

	void Awake() {
		player = new GameObject ();
		shieldTime = 0;
		shieldTimeLimit = 2.0f;
		set = false;
	}

	// Use this for initialization
	void Start () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!other.isTrigger && other.gameObject.tag == "Enemy") {
			Debug.Log ("TRIGGERED");
			other.GetComponent<EnemyController> ().knockBack(knockback);
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Debug.Log ("set: " + set);
		if (set) {
			if (player != null) {
				transform.position = player.transform.position;
			}
			checkState ();
		}
	}

	public void setParent (GameObject parent) {
		set = true;
		if (parent != null) {
			player = parent;
		}

	}

	public void setShieldTimeLimit(float t) {
		shieldTimeLimit = t;
	}

	private void checkState() {
		if (shieldTime < shieldTimeLimit) {
			shieldTime += Time.deltaTime;
		} else if (shieldTime >= shieldTimeLimit) {
			shieldTime = 0;

			Destroy(this.gameObject);
		}
	}
}
