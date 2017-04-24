using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour {
	public Transform start;
	public Transform end;
	public float speed;
	// Use this for initialization
	void Start () {
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
	}

	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (transform.position.x - end.position.x) <= 0.1f) {
			Vector2 pos = new Vector2 (start.position.x, transform.position.y);
			transform.position = pos;
		}
	}
}
