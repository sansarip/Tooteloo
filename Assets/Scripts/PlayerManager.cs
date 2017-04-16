using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public Transform[] spawnPoints;
	public GameObject player;
	public static Vector3 averagePosition;
	public int numUnits;
	private GameObject[] playerUnitArray;
	private string defaultKeyText;

	// Use this for initialization
	void Start ()
	{
		playerUnitArray = new GameObject[numUnits];
		defaultKeyText = "Combo: ";
		for (int i = 0; i < spawnPoints.Length; i++) {
			playerUnitArray [i] = (GameObject)Instantiate (player, spawnPoints [i].position, spawnPoints [i].rotation);
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
