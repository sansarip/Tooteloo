using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Created by Sepehr Ansaripour
 * UserInput is designed to receive and verify user input keys
 * It should return an appropriate enum ActionName according to the user input
 */
public class UserInput
{
	public enum ActionName
	{
		None,
		Walk,
		Attack,
		Jump,
		Block}

	;
	private GameObject[] audioClips;

	private char[] keyList; 		// holds user input
	private int keyListPosition;
	private string[] comboList;		// holds combos as strings
	private int numCombos;			// size of comboList
	private int comboLength;		// length of each combo string

	// constructor
	public UserInput ()
	{
		audioClips = new GameObject[4];
		audioClips [0] = GameObject.Find ("KeySoundQ");
		audioClips [1] = GameObject.Find ("KeySoundW");
		audioClips [2] = GameObject.Find ("KeySoundE");
		audioClips [3] = GameObject.Find ("KeySoundR");
		numCombos = 4;
		keyList = new char[numCombos];
		keyListPosition = 0;
		comboList = generateComboList ();
		comboLength = 4;
	}
		

	public void setComboList (string[] newComboList)
	{
		comboList = newComboList;
	}

	public string[] getComboList ()
	{
		return comboList;
	}

	public char[] getKeyList()
	{
		return keyList;
	}

	private string[] generateComboList ()
	{
		return new string[4] { "qwer", "qwrr", "qwqw", "qwee" }; // combos here
	}

	private void resetComboList ()
	{
		comboList = new string[numCombos];
	}

	private void resetKeyList ()
	{
		keyList = new char[comboLength];
	}

	private void resetKeyListPos ()
	{
		keyListPosition = 0;
	}

	public bool checkInputKey ()
	{
		if (Input.GetKeyUp ("q")) {
			//Debug.Log ("Q");
			playSound(0);
			int index = keyListPosition;
			keyList [index] = 'q';
			keyListPosition++;
			return true;
		} else if (Input.GetKeyUp ("w")) {
			//Debug.Log ("W");
			playSound(1);
			int index = keyListPosition;
			keyList [index] = 'w';
			keyListPosition++;
			return true;
		} else if (Input.GetKeyUp ("e")) {
			//Debug.Log ("E");
			playSound(2);
			int index = keyListPosition;
			keyList [index] = 'e';
			keyListPosition++;
			return true;
		} else if (Input.GetKeyUp ("r")) {
			//Debug.Log ("R");
			playSound(3);
			int index = keyListPosition;
			keyList [index] = 'r';
			keyListPosition++;
			return true;
		} else if (Input.anyKey) {
			//resetKeyList ();
		}
		return false;
	}

	private void playSound(int index) {
		audioClips [index].GetComponent<AudioSource> ().Play();
	}
	/*
	 * compareEquals compares a char array with a string
	 * returns true if equal, else false
	 */
	private bool compareEquals (char[] charray1, string charray2)
	{
		int size = 0;
		int length1 = charray1.Length;
		int length2 = charray2.Length;
		size = length1 > length2 ? length2 : length1;
		for (int i = 0; i < size; i++) {
			if (charray1 [i] != charray2 [i]) {
				return false;
			}
		}
		return true;
	}

	public ActionName compare()
	{
		ActionName returnAction = ActionName.None;
		if (keyListPosition >= comboLength) { // if key list is fully populated
			for (int i = 0; i < comboList.Length; i++) { // iterate through combos
				if (compareEquals (keyList, comboList [i])) {
					switch (i) {
					case 0:
						returnAction = ActionName.Walk;
						break;
					case 1:
						returnAction = ActionName.Attack;
						break;
					case 2:
						returnAction = ActionName.Jump;
						break;
					case 3:
						returnAction = ActionName.Block;
						break;
					default:
						break;
					}
				}
			}
		}
		return returnAction;
	}

	public ActionName compareAndReset ()
	{
		ActionName returnAction = ActionName.None;
		if (keyListPosition >= comboLength) { // if key list is fully populated
			for (int i = 0; i < comboList.Length; i++) {
				if (compareEquals (keyList, comboList [i])) {
					switch (i) {
					case 0:
						returnAction = ActionName.Walk;
						break;
					case 1:
						returnAction = ActionName.Attack;
						break;
					case 2:
						returnAction = ActionName.Jump;
						break;
					case 3:
						returnAction = ActionName.Block;
						break;
					default:
						break;
					}
				}
			}
			resetKeyListPos ();
			resetKeyList ();
		}
		return returnAction;
	}

	public void reset() {
		resetKeyListPos ();
		resetKeyList ();
	}
}
