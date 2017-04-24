using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {
	public static bool endGame;

	// Use this for initialization
	void Start () {
		endGame = false;
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (!other.isTrigger && other.gameObject.tag == "Player" && EnemyManager.numEnemies == 0) {
			endGame = true;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
