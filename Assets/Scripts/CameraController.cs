using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float xOffset;
	public float yOffset;
	public float zOffset;

	// Use this for initialization
	void Start ()
	{
		transform.position = new Vector3 (transform.position.x, transform.position.y + yOffset, transform.position.z + zOffset);
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		Vector3 newPosition = PlayerManager.averagePosition;
		newPosition.x += xOffset;					// x offset
		newPosition.y = transform.position.y;
		newPosition.z = transform.position.z;
		transform.position = newPosition;
	}
}
