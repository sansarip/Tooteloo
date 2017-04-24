using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public int health;
	public int speed;
	public int range;
	public int damage;
	private Animator animator;
	private Rigidbody2D rigidBody2D;
	private Vector2 targetPos;
	private int randNum;
	private int randDel;
	private int timeIdleMoveLimit;
	private float del;
	private float timeIdleMove;
	private float timeMoveLimit;
	private float timeMove;
	private float timeDead;
	private float timeDeadLimit;
	private float timeMoveAway;
	private float timeMoveAwayLimit;
	private float timeMoveTo;
	private float timeMoveToLimit;
	private float timeDamaged;
	private float timeDamagedLimit;
	private float timeKnockedBack;
	private float timeKnockedBackLimit;
	private bool didOnce;
	private bool move;
	private bool target;
	private bool moveAway;
	private bool moveTo;
	private bool takenDamage;
	private bool arrivedTo;
	private bool knockedBack;
	private enum State {
		MOVE_TO,
		MOVE_AWAY,
		IDLE
	}
	private State state;
	private float yPos;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		rigidBody2D = this.GetComponent<Rigidbody2D> ();
		targetPos = new Vector2 ();
		yPos = transform.position [1];
		randNum = 0;
		randDel = (int) Random.Range (8f, 16f); // begin moving after delay
		del = 0;
		timeIdleMoveLimit = 8; // move every 8 seconds
		timeMoveLimit = 1.0f;
		timeMove = 0;
		timeDead = 0;
		timeDeadLimit = 0.5f;
		timeDamaged = 0;
		timeDamagedLimit = 0.5f;
		timeMoveAway = 0;
		timeMoveAwayLimit = 1.0f;
		timeMoveTo = 0;
		timeMoveToLimit = 2.0f;
		timeKnockedBack = 0;
		timeKnockedBackLimit = 0.1f;
		didOnce = false;
		move = false;
		target = false;
		takenDamage = false;
		arrivedTo = false;
		knockedBack = false;
		state = State.IDLE;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!CameraController.intro) {
			moveToTarget ();
			checkState ();
			moveAwayFromTarget ();
			idleMove ();
		}

	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (!target) {
				Debug.Log ("TARGET");
				target = true;
				targetPos = other.transform.position;
			}
		}
	}

	/* idleMove moves unit in random direction left or right a set distance
	 * begins moving after a set delay, executes continuously after every interval
	 */
	void idleMove () {
		if (health > 0) {
			if (del >= randDel) {
				if (timeIdleMove >= timeIdleMoveLimit) {
					timeIdleMove = 0;
					float y = 0;
					if (!target) { // no target in sight
						state = State.IDLE;
						randNum = (int)Random.Range (0f, 10f) % 2;
						if (randNum == 0) { // move left
							move = true;
							rigidBody2D.velocity = -(new Vector2 (speed, y));
						} else { // move right
							move = true;
							rigidBody2D.velocity = new Vector2 (speed, y);
						}
					}
				}
				timeIdleMove += Time.deltaTime;
			}
			if (del < randDel) {
				del += Time.deltaTime;
			}
		}
	}

	void moveToTarget () {
		if (health > 0) {
			if (target && state != State.MOVE_TO && state != State.MOVE_AWAY) {
				float y = 0;
				rigidBody2D.velocity = -(new Vector2 (speed * 2, y)); // move left
				state = State.MOVE_TO;
			}
		}
	}

	void moveAwayFromTarget () {
		if (health > 0) {
			if (arrivedTo && state != State.MOVE_AWAY) {  
				Debug.Log ("MOVEAWAY");
				arrivedTo = false;
				float y = 0;
				rigidBody2D.velocity = new Vector2 (speed * 2, y);
				state = State.MOVE_AWAY;
			}
		}
	}
		

	private void checkState() {
		float y = rigidBody2D.velocity [1];
		if (health <= 0) {
			if (!didOnce) {
				animator.SetBool ("Dead", true);
				rigidBody2D.gravityScale = 1;
				this.GetComponents<BoxCollider2D> () [0].enabled = false;
				this.GetComponents<BoxCollider2D> () [1].enabled = false;
				this.GetComponent<SpriteRenderer> ().sortingOrder = 0;
				rigidBody2D.velocity = new Vector2 (0, 0);
				EnemyManager.numEnemies -= 1;
			}
			didOnce = true;
			if (timeDead >= timeDeadLimit) {
				rigidBody2D.gravityScale = 0;
				rigidBody2D.velocity = new Vector2 (0, 0);
				timeDead = timeDeadLimit;
				//Destroy (this.gameObject);
			}
			timeDead += Time.deltaTime;
		}

		if (move && timeMove < timeMoveLimit) { // check idle moving
			timeMove += Time.deltaTime;
		}
		else if (timeMove >= timeMoveLimit) {
			timeMove = 0;
			move = false;
			stopMovement();
		}

		if (state == State.MOVE_TO && timeMoveTo < timeMoveToLimit) { // check moving to target
			timeMoveTo += Time.deltaTime;
		} else if (timeMoveTo >= timeMoveToLimit) {
			timeMoveTo = 0;
			arrivedTo = true;
			stopMovement();
		}

		if (state == State.MOVE_AWAY && timeMoveAway < timeMoveAwayLimit) { // check moving away 
			timeMoveAway += Time.deltaTime;
		} else if (timeMoveAway >= timeMoveAwayLimit) {
			timeMoveAway = 0;
			target = false; // ready to acquire new target
			state = State.IDLE;
			stopMovement();
		}

		if (takenDamage && timeDamaged < timeDamagedLimit) {
			timeDamaged += Time.deltaTime;
		} else if (!animator.GetBool("Dead") && timeDamaged >= timeDamagedLimit) {
			takenDamage = false;
			timeDamaged = 0;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
		}
			

		if (knockedBack && timeKnockedBack < timeKnockedBackLimit) {
			timeKnockedBack += Time.deltaTime;
		} else if (knockedBack && timeKnockedBack >= timeKnockedBackLimit) {
			if (Mathf.Abs (transform.position [1] - yPos) <= 0.1f) {
				knockedBack = false;
				timeKnockedBack = 0;
				rigidBody2D.gravityScale = 0;
				rigidBody2D.velocity = new Vector2 (0, 0);
			}
		}
			
	}

	private void stopMovement() {
		float y = 0;
		rigidBody2D.velocity = new Vector2 (0, y);
	}

	public void receiveDamage(int damage) {
		takenDamage = true;
		timeDamaged = 0;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (0.75f, 0.75f, 0.75f, 0.85f);
		health -= damage;
	}

	public void knockBack(float distance) {
		if (!knockedBack) {
			knockedBack = true;
			arrivedTo = false;
			Vector2 position = transform.position;
			Vector2 target = position + new Vector2 (distance, 0);
			Vector2 trajectory = Utility.ballisticVel (position, target, 45.0f);
			rigidBody2D.gravityScale = 1;
			rigidBody2D.velocity = trajectory;
		}
	}
}
