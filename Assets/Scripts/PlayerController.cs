using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private int comboPosition;
	public float speed;
	public int health;
	private char[] keyList;
	private Animator animator;
	private int timewalking;


	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		keyList = new char[4];
		comboPosition = 0;
		timewalking = 0;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("q")) {
			int index = comboPosition % 4;
			keyList [index] = 'q';
			comboPosition++;
			Debug.Log ("Q\n");
			Debug.Log ("comboPosition%4: " + comboPosition % 4 + "\n");
		}
		if (Input.GetKeyUp ("w")) {
			int index = comboPosition % 4;
			keyList [index] = 'w';
			comboPosition++;
			Debug.Log ("W\n");
			Debug.Log ("comboPosition%4: " + comboPosition % 4 + "\n");
		}
		if (Input.GetKeyUp ("e")) {
			int index = comboPosition % 4;
			keyList [index] = 'e';
			comboPosition++;
			Debug.Log ("E\n");
			Debug.Log ("comboPosition%4: " + comboPosition % 4 + "\n");
		}
		if (Input.GetKeyUp ("r")) {
			int index = comboPosition % 4;
			keyList [index] = 'r';
			comboPosition++;
			Debug.Log ("R\n");
			Debug.Log ("comboPosition%4: " + comboPosition % 4 + "\n");
		}
		Debug.Log ("keyList: " + keyList[0] + "|" + keyList[1] + "|" + keyList[2] + "|" + keyList[3] + "\n");
		if (keyList[0] == 'q' && keyList[1] == 'w' && keyList[2] == 'e' && keyList[3] == 'r') {
			animator.SetBool ("Walk", true);
			Debug.Log ("SUCCESS\n");
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
			keyList = new char[4];
		}
		if (animator.GetBool ("Walk") && timewalking == 0 || timewalking % 100 != 0) {
			timewalking += (int) (Time.deltaTime * 100);
			Debug.Log ("Timewalking: " + timewalking + "\n");
		} else if (timewalking != 0 && timewalking % 2 == 0) {
			animator.SetBool ("Walk", false);
			timewalking = 0;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		}
		if (comboPosition >= 0x7FFFFFFC) {																		// reset comboPosition if it nears 32 bit bounds
			Debug.Log ("RESET\n");					
			comboPosition %= 4;
		}
	}
}
