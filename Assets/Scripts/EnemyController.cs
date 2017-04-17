using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public int health;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		checkState ();
	}

	bool checkState() {
		if (health <= 0) {
			Destroy (this.gameObject);
		}
		return true;
	}
}
