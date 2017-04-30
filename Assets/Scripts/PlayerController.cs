using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public GameObject shield;
	public GameObject weapon;
	public float speed;
	public float range;
	public int health;
	public bool testing;
	private AudioSource walkResponse;
	private AudioSource attackResponse;
	private AudioSource blockResponse;
	private AudioSource jumpResponse;
	private AudioSource hurtResonse;
	private Animator animator;
	private Rigidbody2D rigidBody2D;
	private UserInput userInput;
	private Text keyText;
	private UserInput.ActionName comboMatch;
	private string defaultKeyText;
	private int index;
	private float timeWalk;
	private float timeDisplay;
	private float timeNoInput;
	private float timeNoInpLimit;
	private float timeAttack;
	private float timeAttackInterval;
	private float timeBlock;
	private float timeCombo;
	private float timeComboLimit;
	private float timeDamaged;
	private float timeDamagedLimit;
	private bool block;
	private bool inputTimerStarted;
	private bool executingCombo;
	private bool takenDamage;
	private bool noSound;

	void Awake() {
		index = 0;
	}
	// Use this for initialization
	void Start ()
	{
		try {
			walkResponse = GameObject.Find ("WalkSoundResponse").GetComponent<AudioSource>();
			attackResponse = GameObject.Find ("AttackSoundResponse").GetComponent<AudioSource>();
			blockResponse = GameObject.Find ("BlockSoundResponse").GetComponent<AudioSource>();
			jumpResponse = GameObject.Find ("JumpSoundResponse").GetComponent<AudioSource>();
			hurtResonse = GameObject.Find ("HurtSoundResponse").GetComponent<AudioSource>();
			noSound = false;
		} catch (MissingReferenceException e ) {
			noSound = true;
		}
		comboMatch = UserInput.ActionName.None;
		keyText = (Text)GameObject.Find ("KeyText").GetComponent<Text> ();
		defaultKeyText = "Combo: ";
		keyText.text = defaultKeyText;
		animator = this.GetComponent<Animator> ();
		rigidBody2D = this.GetComponent<Rigidbody2D> ();
		userInput = new UserInput ();
		timeWalk = 0;
		timeNoInput = 0;
		timeNoInpLimit = 0.7f;
		timeAttack = 0;
		timeAttackInterval = 0;
		timeDisplay = 0;
		timeBlock = 0;
		timeCombo = 2.0f;
		timeComboLimit = 2.0f;
		timeDamaged = 0;
		timeDamagedLimit = 0.5f;
		block = false;
		inputTimerStarted = false;
		executingCombo = false;
		takenDamage = false;
	}
 
	// Update is called once per frame
	void Update ()
	{
		if (!CameraController.intro) {
			commandUnits ();
			checkState ();
			displayKeyList ();
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Enemy") {
			int dam = other.gameObject.GetComponent<EnemyController> ().damage;
			receiveDamage (dam);
		}
	}

	public void setIndex(int i) {
		index = i;
	}

	void receiveDamage(int damage) {
		if (!block) {
			takenDamage = true;
			timeDamaged = 0;
			health -= damage;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (0.75f, 0.75f, 0.75f, 0.85f);
			if (!noSound) {
				hurtResonse.Play ();
			}
		}
	}

	void commandUnits ()
	{
		bool inputBool = false;
		bool acceptBool = Rhythm_System.accept;

		if (!executingCombo) { // not already executing a combo
			if (acceptBool) { // time when input is accepted
				inputBool = userInput.checkInputKey ();
				if (inputBool) {
					inputTimerStarted = true;
					timeNoInput = 0;
				}
			}
		}

		incrTimer (inputTimerStarted, userInput); 		// increment timer once input is accepted

		if (inputBool) { 					// if acceptable input entered
			comboMatch = userInput.compare ();		// check if macthes combo
			if (comboMatch != UserInput.ActionName.None) { 	// display last element in keyList
				displayKeyList (); 			// display current keyList
				timeDisplay = 2*timeNoInpLimit;
				userInput.reset (); 			// reset keyList
			}
			switch (comboMatch) {
			case UserInput.ActionName.Walk:
				timeCombo = 0;
				walkResponse.Play ();
				oneState ("Walk");
				float y = rigidBody2D.velocity [1];
				rigidBody2D.velocity = new Vector2 (speed, y);
				break;
			case UserInput.ActionName.Jump:
				timeCombo = 0;
				jumpResponse.Play ();
				float x = rigidBody2D.velocity [0];
				rigidBody2D.velocity = new Vector2 (x, speed * 2.5f);
				break;
			case UserInput.ActionName.Attack:
				timeCombo = 0;
				attackResponse.Play ();
				oneState ("Attack");
				break;
			case UserInput.ActionName.Block:
				timeCombo = 0;
				blockResponse.Play ();
				oneState ("Block");
				summonShield ();
				break;
			default:
				break;
			}
		}
			
	}
		

	void oneState(string state) {
		if (state != "Block") { // block animation state doesn't exist
			animator.SetBool (state, true);
		}
		if (state == "Walk") {
			animator.SetBool ("Attack", false);
			block = false;
		} else if (state == "Attack") {
			animator.SetBool ("Walk", false);
			block = false;
		} else if (state == "Block") {
			animator.SetBool ("Walk", false);
			animator.SetBool ("Attack", false);
		}
	}

	void summonShield() {
		GameObject go = (GameObject)Instantiate (shield, transform.position, Quaternion.identity);
		go.GetComponent<Shield> ().setShieldTimeLimit (timeComboLimit);
		go.GetComponent<Shield> ().setParent (this.gameObject);
		block = true;
	}

	void fireAttack() {
		Vector2 position = transform.position;
		Vector2 target = position + new Vector2 (range, 0);
		Vector2 trajectory = Utility.ballisticVel (position, target, 55.0f);
		Vector2 offset = position + new Vector2 (0, 2.0f);
		Vector2 midPoint2 = (target) / 2;
		Vector3 midPoint3 = midPoint2;
		midPoint3.z = 20f;
		GameObject wep = (GameObject)Instantiate (weapon, offset, Quaternion.identity);
		wep.GetComponent<Rigidbody2D> ().velocity = trajectory;
	}

	void checkState ()
	{
		float y = rigidBody2D.velocity [1];
		if (health <= 0) {
			setInactive ();
			Destroy (this.gameObject);
		}
		if (animator.GetBool ("Walk") && timeWalk < timeComboLimit) {
			timeWalk += Time.deltaTime;
			//Debug.Log ("Timewalking: " + timeWalk + "\n");
		} else if (timeWalk >= timeComboLimit) {
			animator.SetBool ("Walk", false);
			timeWalk = 0;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, y);
		}

		if (animator.GetBool("Attack") && timeAttack < timeComboLimit) {
			if (timeAttackInterval >= 0.5f) {
				fireAttack ();
				timeAttackInterval = 0;
			}
			timeAttackInterval += Time.deltaTime;
			timeAttack += Time.deltaTime;
		} else if (timeAttack >= timeComboLimit) {
			animator.SetBool("Attack", false);
			timeAttack = 0;
		}
		 
		if (block && timeBlock < timeComboLimit) {
			timeBlock += Time.deltaTime;
		} else if (timeBlock >= timeComboLimit) {
			block = false;
			timeBlock = 0;
		}
			
		if (timeCombo < timeComboLimit) { // indicate that user should not be allowed to input
			executingCombo = true;
			timeCombo += Time.deltaTime;
		} else if (timeCombo >= timeComboLimit) { // indicate that user can input
			timeCombo = timeComboLimit;
			executingCombo = false;
		}

		if (takenDamage && timeDamaged < timeDamagedLimit) {
			timeDamaged += Time.deltaTime;
		} else if (timeDamaged >= timeDamagedLimit) {
			takenDamage = false;
			timeDamaged = 0;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
		}

	}

	bool isShielded() {
		return block;
	}

	bool incrTimer (bool started, UserInput input)
	{
		if (started) {
			timeNoInput += Time.deltaTime;
		} else {
			return false;
		}

		if (timeNoInput > timeNoInpLimit) {
			timeNoInput = 0;
			input.reset ();
			inputTimerStarted = false;
			Debug.Log ("RESET");
			return false;
		}
		return true;
	}

	/*
	 * call this before destroying object to keep from crashing when calculating camera position
	 */
	void setInactive() {
		if (!testing) {
			PlayerManager.activeObjects [index] = false;
		}
	}

	void displayKeyList ()
	{
		if (timeDisplay > timeNoInpLimit) { // keep current text display
			timeDisplay -= Time.deltaTime; 
		} else { // change text display
			char[] keyList = userInput.getKeyList ();
			string s = Utility.toString (keyList);
			s = s.ToUpper ();
			if (s.Length != 0) {
				setKeyText (defaultKeyText + s);
			} else {
				setKeyText (defaultKeyText);
			}
			timeDisplay = 0;
		}
	}

	void setKeyText (string text)
	{
		keyText.text = text;
	}
}
