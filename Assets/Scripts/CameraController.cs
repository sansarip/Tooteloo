using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static bool intro;
	public GameObject endLevelBoundary;
	public float xOffset;
	public float yOffset;
	public float zOffset;
	private float speed = 0.25f;
	public static Vector3 position;
	private Vector3 endLevelBoundaryPosition;
	private bool panBack;
	// Use this for initialization
	void Start ()
	{
		endLevelBoundaryPosition = endLevelBoundary.transform.position;
		position = new Vector3 (transform.position.x, transform.position.y + yOffset, transform.position.z + zOffset);
		transform.position = position;
		intro = true;
		panBack = false;
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		if (intro == true) {
			updatePositionToEndAndBack ();
		} else {
			updatePositionToPlayers ();
		}
		
	}

	void updatePositionToEndAndBack() {
		if (!panBack) {
			Vector3 pos = transform.position;
			float x = pos [0];
			float y = pos [1];
			float z = pos [2];
			transform.position = new Vector3 (x + speed, y, z);
			pos = transform.position;
			if (Mathf.Abs (pos [0] - Mathf.Abs(endLevelBoundaryPosition [0] - xOffset)) <= 0.25f) {
				panBack = true;
			}
		} else {
			Vector3 pos = transform.position;
			float x = pos [0];
			float y = pos [1];
			float z = pos [2];
			transform.position = new Vector3 (x - speed, y, z);
			pos = transform.position;
			if (Mathf.Abs (pos [0] - (PlayerManager.averagePosition [0]+xOffset)) <= 0.25f) {
				panBack = false;
				intro = false;
			}
		}
	}

	void updatePositionToPlayers() {
		Vector3 newPosition = PlayerManager.averagePosition;
		newPosition.x += xOffset;				// x offset
		newPosition.y = transform.position.y;
		newPosition.z = transform.position.z;
		position = newPosition;
		try {
			transform.position = newPosition;
		} catch (UnityException e) {
		
		}
	}
}
