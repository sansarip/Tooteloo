using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static bool[] activeObjects;
	public Transform[] spawnPoints;
	public GameObject player;
	public static Vector3 averagePosition;
	public int numUnits;
	private GameObject[] playerUnitArray;


	// Use this for initialization
	void Start ()
	{
		activeObjects = new bool[numUnits];
		playerUnitArray = new GameObject[numUnits];
		for (int i = 0; i < spawnPoints.Length; i++) {
			playerUnitArray [i] = (GameObject)Instantiate (player, spawnPoints [i].position, spawnPoints [i].rotation);
			playerUnitArray [i].GetComponent<PlayerController> ().setIndex (i);
			activeObjects [i] = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		averagePosition = averagePositions (playerUnitArray);
	}

	Vector3 averagePositions (GameObject[] goArray)
	{
		int length = goArray.Length;
		int numActive = 0; // number of active objects
		if (length == 0) {
			Debug.Log ("empty GameObject array\n");
			return new Vector2 (0, 0);
		}
		float x = 0;
		float y = 0;
		float z = 0;
		for (int i = 0; i < length; i++) {
			if (activeObjects [i]) {
				GameObject go = goArray [i];
				x += go.transform.position [0];
				y += go.transform.position [1];
				z += go.transform.position [2];
				numActive++;
			} 
		}
		x /= numActive;
		y /= numActive;
		z /= numActive;
		return new Vector3 (x, y, z);
	}
		
}
