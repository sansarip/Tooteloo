using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public static int numEnemies;
	public Transform[] spawnPoints;
	public GameObject enemy;
	//public static Vector3 averagePosition;
	public int numUnits;
	private GameObject[] enemyUnitArray;

	// Use this for initialization
	void Start ()
	{
		enemyUnitArray = new GameObject[numUnits];
		numEnemies = numUnits;
		for (int i = 0; i < spawnPoints.Length; i++) {
			enemyUnitArray [i] = (GameObject)Instantiate (enemy, spawnPoints [i].position, spawnPoints [i].rotation);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//averagePosition = averagePositions (enemyUnitArray);
	}

	void OnTriggerEnter2D ( Collider2D other ) {
	
	}

	Vector3 averagePositions (GameObject[] goArray)
	{
		int length = goArray.Length;
		if (length == 0) {
			Debug.Log ("empty GameObject array\n");
			return new Vector2 (0, 0);
		}
		float x = 0;
		float y = 0;
		float z = 0;
		for (int i = 0; i < length; i++) {
			GameObject go = goArray [i];
			x += go.transform.position [0];
			y += go.transform.position [1];
			z += go.transform.position [2];
		}
		x /= length;
		y /= length;
		z /= length;
		return new Vector3 (x, y, z);
	}
}
