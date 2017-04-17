using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour {
	public int damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if(other.gameObject.tag.Equals("Enemy")) {
			int health = other.GetComponent<EnemyController> ().health;
			other.GetComponent<EnemyController> ().health = health - damage;
			Destroy (this.gameObject);
		}
	}
}
