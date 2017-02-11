using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float speed;
	public int health;
	private Animator animator;
	private float timewalking;
    private float timeLimit;
    private UserInput userInput;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
        userInput = new UserInput();
		timewalking = 0;
        timeLimit = 2.0f;
	}
 
	// Update is called once per frame
	void Update () {
        userInput.checkInputKey();
        bool comboMatch = userInput.compareAndReset();
		if (comboMatch) {
			animator.SetBool ("Walk", true);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
		}
		if (animator.GetBool ("Walk") && timewalking < timeLimit) {
			timewalking += Time.deltaTime;
			Debug.Log ("Timewalking: " + timewalking + "\n");
		} else if (timewalking >= timeLimit) {
			animator.SetBool ("Walk", false);
			timewalking = 0;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		}
	}

    public class UserInput
    {
        private char[] keyList;
        private int keyListPosition;
        private string[] comboList;
        private int numCombos;
        private int comboLength;

        // constructor
        public UserInput()
        {
            numCombos = 4;
            keyList = new char[numCombos];
            keyListPosition = 0;
            comboList = generateComboList();
            Debug.Log("STRINGARRAY: " + comboList[0] + "\n");
            comboLength = 4;
        }

        public void setComboList(string[] newComboList)
        {
            comboList = newComboList;
        }

        public string[] getComboList()
        {
            return comboList;
        }

        private string[] generateComboList()
        {
            return new string[4] { "qwer", "qqwr", "qwqw", "were" };
        }

        private void resetComboList()
        {
            comboList = new string[numCombos];
        }

        private void resetKeyList()
        {
            keyList = new char[comboLength];
        }

        private void resetKeyListPos()
        {
            keyListPosition = 0;
        }
        public bool checkInputKey()
        {
            if (Input.GetKeyUp("q"))
            {
                int index = keyListPosition;
                keyList[index] = 'q';
                keyListPosition++;
                return true;
            }
            else if (Input.GetKeyUp("w"))
            {
                int index = keyListPosition;
                keyList[index] = 'w';
                keyListPosition++;
                return true;
            }
            else if (Input.GetKeyUp("e"))
            {
                int index = keyListPosition;
                keyList[index] = 'e';
                keyListPosition++;
                return true;
            }
            else if (Input.GetKeyUp("r"))
            {
                int index = keyListPosition;
                keyList[index] = 'r';
                keyListPosition++;
                return true;
            }
            return false;
        }

        private bool compareEquals(char[] charray1, string charray2)
        {
            int size = 0;
            int length1 = charray1.Length;
            int length2 = charray2.Length;
            size = length1 > length2 ? length2 : length1;
            for (int i = 0; i < size; i++)
            {
                if (charray1[i] != charray2[i])
                {
                    return false;
                }
            }
            return true;
        }


        public bool compareAndReset()
        {
            bool returnBool = false;
            if ( keyListPosition == comboLength )
            {
                for (int i = 0; i < comboList.Length; i++)
                {
                    if (compareEquals(keyList, comboList[i]))
                    {
                        resetKeyListPos();
                        resetKeyList();
                        returnBool = true;
                    }
                }
                resetKeyListPos();
                resetKeyList();
            }
            return returnBool;
        }
    }

}
