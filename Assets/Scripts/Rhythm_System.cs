using UnityEngine;
using System.Collections.Generic;


public class Rhythm_System : MonoBehaviour
{
	
	//divides the beats so to set the buffer for acceptance.
	//Can  be changed to numbers divisible by 4 to increase or decrease 
	//the length of the buffer.
	int division = 4;
	int count = 0;
	public static bool accept = false;

	public Rigidbody projectile;

	void Start()
	{
		//0.5 is 120 beats per minute
		InvokeRepeating("getInput", 0f, 0.5f/division);
	}

	void getInput()
	{
		count++;
		//start of buffer time
		if (count == division - 1) {
			accept = true;
		}

		//resets after buffer time has passed
		if (count > division) {
			count = 1;
			accept = false;
		}
	}
}