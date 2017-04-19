using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public GameObject weapon;
	public float speed;
	public float range;
	public int health;
	private Animator animator;
	private Rigidbody2D rigidBody2D;
	private UserInput userInput;
	private Text keyText;
	private UserInput.ActionName comboMatch;
	private string defaultKeyText;
	private float timeWalk;
	private float timeDisplay;
	private float timeNoInput;
	private float timeNoInpLimit;
	private float timeWalkLimit;
	private bool inputTimerStarted;

	// Use this for initialization
	void Start ()
	{
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
		timeWalkLimit = 2.0f;
		timeDisplay = 0;
		inputTimerStarted = false;
	}
 
	// Update is called once per frame
	void Update ()
	{
		commandUnits ();
		checkState ();
		displayKeyList ();
		Debug.Log (Rhythm_System.accept);
	}

	void commandUnits ()
	{
		bool inputBool = false;
		bool acceptBool = Rhythm_System.accept;

		if (acceptBool) { // time when input is accepted
			inputBool = userInput.checkInputKey ();
			if (inputBool) {
				inputTimerStarted = true;
				timeNoInput = 0;
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
				animator.SetBool ("Walk", true);
				float y = rigidBody2D.velocity [1];
				rigidBody2D.velocity = new Vector2 (speed, y);
				break;
			case UserInput.ActionName.Jump:
				float x = rigidBody2D.velocity [0];
				rigidBody2D.velocity = new Vector2 (x, speed * 4);
				break;
			case UserInput.ActionName.Attack:
				Vector2 position = transform.position;
				Vector2 target = position + new Vector2 (range, 0);
				Vector2 trajectory = Utility.ballisticVel (position, target, 55.0f);
				Vector2 offset = position + new Vector2 (0, 2.0f);
				Vector2 midPoint2 = (target) / 2;
				Vector3 midPoint3 = midPoint2;
				midPoint3.z = 20f;
				GameObject wep = (GameObject) Instantiate (weapon, offset, Quaternion.AngleAxis(-80.0f, midPoint3));
				wep.GetComponent<Rigidbody2D> ().velocity = trajectory;
				wep.GetComponent<Spear> ().invokeRotation (midPoint3);
				break;
			default:
				break;
			}
		}
			
	}

	void checkState ()
	{
		float y = 0;
		if (animator.GetBool ("Walk") && timeWalk < timeWalkLimit) {
			timeWalk += Time.deltaTime;
			//Debug.Log ("Timewalking: " + timeWalk + "\n");
		} else if (timeWalk >= timeWalkLimit) {
			animator.SetBool ("Walk", false);
			timeWalk = 0;
			y = rigidBody2D.velocity [1];
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, y);
		}
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
