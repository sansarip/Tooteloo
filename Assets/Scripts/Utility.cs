using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

	/* toString returns string equivalent to char[], does not include '\0' character
	 * returns string equivalent of c
	 */
	public static string toString(char[] c) {
		string s = "";
		for (int i = 0; i < c.Length; i++) {
			if (c[i] == '\0') {
				break;
			}
			s += c[i];
		}
		return s;
	}

	/* ballisticVel calculates a ballistic trajectory
	 * returns Vector3 ballistic trajectory from origin to target at angle inclination
	 */
	public static Vector2 ballisticVel(Vector3 origin, Vector3 target, float angle) {
		Vector2 dir = target - origin;  // get target direction
		float h = dir.y;  // get height difference
		dir.y = 0;  // retain only the horizontal direction
		float dist = dir.magnitude ;  // get horizontal distance
		float a = angle * Mathf.Deg2Rad;  // convert angle to radians
		dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
		dist += h / Mathf.Tan(a);  // correct for small height differences
		float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a)); // calculate the velocity magnitude
		return vel * dir.normalized;
	}
}
