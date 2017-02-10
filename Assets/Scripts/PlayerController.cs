using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();	
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		if (horizontal > 0) {
			animator.SetBool ("Walk", true);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (2.0f, 0);

		} else if (horizontal == 0) {
			animator.SetBool ("Walk", false);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		}
	}
}
