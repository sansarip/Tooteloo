using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public int health;
	public int speed;
	public int range;
	private Rigidbody2D rigidBody2D;
	private Vector2 targetPos;
	private int randNum;
	private int randDel;
	private int interval;
	private float del;
	private float time;
	private float timeMoveLimit;
	private float timeMove;
	private bool move;
	private bool target;


	// Use this for initialization
	void Start () {
		rigidBody2D = this.GetComponent<Rigidbody2D> ();
		targetPos = new Vector2 ();
		randNum = 0;
		randDel = (int) Random.Range (0f, 10f);
		interval = 8;
		timeMoveLimit = 1.0f;
		timeMove = 0;
		move = false;
		target = false;
		//InvokeRepeating ("idleMove", randDel, 8.0f);
	}
	
	// Update is called once per frame
	void Update () {
		checkState ();
		moveToTarget ();
		idleMove ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			Debug.Log ("PLAYER");
			target = true;
			targetPos = other.transform.position;
		}
	}

	/* idleMove moves unit in random direction left or right a set distance
	 * begins moving after a set delay, executes continuously after every interval
	 */
	void idleMove () {
		if (del >= randDel) {
			if (time >= interval) {
				time = 0;
				float y = 0;
				if (!target) {
					randNum = (int)Random.Range (0f, 10f) % 2;
					if (randNum == 0) { // move left
						move = true;
						y = rigidBody2D.velocity [1];
						rigidBody2D.velocity = -(new Vector2 (speed, y));
					} else { // move right
						move = true;
						y = rigidBody2D.velocity [1];
						rigidBody2D.velocity = new Vector2 (speed, y);
					}
				}
			}
			time += Time.deltaTime;
		}
		if (del < randDel) {
			del += Time.deltaTime;
		}
	}

	void moveToTarget () {
		if (target == true) {
			Vector2 pos = transform.position;
			float y = rigidBody2D.velocity [1];
			if (targetPos.x - pos.x < 0) { // target is to left
				rigidBody2D.velocity = -(new Vector2 (speed, y));
			} else if (targetPos.x - pos.x >= 0) {
				rigidBody2D.velocity = new Vector2 (0, y);
				target = false;
			}
		}

	}

	void checkState() {
		if (health <= 0) {
			Destroy (this.gameObject);
		}
		if (move && timeMove < timeMoveLimit) {
			timeMove += Time.deltaTime;
		}
		else if (timeMove >= timeMoveLimit) {
			timeMove = 0;
			move = false;
			float y = rigidBody2D.velocity [1];
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, y);
		}
	}
}
