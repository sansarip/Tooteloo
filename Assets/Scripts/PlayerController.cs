using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public int health;
	private Animator animator;
	private Rigidbody2D rigidBody2D;
	private float timewalking;
	private float timeLimit;
	private UserInput userInput;

	// Use this for initialization
	void Start ()
	{
		animator = this.GetComponent<Animator> ();
		rigidBody2D = this.GetComponent<Rigidbody2D> ();
		userInput = new UserInput ();
		timewalking = 0;
		timeLimit = 2.0f;
	}
 
	// Update is called once per frame
	void Update ()
	{
		userInput.checkInputKey ();
		UserInput.ActionName comboMatch = userInput.compareAndReset ();
		if (comboMatch == UserInput.ActionName.Walk) {
			animator.SetBool ("Walk", true);
			float y = rigidBody2D.velocity [1];
			rigidBody2D.velocity = new Vector2 (speed, y);
		} else if (comboMatch == UserInput.ActionName.Jump) {
			float x = rigidBody2D.velocity [0];
			rigidBody2D.velocity = new Vector2 (x, speed * 4);
		}
		if (animator.GetBool ("Walk") && timewalking < timeLimit) {
			timewalking += Time.deltaTime;
			Debug.Log ("Timewalking: " + timewalking + "\n");
		} else if (timewalking >= timeLimit) {
			animator.SetBool ("Walk", false);
			timewalking = 0;
			float y = rigidBody2D.velocity [1];
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, y);
		}
	}



}
