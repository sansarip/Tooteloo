using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public int health;
	private Animator animator;
	private Rigidbody2D rigidBody2D;
	private UserInput userInput;
	private Text keyText;
	private UserInput.ActionName comboMatch;
	private string defaultKeyText;
	private float timeWalk;
	private float timeNoInput;
	private float timeNoInpLimit;
	private float timeWalkLimit;
	private bool inputTimerStarted;

	// Use this for initialization
	void Start ()
	{
		comboMatch = UserInput.ActionName.None;
		keyText = (Text) GameObject.Find ("KeyText").GetComponent<Text>();
		defaultKeyText = "Combo: ";
		keyText.text = defaultKeyText;
		animator = this.GetComponent<Animator> ();
		rigidBody2D = this.GetComponent<Rigidbody2D> ();
		userInput = new UserInput ();
		timeWalk = 0;
		timeNoInput = 0;
		timeNoInpLimit = 0.7f;
		timeWalkLimit = 2.0f;
		inputTimerStarted = false;
	}
 
	// Update is called once per frame
	void Update ()
	{
		commandUnits ();
		checkState ();
		displayKeyList ();
		//TODO: fix fourth combo letter not showing
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

		incrTimer (inputTimerStarted, userInput); // increment timer once input is accepted

		if (inputBool) { // if acceptable input entered
			comboMatch = userInput.compareAndReset();
			if (comboMatch == UserInput.ActionName.Walk) {
				animator.SetBool ("Walk", true);
				float y = rigidBody2D.velocity [1];
				rigidBody2D.velocity = new Vector2 (speed, y);
			} else if (comboMatch == UserInput.ActionName.Jump) {
				float x = rigidBody2D.velocity [0];
				rigidBody2D.velocity = new Vector2 (x, speed * 4);
			}
		}
			
	}

	void checkState() {
		if (animator.GetBool ("Walk") && timeWalk < timeWalkLimit) {
			timeWalk += Time.deltaTime;
			//Debug.Log ("Timewalking: " + timeWalk + "\n");
		} else if (timeWalk >= timeWalkLimit) {
			animator.SetBool ("Walk", false);
			timeWalk = 0;
			float y = rigidBody2D.velocity [1];
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, y);
		}
	}

	bool incrTimer(bool started, UserInput input) {
		if (started) {
			timeNoInput += Time.deltaTime;
		} else {
			return false;
		}

		if (timeNoInput > timeNoInpLimit) {
			timeNoInput = 0;
			input.reset ();
			Debug.Log ("RESET");
			return false;
		}
		return true;
	}

	void displayKeyList() {
		char[] keyList = userInput.getKeyList ();
		string s = Utility.toString (keyList);
		s = s.ToUpper ();
		Debug.Log ("length: " + s.Length.ToString() + " s: " + s);
		if (s.Length != 0) {
			setKeyText (defaultKeyText + s);
		} else {
			setKeyText (defaultKeyText);
		}
	}

	void setKeyText (string text) {
		keyText.text = text;
	}
}
